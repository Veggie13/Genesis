using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Controls
{
    public partial class EventProviderEditorDlg : Form
    {
        #region Class Members
        private AEventControl _editor;
        #endregion

        public EventProviderEditorDlg()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(EventProviderEditorDlg_FormClosing);
        }

        #region Properties
        public IEventColorProvider ColorProvider
        {
            get;
            set;
        }
        
        public IEventProvider Provider
        {
            get { return (_editor == null) ? null : _editor.Provider; }
            set
            {
                if (_editor != null)
                {
                    _editor.ModifiedChanged -= _editor_ModifiedChanged;
                    _editorPanel.Controls.Remove(_editor);
                    _editor = null;
                }

                if (value != null)
                {
                    _editor = EventProviderControlFactory.Create(value, ColorProvider);
                    _editor.ModifiedChanged += new Action(_editor_ModifiedChanged);
                    _editor.Dock = DockStyle.Fill;
                    _editorPanel.Controls.Add(_editor);
                }
            }
        }

        public bool Cancellable
        {
            get { return _btnCancel.Visible; }
            set
            {
                _btnCancel.Visible = value;
                _btnCancel.Enabled = value;
                _btnClose.Text = value ? "OK" : "Close";
            }
        }
        #endregion

        #region Event Handlers
        private void EventProviderEditorDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_editor != null && _editor.Modified && this.DialogResult != DialogResult.Cancel)
            {
                DialogResult result = MessageBox.Show(this, "Save changes to event?", "Modified Event", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
                else if (result == DialogResult.Yes)
                    _editor.ApplyChanges();
            }
        }

        private void _editor_ModifiedChanged()
        {
            this.Text = _editor.Modified ? "*" : "";
        }
        #endregion
    }
}
