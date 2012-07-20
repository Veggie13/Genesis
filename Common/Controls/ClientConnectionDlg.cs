using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Common.API.Comms;

namespace Genesis.Common.Controls
{
    public partial class ClientConnectionDlg : Form
    {
        private IGenesisApplication _app;
        private ClientHubProvider _clientHub;

        public ClientConnectionDlg(IGenesisApplication app, ClientHubProvider hub)
        {
            InitializeComponent();

            _app = app;
            _clientHub = hub;
        }

        private void _btnConnect_Click(object sender, EventArgs e)
        {
            _clientHub.SetChannel(_details);
            if (!_clientHub.Initialize(_app))
            {
                MessageBox.Show("Could not initialize connection to server.",
                    "Initialize Failed", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
