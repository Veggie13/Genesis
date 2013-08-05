using System;
using System.Collections.Generic;
using System.Text;
using Genesis.Ambience.Scheduler;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace Genesis.Ambience.Audio
{
    public class SoundEvent : IScheduleEvent
    {
        public class Provider : AEventProvider
        {
            private ResourceManager _manager;

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

            public Provider(string name, ResourceManager mgr, string resName)
                : base(name)
            {
                ResourceName = resName;
                _manager = mgr;
            }

            public string ResourceName
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

            #endregion

            private List<SoundEvent> _all = new List<SoundEvent>();
            private SoundEvent CreateEvent(uint timePerSecond)
            {
                var res = _manager.GetResource(ResourceName);
                SoundEvent evt = new SoundEvent(this, res, timePerSecond);
                _all.Add(evt);
                return evt;
            }
        }

        private SoundResource _resource;
        private static int s_next = 1;
        private int _id = s_next++;
        
        public SoundEvent(Provider src, SoundResource res, uint timePerSecond)
        {
            Console.WriteLine("{0} created", _id);
            _source = src;
            _resource = res;
            _length = (ulong)Math.Ceiling(res.Length * (double)timePerSecond);
            if (_length < 1)
                _length = 1;
            res.Init();
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
