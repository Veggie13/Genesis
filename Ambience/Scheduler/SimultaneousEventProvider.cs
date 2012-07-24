using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class SimultaneousEventProvider : AEventProvider, IEventProviderInstance
    {
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
            return this;
        }

        public override IEventProviderInstance CreateInstance(IEventProvider src)
        {
            return this;
        }

        #endregion

        #region IEventProviderInstance Members

        public bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
        {
            if (_group == null || _group.Count == 0)
                return false;

            foreach (IEventProvider prov in _group)
                sched.AddProvider(prov.CreateInstance(), currTimeCode);
            return true;
        }

        public IEventProvider Model
        {
            get { return this; }
        }

        private IEventProvider _src;
        public IEventProvider Source
        {
            get { return (_src == null) ? Model : _src; }
        }

        #endregion
    }
}
