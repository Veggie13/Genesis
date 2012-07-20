using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.API.Comms
{
    public class DefaultHubProvider : IHubProvider
    {
        private Hub _hub = null;

        #region IHubProvider Members

        public bool Initialize(IGenesisApplication app)
        {
            if (_hub == null)
                _hub = new Hub();

            _hub.RegisterApplication(app);

            return true;
        }

        public void Finish(IGenesisApplication app)
        {
            if (_hub != null)
                _hub.UnregisterApplication(app.AppID);
        }

        public IHub Hub
        {
            get { return _hub; }
        }

        #endregion
    }
}
