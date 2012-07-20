using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Common.Controls;
using Genesis.Common.API.Comms;

namespace Genesis.Applications.Core
{
    public partial class MainForm : AppMainForm
    {
        private App _app;
        private object _treeLocker = new object();

        public MainForm(App app)
            : base("Core Control Center")
        {
            InitializeComponent();
            _app = app;
            _app.AppRegistered += new RegisterApplicationEvent(_app_AppRegistered);
            _app.AppUnregistered += new UnregisterApplicationEvent(_app_AppUnregistered);
            _app.TrigRegistered += new RegisterTriggerEvent(_app_TrigRegistered);
            _app.TrigUnregistered += new UnregisterTriggerEvent(_app_TrigUnregistered);
            _app.ObsConnected += new ConnectObserverEvent(_app_ObsConnected);
            _app.ObsDisconnected += new DisconnectObserverEvent(_app_ObsDisconnected);
        }

        void _app_ObsDisconnected(TriggerID id, IObserver obs)
        {
            if (InvokeRequired)
                Invoke(new DisconnectObserverEvent(_app_ObsDisconnected), id, obs);
            else
            {
                lock (_treeLocker)
                {
                    TreeNode[] app = _treeTriggers.Nodes.Find(id.ApplicationID.ToString(), false);
                    TreeNode[] trig = app[0].Nodes.Find(id.Name, false);
                    trig[0].Nodes.RemoveByKey(obs.ApplicationID.ToString());
                }
            }
        }

        void _app_TrigUnregistered(TriggerID id)
        {
            if (InvokeRequired)
                Invoke(new UnregisterTriggerEvent(_app_TrigUnregistered), id);
            else
            {
                lock (_treeLocker)
                {
                    TreeNode[] app = _treeTriggers.Nodes.Find(id.ApplicationID.ToString(), false);
                    app[0].Nodes.RemoveByKey(id.Name);
                }
            }
        }

        void _app_AppUnregistered(uint appId)
        {
            if (InvokeRequired)
                Invoke(new UnregisterApplicationEvent(_app_AppUnregistered), appId);
            else
            {
                lock (_treeLocker)
                {
                    _treeTriggers.Nodes.RemoveByKey(appId.ToString());
                }
            }
        }

        void _app_ObsConnected(TriggerID id, IObserver obs)
        {
            if (InvokeRequired)
                Invoke(new ConnectObserverEvent(_app_ObsConnected), id, obs);
            else
            {
                lock (_treeLocker)
                {
                    TreeNode[] app = _treeTriggers.Nodes.Find(id.ApplicationID.ToString(), false);
                    TreeNode[] trig = app[0].Nodes.Find(id.Name, false);
                    trig[0].Nodes.Add(obs.ApplicationID.ToString(),
                        string.Format("{0} - {1}", obs.ApplicationID,
                            _app.Hub.GetRegisteredAppName(obs.ApplicationID)));
                }
            }
        }

        void _app_TrigRegistered(Common.API.Comms.TriggerID id)
        {
            if (InvokeRequired)
                Invoke(new RegisterTriggerEvent(_app_TrigRegistered), id);
            else
            {
                lock (_treeLocker)
                {
                    TreeNode[] nodes = _treeTriggers.Nodes.Find(id.ApplicationID.ToString(), false);
                    nodes[0].Nodes.Add(id.Name, id.Name);
                }
            }
        }

        void _app_AppRegistered(uint appId)
        {
            if (InvokeRequired)
                Invoke(new RegisterApplicationEvent(_app_AppRegistered), appId);
            else
            {
                lock (_treeLocker)
                {
                    _treeTriggers.Nodes.Add(appId.ToString(),
                        string.Format("{0} - {1}", appId,
                            _app.Hub.GetRegisteredAppName(appId)));
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Splash.DoSplash(Properties.Resources.Splash, Splash.BasicDelay(1000));

            _chkTCP.Checked = _app.UseTCP;
            _chkPipes.Checked = _app.UsePipes;
            _spnTCPPort.Value = _app.TCPPort;
            _spnPipePort.Value = _app.NamedPipesPort;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_app.IsRunning)
                StopServer();
            Close();
        }

        private void _chkTCP_CheckedChanged(object sender, EventArgs e)
        {
            _app.UseTCP = _chkTCP.Checked;
            _spnTCPPort.Enabled = !_app.IsRunning || !_chkTCP.Checked;
            if (!_app.IsRunning)
            {
                _btnState.Enabled = _chkTCP.Checked || _chkPipes.Checked;
            }
        }

        private void _chkPipes_CheckedChanged(object sender, EventArgs e)
        {
            _app.UsePipes = _chkPipes.Checked;
            _spnPipePort.Enabled = !_app.IsRunning || !_chkPipes.Checked;
            if (!_app.IsRunning)
            {
                _btnState.Enabled = _chkTCP.Checked || _chkPipes.Checked;
            }
        }

        private void _spnTCPPort_ValueChanged(object sender, EventArgs e)
        {
            _app.TCPPort = (int)_spnTCPPort.Value;
        }

        private void _spnPipePort_ValueChanged(object sender, EventArgs e)
        {
            _app.NamedPipesPort = (int)_spnPipePort.Value;
        }

        private void _btnState_Click(object sender, EventArgs e)
        {
            if (_app.IsRunning)
            {
                StopServer();
            }
            else
            {
                StartServer();
            }
        }

        private void StartServer()
        {
            _btnState.Text = "Stop";
            _spnTCPPort.Enabled = !_chkTCP.Checked;
            _spnPipePort.Enabled = !_chkPipes.Checked;
            _treeTriggers.Enabled = true;
            _btnKick.Enabled = false;

            _app.StartServer();
        }

        private void StopServer()
        {
            _btnState.Text = "Start";
            _spnTCPPort.Enabled = true;
            _spnPipePort.Enabled = true;
            _treeTriggers.Enabled = false;
            _btnKick.Enabled = false;

            _treeTriggers.Nodes.Clear();

            _app.StopServer();
        }

        private void _btnKick_Click(object sender, EventArgs e)
        {
            uint appId = 0;
            string key = _treeTriggers.SelectedNode.Name;
            if (uint.TryParse(key, out appId))
            {
                _app.KickApp(appId);
            }
        }

        private void _treeTriggers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _btnKick.Enabled = (_treeTriggers.SelectedNode != null &&
                _treeTriggers.SelectedNode.Level == 0 &&
                !_treeTriggers.SelectedNode.Name.Equals(_app.AppID.ToString()));
        }
    }
}
