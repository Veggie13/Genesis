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
    public class Program : AGenesisApplication, IClientChannelDetailProvider
    {
        static Program _this = new Program();

        static void Main(string[] args)
        {
            //try
            {
                ClientHubProvider hubProv = _this._hubProv;
                hubProv.Initialize(_this);

                var id = new TriggerID("Test1", _this.AppID);
                hubProv.Hub.RegisterTrigger(id, _this._test1);
                id = new TriggerID("Test2", _this.AppID);
                hubProv.Hub.RegisterTrigger(id, _this._test2);

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

                hubProv.Finish(_this);
            }
            /*catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }*/
        }

        ClientHubProvider _hubProv = new ClientHubProvider();
        BasicTrigger _test1 = new BasicTrigger("Test1");
        BasicTrigger _test2 = new BasicTrigger("Test2");

        public Program()
        {
            _hubProv.SetChannel(this);
        }

        void hubProv_ApplicationRegistered(uint appId)
        {
            Console.WriteLine("Application {0} registered.", appId);
        }

        #region AGenesisApplication Members

        public override string AppName
        {
            get { return "MyServer"; }
        }

        public override ChannelMode Channel
        {
            get { return ChannelMode.TCPIP; }
        }

        public override void OnDisconnect()
        {
            
        }

        protected override IHubProvider HubProvider
        {
            get { return _hubProv; }
        }

        #endregion

        bool _swap = false;
        private void EmitSignal()
        {
            if (_swap)
                _test2.Emit(null);
            else
                _test1.Emit(null);
            _swap = !_swap;
        }

        public ChannelMode Mode
        {
            get { return Channel; }
        }

        public string URL
        {
            get { return "tcp://localhost:13584"; }
        }
    }
}
