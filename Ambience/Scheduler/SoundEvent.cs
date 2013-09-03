using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class SoundEvent : IScheduleEvent
    {
        #region Helper Classes
        public interface IResourceProvider
        {
            IResource GetResource(string resName);
            List<string> GetAllSounds();
        }

        public interface IResource
        {
            event Action PlaybackStopped;

            double Length { get; }
            string FullName { get; }

            void Play();
            void Stop();
        }

        public class Provider : AEventProvider
        {
            public class Instance : AEventProviderInstance<Provider>
            {
                public Instance(Provider parent)
                    : base(parent, parent)
                {
                }

                public Instance(Provider parent, IEventProvider src)
                    : base(parent, src)
                {
                }

                public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
                {
                    sched.ScheduleEvent(_parent.CreateEvent(sched.TicksPerSec), currTimeCode);
                    return false;
                }
            }

            public Provider(string name, IResourceProvider mgr, string resName)
                : base(name)
            {
                ResourceName = resName;
                ResourceProvider = mgr;
            }

            public string ResourceName
            {
                get;
                set;
            }

            public IResourceProvider ResourceProvider
            {
                get;
                private set;
            }

            #region IEventProvider Members

            public override bool DependsOn(IEventProvider dependent)
            {
                throw new NotImplementedException();
            }

            public override IEventProviderInstance CreateInstance()
            {
                return new Instance(this);
            }

            public override IEventProviderInstance CreateInstance(IEventProvider src)
            {
                return new Instance(this, src);
            }

            #region IVisitable
            public override void Accept(IEventProviderVisitor visitor)
            {
                visitor.Visit(this);
            }
            #endregion

            #endregion

            private List<SoundEvent> _all = new List<SoundEvent>();
            private SoundEvent CreateEvent(uint timePerSecond)
            {
                var res = ResourceProvider.GetResource(ResourceName);
                SoundEvent evt = new SoundEvent(this, res, timePerSecond);
                _all.Add(evt);
                return evt;
            }
        }
        #endregion

        #region Class Members
        private IResource _resource;
        private static int s_next = 1;
        private int _id = s_next++;
        #endregion

        public SoundEvent(Provider src, IResource res, uint timePerSecond)
        {
            Console.WriteLine("{0} created", _id);
            Started = false;
            _source = src;
            _resource = res;
            _length = (ulong)Math.Ceiling(res.Length * (double)timePerSecond);
            if (_length < 1)
                _length = 1;
        }

        private void PlaybackFinished()
        {
            _active = false;
        }

        #region IScheduleEvent Members

        private ulong _length;
        public ulong Length
        {
            get { return _length; }
        }

        private bool _active = false;
        public bool Active
        {
            get { return _active; }
        }

        public bool Started
        {
            get;
            private set;
        }

        public string Name
        {
            get { return _source.Name; }
        }

        private Provider _source;
        public IEventProvider Source
        {
            get { return _source; }
        }

        public void Start()
        {
            //Console.WriteLine("Event started");
            _active = true;
            Started = true;
            _resource.PlaybackStopped += PlaybackFinished;
            _resource.Play();
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }

        public void Stop()
        {
            //Console.WriteLine("Event Stopped");
            _resource.PlaybackStopped -= PlaybackFinished;
            _resource.Stop();
            _active = false;
            Console.WriteLine("{0} disposed", _id);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
        }
        #endregion
    }
}
