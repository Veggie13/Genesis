using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Common.API.Comms;

namespace Genesis.Ambience.Scheduler
{
    public class EventProviderActivator : AObserver
    {
        public EventProviderActivator(uint appId)
        {
            _appId = appId;
        }

        private EventSchedule _sched = null;
        public EventSchedule Schedule
        {
            get { return _sched; }
            set { _sched = value; }
        }

        private IEventProvider _prov = null;
        public IEventProvider Provider
        {
            get { return _prov; }
            set { _prov = value; }
        }

        public void Activate()
        {
            if (_sched == null || _prov == null)
                return;

            _sched.AddProvider(_prov);
        }

        #region AObserver
        public override void Trigger(ITrigger sender, IDictionary<string, object> parms)
        {
            Activate();
        }

        private uint _appId = 0;
        public override uint ApplicationID
        {
            get { return _appId; }
        }
        #endregion
    }
}
