using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Genesis.Ambience.Scheduler
{
    public interface IEventScheduler
    {
        bool ScheduleEvent(IScheduleEvent evt, ulong timeCode);
        void AddProvider(IEventProviderInstance prov, ulong timeCode);
        uint TicksPerSec { get; }
    }

    public class EventSchedule : IDisposable
    {
        #region Constants
        public const uint DefaultMaxTime = 0;
        public const bool DefaultStopOnEmpty = true;
        #endregion

        #region Private Members
        private class Scheduler : IEventScheduler
        {
            private EventSchedule _parent;
            public Scheduler(EventSchedule parent)
            {
                _parent = parent;
            }

            #region IEventScheduler Members

            public bool ScheduleEvent(IScheduleEvent evt, ulong timeCode)
            {
                return _parent.ScheduleEvent(evt, timeCode);
            }

            public void AddProvider(IEventProviderInstance prov, ulong timeCode)
            {
                _parent.AddProvider(prov, timeCode);
            }

            public uint TicksPerSec
            {
                get { return _parent.TicksPerSec; }
            }

            #endregion
        }
        private Scheduler _scheduler;

        private Dictionary<ulong, List<IEventProviderInstance>> _providers = new Dictionary<ulong, List<IEventProviderInstance>>();
        private List<IScheduleEvent>[] _schedule;
        private List<IScheduleEvent> _active = new List<IScheduleEvent>();
        private List<IScheduleEvent> _waiting = new List<IScheduleEvent>();
        private List<IScheduleEvent> _doomed = new List<IScheduleEvent>();
        private ulong _currTimeCode = 0;
        private ulong _baseTimeCode = 0;
        private ulong _nextBaseTime;
        private int _timeSpan = 0;
        private int _nextSpan = 0;
        private int _offset = 0;
        private int _nextOffset = 0;
        private object _locker = new object();
        private Timer _timer = null;
        private AutoResetEvent _reset = new AutoResetEvent(false);
        private bool _initialized = false;
        #endregion

        public EventSchedule(int bufLen)
        {
            _scheduler = new Scheduler(this);

            _schedule = new List<IScheduleEvent>[bufLen];
            for (int i = 0; i < bufLen; i++)
                _schedule[i] = new List<IScheduleEvent>();
        }

        #region Properties
        private IList<IEventProvider> _model = new List<IEventProvider>();
        public IList<IEventProvider> Model
        {
            get { return _model; }
        }

        private uint _ticksPerSec = 1;
        public uint TicksPerSec
        {
            get { return _ticksPerSec; }
            set { _ticksPerSec = value; }
        }

        private bool _running = false;
        public bool IsRunning
        {
            get { return _running; }
        }

        public IEnumerable<IScheduleEvent> CurrentEvents
        {
            get { return _schedule[CurrentIndex]; }
        }

        public ulong CurrentTimeCode
        {
            get { return _currTimeCode; }
        }
        #endregion

        #region Events
        public delegate void Trigger(EventSchedule sched);
        public event Trigger Started;
        public event Trigger Finished;

        public delegate void TickEvent(EventSchedule sched, ulong newTimeCode);
        public event TickEvent Tick;

        public delegate void ScheduleExtend(EventSchedule sched);
        public event ScheduleExtend ScheduleExtended;
        #endregion

        #region Public Methods
        public void AddProvider(IEventProvider prov)
        {
            lock (_locker)
                AddProvider(prov.CreateInstance(), _currTimeCode);
        }

        public void ExecuteSchedule()
        {
            ExecuteSchedule(DefaultMaxTime, DefaultStopOnEmpty);
        }

        public void ExecuteSchedule(ulong maxTimeCode)
        {
            ExecuteSchedule(maxTimeCode, DefaultStopOnEmpty);
        }

        public void ExecuteSchedule(bool stopOnEmpty)
        {
            ExecuteSchedule(DefaultMaxTime, stopOnEmpty);
        }
        
        public void ExecuteSchedule(ulong maxTimeCode, bool stopOnEmpty)
        {
            if (_model == null || _model.Count == 0)
                return;

            InitializeSchedule();

            int msWait = (int)(1000.0d / _ticksPerSec);
            do
            {
                Next();
                Thread.Sleep(msWait);
            } while ((maxTimeCode == 0 || _currTimeCode < maxTimeCode) &&
                    (!stopOnEmpty || _providers.Count > 0 || _active.Count > 0 || _waiting.Count > 0));

            FinishSchedule();
        }

        public void Start()
        {
            Start(DefaultStopOnEmpty);
        }
        
        public void Start(bool stopOnEmpty)
        {
            if (_running || _model == null || (stopOnEmpty && (_model.Count == 0)))
                return;

            InitializeSchedule();

            int msWait = (int)(1000.0d / _ticksPerSec);

            _running = true;
            
            DoTick();
            if (Started != null)
                Started(this);

            _reset.Set();
            _timer = new Timer(Callback, this, 0, msWait);
        }

        public void Stop()
        {
            if (!_running)
                return;

            _timer.Dispose();

            int msWait = (int)(1000.0d / _ticksPerSec);
            _reset.WaitOne(msWait);

            FinishSchedule();
            _running = false;
            _initialized = false;

            if (Finished != null)
                Finished(this);
        }

        public void Initialize()
        {
            InitializeSchedule();
        }

        public IEnumerable<IScheduleEvent>[] GetActualFuture(out ulong currTime)
        {
            List<IScheduleEvent>[] copy;
            int idx;
            lock (_locker)
            {
                copy = Array.ConvertAll(_schedule, (li) => (new List<IScheduleEvent>(li)));
                currTime = _currTimeCode;
                idx = CurrentIndex;
            }

            if (idx < copy.Length / 2)
            {
                var result = new IEnumerable<IScheduleEvent>[copy.Length - idx];
                Array.Copy(copy, idx, result, 0, result.Length);
                return result;
            }
            else
            {
                var result = new IEnumerable<IScheduleEvent>[copy.Length - (idx - copy.Length / 2)];
                Array.Copy(copy, idx, result, 0, copy.Length - idx);
                Array.Copy(copy, 0, result, copy.Length - idx, copy.Length / 2);
                return result;
            }
        }
        #endregion

        #region Private Helpers
        private void Next()
        {
            lock (_locker)
            {
                int idx = CurrentIndex;
                foreach (IScheduleEvent evt in _schedule[idx])
                {
                    evt.Start();
                    _active.Add(evt);
                    _waiting.Remove(evt);
                }

                _doomed.Clear();
                foreach (IScheduleEvent evt in _active)
                {
                    evt.Update();
                    if (!evt.Active)
                    {
                        evt.Stop();
                        _doomed.Add(evt);
                    }
                }
                foreach (IScheduleEvent evt in _doomed)
                {
                    _active.Remove(evt);
                }

                ++_currTimeCode;

                if (!WithinBounds(_currTimeCode))
                    UpdateSchedule();
            }

            DoTick();
        }

        private void InitializeScheduleParameters()
        {
            _currTimeCode = 0;
            _nextOffset = 0;
            _nextSpan = _schedule.Length / 2;
            _nextBaseTime = 0;
        }

        private void UpdateScheduleParameters()
        {
            _offset = _nextOffset;
            _baseTimeCode = _nextBaseTime;
            _timeSpan = _nextSpan;

            // Adjust the window between the first and second half of the buffer.
            // This allows us to extend the schedule in place while still running.
            if (_nextOffset == 0)
            {
                _nextOffset = _schedule.Length / 2;
                _nextSpan = _schedule.Length - _nextOffset;
                _nextBaseTime += (ulong)_nextOffset;
            }
            else
            {
                _nextSpan = _schedule.Length / 2;
                _nextBaseTime += (ulong)(_schedule.Length - _nextOffset);
                _nextOffset = 0;
            }
        }

        private void ExtendSchedule()
        {
            ExtendSchedule(true);
        }
        
        private void ExtendSchedule(bool overwrite)
        {
            // Find events to schedule from our model.
            ulong max = _nextBaseTime + (ulong)_nextSpan;
            int span = _nextSpan;
            if (overwrite)
            {
                for (ulong timeCode = _nextBaseTime; timeCode < max; timeCode++, span--)
                {
                    _schedule[Index(timeCode)].Clear();
                }
            }

            for (ulong timeCode = _nextBaseTime; timeCode < max; timeCode++, span--)
            {
                if (!_providers.ContainsKey(timeCode))
                    continue;
                List<IEventProviderInstance> providers = _providers[timeCode];

                for (int i = 0; i < providers.Count; i++)
                {
                    providers[i].Next(_scheduler, timeCode, (ulong)span);
                }

                // Cleanup finished providers.
                _providers.Remove(timeCode);
            }

            if (ScheduleExtended != null)
                ScheduleExtended(this);
        }

        private void UpdateSchedule()
        {
            UpdateScheduleParameters();
            ExtendSchedule();
        }

        private void ClearSchedule()
        {
            foreach (List<IScheduleEvent> time in _schedule)
                time.Clear();
            _providers.Clear();
            _active.Clear();
            _waiting.Clear();
        }

        private void PopulateFromModel()
        {
            foreach (IEventProvider prov in _model)
                AddProvider(prov.CreateInstance(), 0);
        }

        private void InitializeSchedule()
        {
            if (_initialized)
                return;

            ClearSchedule();
            PopulateFromModel();
            InitializeScheduleParameters();
            ExtendSchedule();
            UpdateSchedule();

            _initialized = true;
        }

        private void FinishSchedule()
        {
            foreach (IScheduleEvent evt in _active)
                evt.Stop();
            foreach (IScheduleEvent evt in _waiting)
                evt.Stop();

            _currTimeCode = 0;
        }

        private int Index(ulong timeCode)
        {
            return ((int)(timeCode - _baseTimeCode) + _offset) % _schedule.Length;
        }

        private int CurrentIndex
        {
            get { return Index(_currTimeCode); }
        }

        private bool ScheduleEvent(IScheduleEvent evt, ulong timeCode)
        {
            // Make sure the schedule is in the right window to add this.
            if (timeCode < _baseTimeCode ||
                timeCode - _baseTimeCode >= (ulong)_schedule.Length)
                return false;

            // Insert event into the schedule.
            int idx = Index(timeCode);
            _schedule[idx].Add(evt);

            _waiting.Add(evt);
            return true;
        }

        private void AddProvider(IEventProviderInstance prov, ulong timeCode)
        {
            if (!_providers.ContainsKey(timeCode))
                _providers[timeCode] = new List<IEventProviderInstance>();

            if (WithinBounds(timeCode))
            {
                ulong span = (ulong)_timeSpan - (timeCode - _baseTimeCode);
                prov.Next(_scheduler, timeCode, span);
            }
            else if (WithinExtension(timeCode))
            {
                ulong span = (ulong)_nextSpan - (timeCode - _nextBaseTime);
                prov.Next(_scheduler, timeCode, span);
            }
            else
            {
                _providers[timeCode].Add(prov);
            }
        }

        private void DoTick()
        {
            if (Tick != null)
                Tick(this, _currTimeCode);
        }

        private bool WithinBounds(ulong timeCode)
        {
            return (timeCode >= _baseTimeCode) && (timeCode < _baseTimeCode + (ulong)_timeSpan);
        }

        private bool WithinExtension(ulong timeCode)
        {
            return (timeCode >= _nextBaseTime) && (timeCode < _nextBaseTime + (ulong)_nextSpan);
        }
        #endregion

        #region Timer Callback
        private static void Callback(object o)
        {
            EventSchedule sched = o as EventSchedule;
            if (sched == null)
                return;

            sched._reset.Reset();
            sched.Next();
            sched._reset.Set();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            foreach (IScheduleEvent evt in _active)
                evt.Dispose();
            foreach (IScheduleEvent evt in _waiting)
                evt.Dispose();
        }
        #endregion
    }
}
