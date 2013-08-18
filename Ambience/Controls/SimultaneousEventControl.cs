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
        public SimultaneousEventControl()
        {
        }

        public SimultaneousEventControl(SimultaneousEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            _itemList.UseHoverScroll = true;

            this.Load += new EventHandler(SimultaneousEventControl_Load);
            this.SizeChanged += new EventHandler(SimultaneousEventControl_SizeChanged);
        }

        private void SimultaneousEventControl_Load(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        private void SimultaneousEventControl_SizeChanged(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        public override void ApplyChanges()
        {
            EventProvider.Group.Clear();
            EventProvider.Group.AddRange(_itemList.Items);
        }

        protected override void onInit()
        {
            InitializeComponent();

            _itemList.Items.Clear();
            _itemList.Items.AddRange(EventProvider.Group);
        }

        private void adjustOrientation()
        {
            SuspendLayout();
            _itemList.ViewOrientation = (Width >= Height) ?
                ProviderTokenList.Orientation.Horizontal :
                ProviderTokenList.Orientation.Vertical;
            ResumeLayout(true);
        }
    }

    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<UserControl>))]
    [Helper.ConcreteClass(typeof(AEventControl.Concrete))]
    public abstract class ASimultaneousEventControl : GenericEventControl<SimultaneousEventProvider>
    {
        public ASimultaneousEventControl() { }
        public ASimultaneousEventControl(SimultaneousEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
