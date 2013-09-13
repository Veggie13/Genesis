using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Genesis.Common.Controls;
using Genesis.Common.Tools;
using Genesis.Ambience.Controls;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience
{
    public partial class MainForm : AppMainForm
    {
        #region Class Members
        private ProjectInstance _project = null;
        #endregion

        public MainForm()
            : base("Ambience")
        {
            InitializeComponent();

            _soundBoard.TileClicked += new Ambience.Controls.ProviderTokenButton.TileClickedEvent(_soundBoard_TileClicked);

            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
        }

        #region Event Handlers
        #region MainForm
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_project != null)
                _project.Close();
        }
        #endregion

        #region Menu
        #region File
        private void _fileNewItem_Click(object sender, EventArgs e)
        {
            _project = new ProjectInstance();

            _soundBoard.TokenBoardProvider = _project;
            _spnRowCount.Value = _project.RowCount;
            _spnColCount.Value = _project.ColumnCount;

            _schedView.Schedule = _project.Schedule;

            _project.ItemsChanged += new Action(_project_ItemsChanged);
        }
        #endregion

        #region Library
        private void _libImportFromFileItem_Click(object sender, EventArgs e)
        {

        }
        
        private void _libImportFolderItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                _project.Resources.LoadLibrary(dlg.SelectedPath);
                //_project.GetSounds().ForEach(p => _providerList.Items.Add(p));
            }
        }

        private void _libBrowseItem_Click(object sender, EventArgs e)
        {
            var dlg = new LibraryBrowserDlg(_project);
            dlg.ShowDialog(this);
        }
        #endregion

        #region Events
        private void _evtNewDelayItem_Click(object sender, EventArgs e)
        {
            var evt = new DelayEventProvider("New Delay Event");
            doNewEvent(evt);
        }

        private void _evtNewPeriodicItem_Click(object sender, EventArgs e)
        {
            var evt = new PeriodicEventProvider("New Periodic Event");
            doNewEvent(evt);
        }

        private void _evtNewSequentialItem_Click(object sender, EventArgs e)
        {
            var evt = new SequentialEventSelector("New Sequential Selector");
            doNewEvent(evt);
        }

        private void _evtNewSimultaneousItem_Click(object sender, EventArgs e)
        {
            var evt = new SimultaneousEventProvider("New Simultaneous Event");
            doNewEvent(evt);
        }

        private void _evtNewRandomItem_Click(object sender, EventArgs e)
        {
            var evt = new RandomEventSelector("New Random Selector");
            doNewEvent(evt);
        }
        #endregion
        #endregion

        #region Project
        private void _project_ItemsChanged()
        {
            _providerList.Items.Clear();
            _providerList.Items.AddRange(_project.Events);
        }
        #endregion

        #region Sound Board
        private void _soundBoard_TileClicked(ProviderToken token)
        {
            _project.Schedule.AddProvider(token.Provider);
        }
        #endregion

        #region Schedule
        private void _btnPlay_Click(object sender, EventArgs e)
        {
            _schedView.Schedule = _project.Schedule;
            _project.Schedule.Start(false);
        }

        private void _btnStop_Click(object sender, EventArgs e)
        {
            _project.Schedule.Stop();
        }
        #endregion
        #endregion

        #region Private Helpers
        private void doNewEvent(IEventProvider evt)
        {
            DialogResult result = EventProviderEditorDlg.Show(this, evt, _project.Events, true);
            if (result != DialogResult.Cancel)
            {
                _project.Events.Add(evt);
            }
        }
        #endregion
    }
}
