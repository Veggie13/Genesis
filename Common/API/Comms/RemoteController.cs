using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Genesis.Common.API.Comms
{
    /*public class RemoteController : Hub
    {
        private Thread _server;

        public RemoteController()
        {
        }

        public bool StartServer()
        {
            if (_server != null)
                return false;

            _server = new Thread(this.MainThread);
            _server.Start();
            return true;
        }

        private void MainThread()
        {
        }

        #region IController Members

        public uint ApplicationID
        {
            get { return (uint)AppID.Core; }
        }

        public ICollection<TriggerID> GetTriggers()
        {
            throw new NotImplementedException();
        }

        public ICollection<string> GetLocalTriggerNames()
        {
            throw new NotImplementedException();
        }

        public bool TriggerNameExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool TriggerNameExists(string name, uint appId)
        {
            throw new NotImplementedException();
        }

        public bool RegisterTrigger(ITrigger trig)
        {
            throw new NotImplementedException();
        }

        #endregion
    }*/
}
