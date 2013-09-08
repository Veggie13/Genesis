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
        public class BrowseHandlerEventArgs
        {
            public bool Cancel { get; set; }
            public string ResourceName { get; set; }
        }

        public SoundEventControl()
            : base()
        {
        }

        public SoundEventControl(SoundEvent.Provider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }

        public delegate void BrowseHandler(BrowseHandlerEventArgs e);
        public event BrowseHandler Browse = (e) => { };

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

                if (!Initializing)
                    emitModified();
            }
        }

        public override void ApplyChanges()
        {
            EventProvider.ResourceName = ResourceName;
        }

        protected override void onInit()
        {
            InitializeComponent();

            ResourceName = EventProvider.ResourceName;
        }

        private void _btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new ResourceSelectionDlg(EventProvider.ResourceProvider);
            dlg.SelectedItem = ResourceName;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                ResourceName = dlg.SelectedItem;
            }
        }
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
