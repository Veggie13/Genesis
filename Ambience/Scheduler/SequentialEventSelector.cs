using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class SequentialEventSelector : AEventProvider
    {
        public class Instance : AEventProviderInstance<SequentialEventSelector>
        {
            private int _current = 0;

            public Instance(SequentialEventSelector parent)
                : base(parent, parent)
            {
            }

            public Instance(SequentialEventSelector parent, IEventProvider src)
                : base(parent, src)
            {
            }

            public IEnumerable<IEventProviderInstance> SubordinateSequence
            {
                get;
                private set;
            }
            
            public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
            {
                if (ParentModel.Sequence.Count == 0)
                    return false;
                if (SubordinateSequence == null)
                    SubordinateSequence = ParentModel.Sequence.Select(p => p.CreateInstance(this.Source));

                sched.AddProvider(SubordinateSequence.ElementAt(_current++), currTimeCode);
                _current %= ParentModel.Sequence.Count;
                return true;
            }
        }

        public SequentialEventSelector(string name) : base(name) { }

        private List<IEventProvider> _sequence = new List<IEventProvider>();
        public IList<IEventProvider> Sequence
        {
            get { return _sequence; }
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
