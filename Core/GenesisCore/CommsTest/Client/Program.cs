using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Common.API.Comms;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace Client
{
    class Program : AGenesisApplication, IClientChannelDetailProvider, IObserver
    {
        bool _running = true;
        AutoResetEvent _evt = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            Program _this = new Program();

            _this._hub.SetChannel(_this);
            _this._hub.Initialize(_this);

            uint serverId = _this._hub.Hub.GetRegisteredAppIDs("MyServer").First();
            Console.WriteLine("Looking for server ID: {0}", serverId.ToString());
            
            var id = new TriggerID("Test1", serverId);
            if (!_this._hub.Hub.ConnectObserver(id, _this))
                Console.WriteLine("Failed to connect observer.");

            while (_this._running)
            {
                _this._evt.WaitOne();
                Console.WriteLine("Tick");
                _this._evt.Reset();
            }

            Console.Write("Done...");
            Console.ReadLine();
        }

        ClientHubProvider _hub = new ClientHubProvider();
        protected override IHubProvider HubProvider
        {
            get { return _hub; }
        }

        public override string AppName
        {
            get { return "CommsTestClient"; }
        }

        public override void OnDisconnect()
        {
            _running = false;
            _evt.Set();
        }

        public ChannelMode Mode
        {
            get { return ChannelMode.TCPIP; }
        }

        public override ChannelMode Channel
        {
            get { return Mode; }
        }

        public string URL
        {
            get { return "tcp://localhost:13584"; }
        }

        public uint ApplicationID
        {
            get { return AppID; }
        }

        public void Trigger(ITrigger sender, IDictionary<string, object> parms)
        {
            _evt.Set();
        }
    }
}
