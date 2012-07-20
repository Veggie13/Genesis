using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class PeriodicEventProvider : AEventProvider
    {
        public class Instance : AEventProviderInstance<PeriodicEventProvider>
        {
            private static readonly Random Rand = new Random();

            private ulong _nextPoint, _currBase = 0;

            public Instance(PeriodicEventProvider parent)
                : base(parent)
            {
                ChooseNextPoint();
            }

            private void ChooseNextPoint()
            {
                _nextPoint = _currBase
                    + (ulong)_parent.Period
                    + (ulong)Rand.Next(1 + 2 * (int)_parent.Variance)
                    - (ulong)_parent.Variance;
                _currBase += (ulong)_parent.Period;
            }

            public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
            {
                if (_parent.Subordinate == null)
                    return false;
                sched.AddProvider(_parent.Subordinate.CreateInstance(), _nextPoint);
                sched.AddProvider(this, _nextPoint);
                ChooseNextPoint();
                return true;
            }
        }

        public PeriodicEventProvider(string name) : base(name) { }

        private IEventProvider _subordinate = null;
        public IEventProvider Subordinate
        {
            get { return _subordinate; }
            set { _subordinate = value; }
        }

        private uint _period = 0;
        public uint Period
        {
            get { return _period; }
            set { _period = value; }
        }

        private uint _variance = 0;
        public uint Variance
        {
            get { return _variance; }
            set { _variance = value; }
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

        #endregion
    }
}
