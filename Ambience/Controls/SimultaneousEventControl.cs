using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;
using Genesis.Common.Tools;

namespace Genesis.Ambience.Controls
{
    public partial class SimultaneousEventControl : ASimultaneousEventControl
    {
        #region Constructors
        public SimultaneousEventControl()
        {
            InitializeComponent();
        }

        public SimultaneousEventControl(SimultaneousEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            InitializeComponent();

            _itemList.ItemsChanged += new Action(_itemList_ItemsChanged);

            this.Load += new EventHandler(SimultaneousEventControl_Load);
            this.SizeChanged += new EventHandler(SimultaneousEventControl_SizeChanged);
        }
        #endregion

        #region Event Handlers
        private void SimultaneousEventControl_Load(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        private void SimultaneousEventControl_SizeChanged(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        private void _itemList_ItemsChanged()
        {
            setDirty();
        }
        #endregion

        #region AEventControl
        protected override void saveToProvider()
        {
            EventProvider.Group.Clear();
            EventProvider.Group.AddRange(_itemList.Items);
        }

        protected override void onInit()
        {
            _itemList.Items.Clear();
            _itemList.Items.AddRange(EventProvider.Group);
        }
        #endregion

        #region Private Helpers
        private void adjustOrientation()
        {
            SuspendLayout();
            _itemList.ViewOrientation = (Width >= Height) ?
                ProviderTokenList.Orientation.Horizontal :
                ProviderTokenList.Orientation.Vertical;
            ResumeLayout(true);
        }
        #endregion
    }

    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<AEventControl, AEventControl.Concrete>))]
    public abstract class ASimultaneousEventControl : GenericEventControl<SimultaneousEventProvider>
    {
        public ASimultaneousEventControl() { }
        public ASimultaneousEventControl(SimultaneousEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
