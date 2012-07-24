using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public class SequentialEventSelector : AEventProvider, IEventProviderInstance
    {
        private int _current = 0;

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
            if (Sequence == null || Sequence.Count == 0)
                return false;

            sched.AddProvider(Sequence[_current++].CreateInstance(), currTimeCode);
            _current %= Sequence.Count;
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
