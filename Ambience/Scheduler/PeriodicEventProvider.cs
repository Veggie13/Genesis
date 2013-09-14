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
                : base(parent, parent)
            {
                ChooseNextPoint();
            }

            public Instance(PeriodicEventProvider parent, IEventProvider src)
                : base(parent, src)
            {
                ChooseNextPoint();
            }

            public IEventProviderInstance SubordinateInstance
            {
                get;
                private set;
            }

            private void ChooseNextPoint()
            {
                _nextPoint = _currBase
                    + (ulong)ParentModel.Period
                    + (ulong)Rand.Next(1 + 2 * (int)ParentModel.Variance)
                    - (ulong)ParentModel.Variance;
                _currBase += (ulong)ParentModel.Period;
            }

            public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
            {
                if (ParentModel.Subordinate == null)
                    return false;
                if (SubordinateInstance == null)
                    SubordinateInstance = ParentModel.Subordinate.CreateInstance(this.Source);

                sched.AddProvider(SubordinateInstance, _nextPoint);
                ChooseNextPoint();
                sched.AddProvider(this, _nextPoint);
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

        private uint _period = 1;
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
    }
}
