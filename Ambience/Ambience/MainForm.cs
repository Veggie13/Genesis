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
using Genesis.Ambience.Controls;

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

        void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_project != null)
                _project.Close();
        }

        private void _soundBoard_TileClicked(ProviderToken token)
        {
            _project.Schedule.AddProvider(token.Provider);
        }

        private void _libImportFromFileItem_Click(object sender, EventArgs e)
        {

        }

        private void _fileNameItem_Click(object sender, EventArgs e)
        {
            _project = new ProjectInstance();

            _soundBoard.TokenBoardProvider = _project;
            _spnRowCount.Value = _project.RowCount;
            _spnColCount.Value = _project.ColumnCount;

            _schedView.Schedule = _project.Schedule;
        }

        private void _libImportFolderItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                _project.Resources.LoadLibrary(dlg.SelectedPath);
                _project.GetSounds().ForEach(p => _providerList.Items.Add(p));
            }
        }

        private void _btnPlay_Click(object sender, EventArgs e)
        {
            _schedView.Schedule = _project.Schedule;
            _project.Schedule.Start(false);
        }

        private void _btnStop_Click(object sender, EventArgs e)
        {
            _project.Schedule.Stop();
        }
    }
}
