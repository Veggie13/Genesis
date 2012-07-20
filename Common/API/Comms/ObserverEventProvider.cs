using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.API.Comms
{
    public class ObserverEventProvider : AObserver
    {
        public ObserverEventProvider(IGenesisApplication app)
        {
            _appId = app.AppID;
        }

        private uint _appId = 0;
        public override uint ApplicationID
        {
            get { return _appId; }
        }

        public override void Trigger(ITrigger sender, IDictionary<string, object> parms)
        {
            if (Triggered != null)
                Triggered(sender, parms);
        }

        public delegate void TriggeredEvent(ITrigger sender, IDictionary<string, object> parms);
        public event TriggeredEvent Triggered;
    }
}
