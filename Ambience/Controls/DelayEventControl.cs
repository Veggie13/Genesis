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
    public partial class DelayEventControl : ADelayEventControl
    {
        #region Constructors
        public DelayEventControl()
        {
            InitializeComponent();
        }

        public DelayEventControl(DelayEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            InitializeComponent();

            _element.TokenChanged += new ProviderTokenTile.TokenEvent(_element_TokenChanged);
        }
        #endregion

        #region Event Handlers
        private void _element_TokenChanged(ProviderToken token)
        {
            setDirty();
        }

        private void _spnDelay_ValueChanged(object sender, EventArgs e)
        {
            setDirty();
        }
        #endregion

        #region AEventControl
        protected override void saveToProvider()
        {
            EventProvider.Delay = (uint)_spnDelay.Value;
            if (_element.Token != null)
                EventProvider.Subordinate = _element.Token.Provider;
        }

        protected override void onInit()
        {
            _spnDelay.Value = (decimal)EventProvider.Delay;
            if (EventProvider.Subordinate != null)
                _element.Token = new ProviderToken(EventProvider.Subordinate, ColorProvider);
        }
        #endregion
    }

    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<AEventControl, AEventControl.Concrete>))]
    public abstract class ADelayEventControl : GenericEventControl<DelayEventProvider>
    {
        public ADelayEventControl() { }
        public ADelayEventControl(DelayEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
