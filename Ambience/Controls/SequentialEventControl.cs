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
        #region Constructors
        public SequentialEventControl()
        {
            InitializeComponent();
        }

        public SequentialEventControl(SequentialEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            InitializeComponent();

            _itemList.UseHoverScroll = true;

            this.Load += new EventHandler(SequentialEventControl_Load);
            this.SizeChanged += new EventHandler(SequentialEventControl_SizeChanged);
        }
        #endregion

        #region Event Handlers
        private void SequentialEventControl_Load(object sender, EventArgs e)
        {
            adjustOrientation();
        }

        private void SequentialEventControl_SizeChanged(object sender, EventArgs e)
        {
            adjustOrientation();
        }
        #endregion

        #region AEventControl
        protected override void saveToProvider()
        {
            EventProvider.Sequence.Clear();
            EventProvider.Sequence.AddRange(_itemList.Items);
        }

        protected override void onInit()
        {
            _itemList.Items.Clear();
            _itemList.Items.AddRange(EventProvider.Sequence);
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
    public abstract class ASequentialEventControl : GenericEventControl<SequentialEventSelector>
    {
        public ASequentialEventControl() { }
        public ASequentialEventControl(SequentialEventSelector prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
