using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class SimultaneousEventProvider : AEventProvider
    {
        public class Instance : AEventProviderInstance<SimultaneousEventProvider>
        {
            public Instance(SimultaneousEventProvider parent)
                : base(parent, parent)
            {
            }

            public Instance(SimultaneousEventProvider parent, IEventProvider src)
                : base(parent, src)
            {
            }

            public IEnumerable<IEventProviderInstance> SubordinateGroup
            {
                get;
                private set;
            }

            public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
            {
                if (ParentModel.Group.Count == 0)
                    return false;
                if (SubordinateGroup == null)
                    SubordinateGroup = ParentModel.Group.Select(p => p.CreateInstance(this.Source));

                foreach (IEventProviderInstance inst in SubordinateGroup)
                    sched.AddProvider(inst, currTimeCode);
                return true;
            }
        }

        public SimultaneousEventProvider(string name) : base(name) { }

        private List<IEventProvider> _group = new List<IEventProvider>();
        public IList<IEventProvider> Group
        {
            get { return _group; }
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
