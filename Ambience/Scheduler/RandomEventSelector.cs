using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class RandomEventSelector : AEventProvider
    {
        public class Instance : AEventProviderInstance<RandomEventSelector>
        {
            public Instance(RandomEventSelector parent)
                : base(parent, parent)
            {
            }

            public Instance(RandomEventSelector parent, IEventProvider src)
                : base(parent, src)
            {
            }

            public IEnumerable<IEventProviderInstance> SubordinateSelection
            {
                get;
                private set;
            }

            public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
            {
                if (ParentModel.Selection.Count == 0)
                    return false;
                if (SubordinateSelection == null)
                    SubordinateSelection = ParentModel.Selection.Select(p => p.CreateInstance(this.Source));

                int sel = Rand.Next(ParentModel.Selection.Count);
                sched.AddProvider(SubordinateSelection.ElementAt(sel), currTimeCode);
                return true;
            }
        }

        private static readonly Random Rand = new Random();

        public RandomEventSelector(string name) : base(name) { }

        private List<IEventProvider> _selection = new List<IEventProvider>();
        public IList<IEventProvider> Selection
        {
            get { return _selection; }
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
