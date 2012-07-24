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

                public Instance(Provider parent, IEventProvider src)
                    : base(parent, src)
                {
                }

                public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
                {
                    sched.ScheduleEvent(new BasicEvent(this.Source, this.Model.Name), currTimeCode);
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

            public override IEventProviderInstance CreateInstance(IEventProvider src)
            {
                return new Instance(this, src);
            }

            #endregion
        }

        private static int NEXTID = 1;
        private int _id = NEXTID++;

        public BasicEvent(IEventProvider src, string name)
        {
            _source = src;
            _name = name;
        }
        public BasicEvent(IEventProvider src, string name, ulong len)
        {
            _source = src;
            _name = name;
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

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private IEventProvider _source;
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
