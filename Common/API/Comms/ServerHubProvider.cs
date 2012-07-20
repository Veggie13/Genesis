using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net.Sockets;

namespace Genesis.Common.API.Comms
{
    public class ServerHubProvider : IHubProvider
    {
        public const string ObjectName = "GenesisHub";


        private Hub _hub = null;
        private List<IChannel> _chans = new List<IChannel>();
        
        public IEnumerable<IChannel> Channels
        {
            get { return _chans; }
        }

        public void AddChannel(ServerChannelDetails details)
        {
            AddChannel(ChannelFactory.CreateServerChannel(details));
        }
        
        public void AddChannel(IChannel chan)
        {
            if (!_chans.Contains(chan))
            {
                _chans.Add(chan);
                if (_hub != null)
                {
                    ChannelServices.RegisterChannel(chan, false);
                }
            }
        }

        public void RemoveChannel(IChannel chan)
        {
            if (_chans.Contains(chan))
            {
                _chans.Remove(chan);
                if (_hub != null)
                {
                    ChannelServices.UnregisterChannel(chan);
                }
            }
        }

        public IEnumerable<IGenesisApplication> Applications
        {
            get { return _hub.Applications; }
        }

        public void KickApp(uint appId)
        {
            try
            {
                var ctx = new HubDisconnectContext();
                _hub[appId].Kick(ctx);
                ctx.Release();
            }
            catch (SocketException)
            {
                // do nothing
            }
        }

        #region IHubProvider Members

        public bool Initialize(IGenesisApplication app)
        {
            if (_hub != null)
                return false;

            _hub = new Hub();
            _hub.RegisterApplication(app);

            foreach (IChannel chan in _chans)
                ChannelServices.RegisterChannel(chan, false);
            RemotingServices.Marshal(_hub, ObjectName);

            return true;
        }

        public void Finish(IGenesisApplication app)
        {
            if (_hub != null)
            {
                _hub.UnregisterApplication(app.AppID);
                app.AppID = 0;

                _hub.Dispose();
                RemotingServices.Disconnect(_hub);
                _hub = null;

                foreach (IChannel chan in _chans)
                    ChannelServices.UnregisterChannel(chan);
            }
        }

        public IHub Hub
        {
            get { return _hub; }
        }

        #endregion

    }
}
