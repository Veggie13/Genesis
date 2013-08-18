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
    public partial class RandomEventControl : ARandomEventControl
    {
        public RandomEventControl()
        {
        }
        
        public RandomEventControl(RandomEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            _itemList.UseHoverScroll = true;

            this.Load += new EventHandler(RandomEventControl_Load);
            this.SizeChanged += new EventHandler(RandomEventControl_SizeChanged);
        }

        private void RandomEventControl_Load(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        private void RandomEventControl_SizeChanged(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        public override void ApplyChanges()
        {
            EventProvider.Selection.Clear();
            EventProvider.Selection.AddRange(_itemList.Items);
        }

        protected override void onInit()
        {
            InitializeComponent();

            _itemList.Items.Clear();
            _itemList.Items.AddRange(EventProvider.Selection);
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
    public abstract class ARandomEventControl : GenericEventControl<RandomEventSelector>
    {
        public ARandomEventControl() { }
        public ARandomEventControl(RandomEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
