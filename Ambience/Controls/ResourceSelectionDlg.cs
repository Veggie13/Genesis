using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Audio;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Controls
{
    public partial class ResourceSelectionDlg : Form
    {
        public ResourceSelectionDlg(SoundEvent.IResourceProvider mgr)
        {
            InitializeComponent();

            _view.Resources = mgr;
            _view.SelectionChanged += new LibraryView.ResourceEventHandler(_view_SelectionChanged);
        }

        void _view_SelectionChanged(SoundEvent.IResource res)
        {
            if (res == null)
                SelectedItem = "";
            else
                SelectedItem = res.FullName;
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                _txtSelectedItem.Text = _selectedItem;
                _view.SetSelectedResource(_selectedItem);
            }
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
