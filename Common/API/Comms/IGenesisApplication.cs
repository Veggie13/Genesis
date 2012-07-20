using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Genesis.Common.API.Comms
{
    public interface IGenesisApplication : IGenesisObject
    {
        string AppName { get; }
        uint AppID { get; set; }
        ChannelMode Channel { get; }

        void Kick(HubDisconnectContext ctx);
        void OnDisconnect();
    }

    public abstract class AGenesisApplication : GenesisObject, IGenesisApplication
    {
        protected abstract IHubProvider HubProvider { get; }

        #region IGenesisApplication Members
        public abstract string AppName { get; }
        public abstract ChannelMode Channel { get; }
        public abstract void OnDisconnect();

        private uint _appId = 0;
        public uint AppID
        {
            get { return _appId; }
            set { _appId = value; }
        }

        public void Kick(HubDisconnectContext ctx)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.KickThread));
            t.Start(ctx);
        }
        #endregion

        private void KickThread(object o)
        {
            HubDisconnectContext ctx = o as HubDisconnectContext;
            ctx.Lock();
            HubProvider.Finish(this);
        }
    }
}
