using System;
using System.Collections.Generic;
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

            public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
            {
                if (_parent.Selection.Count == 0)
                    return false;

                int sel = Rand.Next(_parent.Selection.Count);
                sched.AddProvider(_parent.Selection[sel].CreateInstance(this.Source), currTimeCode);
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

        #endregion
    }
}
