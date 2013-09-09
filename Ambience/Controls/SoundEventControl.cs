using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Controls
{
    public partial class SoundEventControl : ASoundEventControl
    {
        #region Constructors
        public SoundEventControl()
            : base()
        {
            InitializeComponent();
        }

        public SoundEventControl(SoundEvent.Provider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        public class BrowseHandlerEventArgs
        {
            public bool Cancel { get; set; }
            public string ResourceName { get; set; }
        }
        public delegate void BrowseHandler(BrowseHandlerEventArgs e);
        public event BrowseHandler Browse = (e) => { };
        #endregion

        #region Properties
        private string _resName;
        private string ResourceName
        {
            get { return _resName; }
            set
            {
                _resName = value;
                var tokens = _resName.Split(new string[] { "::" }, StringSplitOptions.None);
                _txtResName.Text = tokens[1];
                _library.Text = tokens[0];

                setDirty();

                MessageBox.Show(_txtResName.Text);
            }
        }
        #endregion

        #region Event Handlers
        private void _btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new ResourceSelectionDlg(EventProvider.ResourceProvider);
            dlg.SelectedItem = ResourceName;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                ResourceName = dlg.SelectedItem;
            }
        }
        #endregion

        #region AEventControl
        protected override void saveToProvider()
        {
            EventProvider.ResourceName = ResourceName;
        }

        protected override void onInit()
        {
            ResourceName = EventProvider.ResourceName;
        }
        #endregion
    }

    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<AEventControl, AEventControl.Concrete>))]
    public abstract class ASoundEventControl : GenericEventControl<SoundEvent.Provider>
    {
        public ASoundEventControl() { }
        public ASoundEventControl(SoundEvent.Provider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
