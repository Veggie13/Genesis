using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using Genesis.Common.API.Comms;

namespace CommsTest
{
    class Program : AGenesisApplication
    {
        static Program _this = new Program();

        static void Main(string[] args)
        {
            IChannel chan = ChannelFactory.CreateServerChannel(new ServerChannelDetails(ChannelMode.TCPIP, 13584));

            _this._hubProv = new ServerHubProvider();
            _this._hubProv.Hub.ApplicationRegistered += new RegisterApplicationEvent(_this.hubProv_ApplicationRegistered);
            _this._hubProv.AddChannel(chan);
            _this._hubProv.Initialize(_this);

            var id = new TriggerID("Test", _this.AppID);
            _this._hubProv.Hub.RegisterTrigger(id, _this._trig);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Press any key...");
                Console.ReadLine();

                //TriggerID id = new TriggerID("Test", 2);
                //hubProv.Hub.ConnectObserver(id, _this);
                _this.EmitSignal();
            }

            Console.WriteLine("Signal emitted.");
            Console.ReadLine();

            _this._hubProv.Finish(_this);
        }

        BasicTrigger _trig = new BasicTrigger("Test");

        void hubProv_ApplicationRegistered(uint appId)
        {
            Console.WriteLine("Application {0} registered.", appId);
        }

        #region AGenesisApplication Members

        private ServerHubProvider _hubProv;
        protected override IHubProvider HubProvider
        {
            get { return _hubProv; }
        }

        public override string AppName
        {
            get { return "MyServer"; }
        }

        public override ChannelMode Channel
        {
            get { return ChannelMode.Local; }
        }

        public override void OnDisconnect()
        {
            
        }

        #endregion

        private void EmitSignal()
        {
            _trig.Emit(null);
        }
    }
}
