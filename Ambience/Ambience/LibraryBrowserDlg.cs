using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;
using Genesis.Ambience.Controls;

namespace Genesis.Ambience
{
    partial class LibraryBrowserDlg : Form
    {
        private ProjectInstance _inst;

        public LibraryBrowserDlg(ProjectInstance inst)
        {
            InitializeComponent();

            _inst = inst;
            
            _libView.Resources = inst.Resources;
            _libView.SelectionChanged += new LibraryView.ResourceEventHandler(_libView_SelectionChanged);
        }

        private void _libView_SelectionChanged(SoundEvent.IResource res)
        {
            _btnNewEvent.Enabled = (res != null);
        }

        private void _btnNewEvent_Click(object sender, EventArgs e)
        {
            var res = _libView.SelectedResource;
            var evt = new SoundEvent.Provider(res.FullName, _inst.Resources, res.FullName);
            var dlg = new EventProviderEditorDlg();
            dlg.Cancellable = true;
            dlg.Provider = evt;
            DialogResult result = dlg.ShowDialog(this);
            if (result != DialogResult.Cancel)
            {
                _inst.Events.Add(evt);
            }
        }
    }
}
