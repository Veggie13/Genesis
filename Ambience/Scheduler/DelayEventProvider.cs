using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class DelayEventProvider : AEventProvider
    {
        public class Instance : AEventProviderInstance<DelayEventProvider>
        {
            public Instance(DelayEventProvider parent)
                : base(parent)
            {
            }

            public Instance(DelayEventProvider parent, IEventProvider src)
                : base(parent, src)
            {
            }

            public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
            {
                if (_parent.Subordinate != null)
                    sched.AddProvider(_parent.Subordinate.CreateInstance(), currTimeCode + (ulong)_parent.Delay);
                return false;
            }
        }

        public DelayEventProvider(string name) : base(name) { }

        private IEventProvider _subordinate = null;
        public IEventProvider Subordinate
        {
            get { return _subordinate; }
            set { _subordinate = value; }
        }

        private uint _delay = 0;
        public uint Delay
        {
            get { return _delay; }
            set { _delay = value; }
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
    }
}
