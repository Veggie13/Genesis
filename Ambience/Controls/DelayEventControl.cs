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

        private void _element_TokenChanged(ProviderToken token)
        {
            if (!Initializing)
                emitModified();
        }

        public override void ApplyChanges()
        {
            EventProvider.Delay = (uint)_spnDelay.Value;
            EventProvider.Subordinate = _element.Token.Provider;
        }

        protected override void onInit()
        {
            _spnDelay.Value = (decimal)EventProvider.Delay;
            _element.Token = new ProviderToken(EventProvider.Subordinate, ColorProvider);
        }

        private void _spnDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!Initializing)
                emitModified();
        }
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
