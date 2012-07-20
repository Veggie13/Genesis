using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Genesis.Common.API.Comms;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;

namespace Genesis.Applications.Core
{
    public class App : AGenesisApplication
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            App app = new App();
            app.Run();
        }

        public void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(this));
        }

        #region AGenesisApplication
        #region IGenesisApplication
        public override string AppName
        {
            get { return "GenesisCore"; }
        }

        public override ChannelMode Channel
        {
            get { return ChannelMode.Local; }
        }

        public override void OnDisconnect()
        {

        }
        #endregion

        protected override IHubProvider HubProvider
        {
            get { return _hub; }
        }
        #endregion

        private ServerHubProvider _hub = null;
        public IHub Hub
        {
            get { return _hub.Hub; }
        }

        public bool IsRunning
        {
            get { return (_hub != null); }
        }

        private IChannel _tcp = null;
        private IChannel _ipc = null;
        
        private bool _useTCP = false;
        public bool UseTCP
        {
            get { return _useTCP; }
            set
            {
                if (IsRunning && _useTCP != value)
                {
                    if (_useTCP)
                    {
                        var tcpApps = _hub.Applications;
                        foreach (IGenesisApplication app in tcpApps)
                        {
                            if (app.Channel == ChannelMode.TCPIP)
                                _hub.KickApp(app.AppID);
                        }

                        _hub.RemoveChannel(_tcp);
                    }
                    else
                    {
                        SetupTCP();
                    }
                }

                _useTCP = value;
            }
        }

        private int _tcpPort = 0;
        public int TCPPort
        {
            get { return _tcpPort; }
            set { _tcpPort = value; }
        }

        private bool _usePipes = false;
        public bool UsePipes
        {
            get { return _usePipes; }
            set
            {
                if (IsRunning && _usePipes != value)
                {
                    if (_usePipes)
                    {
                        var ipcApps = _hub.Applications;
                        foreach (IGenesisApplication app in ipcApps)
                        {
                            if (app.Channel == ChannelMode.NamedPipe)
                                _hub.KickApp(app.AppID);
                        }

                        _hub.RemoveChannel(_ipc);
                    }
                    else
                    {
                        SetupPipes();
                    }
                }

                _usePipes = value;
            }
        }

        private int _pipePort = 0;
        public int NamedPipesPort
        {
            get { return _pipePort; }
            set { _pipePort = value; }
        }

        private void SetupTCP()
        {
            _tcp = ChannelFactory.CreateServerChannel(
                new ServerChannelDetails(ChannelMode.TCPIP, _tcpPort));
            _hub.AddChannel(_tcp);
        }

        private void SetupPipes()
        {
            _ipc = ChannelFactory.CreateServerChannel(
                new ServerChannelDetails(ChannelMode.NamedPipe, _pipePort));
            _hub.AddChannel(_ipc);
        }

        public void StartServer()
        {
            _hub = new ServerHubProvider();

            if (UsePipes)
                SetupPipes();
            if (UseTCP)
                SetupTCP();

            _hub.Initialize(this);
            _hub.Hub.ApplicationRegistered += new RegisterApplicationEvent(EmitAppRegisteredEvent);
            _hub.Hub.ApplicationUnregistered += new UnregisterApplicationEvent(EmitAppUnregisteredEvent);
            _hub.Hub.TriggerRegistered += new RegisterTriggerEvent(EmitTrigRegisteredEvent);
            _hub.Hub.TriggerUnregistered += new UnregisterTriggerEvent(EmitTrigUnregisteredEvent);
            _hub.Hub.ObserverConnected += new ConnectObserverEvent(EmitObsConnectedEvent);
            _hub.Hub.ObserverDisconnected += new DisconnectObserverEvent(EmitObsDisconnectedEvent);

            EmitAppRegisteredEvent(AppID);
        }

        public void StopServer()
        {
            _hub.Finish(this);
            _hub = null;
        }

        public void KickApp(uint appId)
        {
            _hub.KickApp(appId);
        }

        public event RegisterApplicationEvent AppRegistered;
        private void EmitAppRegisteredEvent(uint appId)
        {
            if (AppRegistered != null)
                AppRegistered(appId);
        }

        public event UnregisterApplicationEvent AppUnregistered;
        private void EmitAppUnregisteredEvent(uint appId)
        {
            if (AppUnregistered != null)
                AppUnregistered(appId);
        }

        public event RegisterTriggerEvent TrigRegistered;
        private void EmitTrigRegisteredEvent(TriggerID id)
        {
            if (TrigRegistered != null)
                TrigRegistered(id);
        }

        public event UnregisterTriggerEvent TrigUnregistered;
        private void EmitTrigUnregisteredEvent(TriggerID id)
        {
            if (TrigUnregistered != null)
                TrigUnregistered(id);
        }

        public event ConnectObserverEvent ObsConnected;
        private void EmitObsConnectedEvent(TriggerID id, IObserver obs)
        {
            if (ObsConnected != null)
                ObsConnected(id, obs);
        }

        public event DisconnectObserverEvent ObsDisconnected;
        private void EmitObsDisconnectedEvent(TriggerID id, IObserver obs)
        {
            if (ObsDisconnected != null)
                ObsDisconnected(id, obs);
        }
    }
}
