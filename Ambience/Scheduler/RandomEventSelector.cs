using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class RandomEventSelector : AEventProvider, IEventProviderInstance
    {
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
            return this;
        }

        #endregion

        #region IEventProviderInstance Members

        public bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
        {
            if (_selection == null || _selection.Count == 0)
                return false;

            int sel = Rand.Next(_selection.Count);
            sched.AddProvider(_selection[sel].CreateInstance(), currTimeCode);
            return true;
        }

        public IEventProvider Model
        {
            get { return this; }
        }

        #endregion
    }
}
