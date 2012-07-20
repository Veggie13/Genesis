using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Common.API.Comms;

namespace Genesis.Common.Controls
{
    public partial class ClientConnectionDetailControl : UserControl, IClientChannelDetailProvider
    {
        public ClientConnectionDetailControl()
        {
            InitializeComponent();
        }

        public ChannelMode Mode
        {
            get
            {
                return _rdoPipe.Checked ? ChannelMode.NamedPipe : ChannelMode.TCPIP;
            }
        }

        public string Server
        {
            get { return _txtServer.Text; }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
        }

        public string URL
        {
            get
            {
                return
                    (Mode == ChannelMode.NamedPipe ? "ipc://" : "tcp://") +
                    Server + ":" +
                    Port.ToString();
            }
        }

        private void _rdoPipe_CheckedChanged(object sender, EventArgs e)
        {
            _txtServer.Enabled = _rdoTcp.Checked;
            if (_rdoPipe.Checked)
                _txtServer.Text = "localhost";
        }

        private void Connection_Load(object sender, EventArgs e)
        {
            _port = int.Parse(_txtPort.Text);
        }

        private void _txtPort_TextChanged(object sender, EventArgs e)
        {
            int val = _port;
            if (!int.TryParse(_txtPort.Text, out val))
                _txtPort.Text = _port.ToString();
            else
                _port = val;
        }
    }
}
