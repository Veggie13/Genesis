using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;

namespace Genesis.Common.API.Comms
{
    public delegate void ConnectionTerminatingEvent();

    public class ClientHubProvider : MarshalByRefObject, IHubProvider
    {
        private IGenesisApplication _app = null;
        private HubProxy _hub = null;
        private IChannel _chan = null;
        private string _url;

        public bool SetChannel(IClientChannelDetailProvider details)
        {
            if (_hub != null)
                return false;

            _chan = ChannelFactory.CreateClientChannel(details);
            _url = details.URL + "/" + ServerHubProvider.ObjectName;
            return true;
        }

        //public event ConnectionTerminatingEvent ConnectionTerminating;

        #region IHubProvider Members

        public bool Initialize(IGenesisApplication app)
        {
            if (_hub != null)
                return false;

            ChannelServices.RegisterChannel(_chan, false);
            Hub mainHub = (Hub)Activator.GetObject(typeof(Hub), _url);
            _hub = new HubProxy(mainHub);
            _hub.RegisterApplication(app);
            mainHub.AboutToDie += new Hub.DeathBedEvent(OnDeathBed);
            _app = app;

            return true;
        }

        public void Finish(IGenesisApplication app)
        {
            if (_hub != null)
            {
                _hub.UnregisterApplication(app.AppID);
                Disconnect();
            }
        }

        public IHub Hub
        {
            get { return _hub; }
        }

        #endregion

        public void OnDeathBed()
        {
            _app.OnDisconnect();
            Disconnect();
        }

        private void Disconnect()
        {
            _hub.DisconnectHub();
            _app.AppID = 0;
            _hub = null;
            ChannelServices.UnregisterChannel(_chan);
            _app = null;
        }

    }
}
