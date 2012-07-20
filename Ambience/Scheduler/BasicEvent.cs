using System;

namespace Genesis.Ambience.Scheduler
{
    public class BasicEvent : IScheduleEvent
    {
        public class Provider : AEventProvider
        {
            public class Instance : AEventProviderInstance<Provider>
            {
                public Instance(Provider parent)
                    : base(parent)
                {
                }

                public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
                {
                    sched.ScheduleEvent(new BasicEvent(this._parent), currTimeCode);
                    return false;
                }
            }

            public Provider(string name) : base(name) { }

            #region IEventProvider Members

            public override bool DependsOn(IEventProvider dependent)
            {
                throw new NotImplementedException();
            }

            public override IEventProviderInstance CreateInstance()
            {
                return new Instance(this);
            }

            #endregion
        }

        private static int NEXTID = 1;
        private int _id = NEXTID++;

        public BasicEvent(Provider src)
        {
            _source = src;
        }
        public BasicEvent(Provider src, ulong len)
        {
            _source = src;
            _length = len;
        }

        #region IScheduleEvent Members

        private ulong _length = 1;
        public ulong Length
        {
            get { return _length; }
        }
        
        private bool _active = false;
        public bool Active
        {
            get { return _active; }
        }

        private Provider _source;
        public IEventProvider Source
        {
            get { return _source; }
        }

        public void Start()
        {
            _active = true;
            Console.WriteLine("BasicEvent {0} Start()", _id);
        }

        private ulong _counter = 0;
        public void Update()
        {
            if (Length == _counter++)
                _active = false;
        }

        public void Stop()
        {
            Console.WriteLine("BasicEvent {0} Stop()", _id);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
        }
        #endregion
    }
}
