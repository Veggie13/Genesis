using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genesis.Common.Controls
{
    public partial class AppMainForm : Form
    {
        private const string TitleFormat = " Genesis - {0}{1}";
        private const string SubtitleFormat = " - {0}{1}";

        private string _title;
        private string _subtitle = "";
        private bool _modified = false;

        public AppMainForm()
        {
            InitializeComponent();
        }
        
        public AppMainForm(string title)
        {
            InitializeComponent();
            _title = title;
        }

        public string Subtitle
        {
            get { return _subtitle; }
            set
            {
                _subtitle = value;
                UpdateTitle();
            }
        }

        public bool IsModified
        {
            get { return _modified; }
            set
            {
                _modified = value;
                UpdateTitle();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Icon = Resources.CreateGenesisIcon();
            if (_title == null)
                _title = Text;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Text = string.Format(TitleFormat, _title,
                string.IsNullOrEmpty(_subtitle) ? "" :
                string.Format(SubtitleFormat, _subtitle,
                _modified ? "*" : ""));
        }
    }
}
