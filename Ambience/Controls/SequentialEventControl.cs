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
    public partial class SequentialEventControl : ASequentialEventControl
    {
        public SequentialEventControl()
        {
        }

        public SequentialEventControl(SequentialEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            _itemList.UseHoverScroll = true;

            this.Load += new EventHandler(SequentialEventControl_Load);
            this.SizeChanged += new EventHandler(SequentialEventControl_SizeChanged);
        }

        private void SequentialEventControl_Load(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        private void SequentialEventControl_SizeChanged(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        public override void ApplyChanges()
        {
            EventProvider.Sequence.Clear();
            EventProvider.Sequence.AddRange(_itemList.Items);
        }

        protected override void onInit()
        {
            InitializeComponent();

            _itemList.Items.Clear();
            _itemList.Items.AddRange(EventProvider.Sequence);
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

    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<AEventControl, AEventControl.Concrete>))]
    public abstract class ASequentialEventControl : GenericEventControl<SequentialEventSelector>
    {
        public ASequentialEventControl() { }
        public ASequentialEventControl(SequentialEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
