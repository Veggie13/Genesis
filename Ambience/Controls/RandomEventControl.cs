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
        #region Constructors
        public RandomEventControl()
        {
            InitializeComponent();
        }
        
        public RandomEventControl(RandomEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            InitializeComponent();

            _itemList.UseHoverScroll = true;

            this.Load += new EventHandler(RandomEventControl_Load);
            this.SizeChanged += new EventHandler(RandomEventControl_SizeChanged);
        }
        #endregion

        #region Event Handlers
        private void RandomEventControl_Load(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        private void RandomEventControl_SizeChanged(object sender, EventArgs e)
        {
            adjustOrientation();
        }
        #endregion

        #region AEventControl
        protected override void saveToProvider()
        {
            EventProvider.Selection.Clear();
            EventProvider.Selection.AddRange(_itemList.Items);
        }

        protected override void onInit()
        {
            _itemList.Items.Clear();
            _itemList.Items.AddRange(EventProvider.Selection);
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
    public abstract class ARandomEventControl : GenericEventControl<RandomEventSelector>
    {
        public ARandomEventControl() { }
        public ARandomEventControl(RandomEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
