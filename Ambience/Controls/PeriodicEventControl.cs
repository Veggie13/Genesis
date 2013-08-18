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
    public partial class PeriodicEventControl : APeriodicEventControl
    {
        public PeriodicEventControl()
            : base()
        {
        }

        public PeriodicEventControl(PeriodicEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
            _element.TokenChanged += new ProviderTokenTile.TokenEvent(_element_TokenChanged);
        }

        private void _element_TokenChanged(ProviderToken token)
        {
            if (!Initializing)
                emitModified();
        }

        public override void ApplyChanges()
        {
            EventProvider.Period = (uint)_spnPeriod.Value;
            EventProvider.Variance = (uint)_spnVariance.Value;
            EventProvider.Subordinate = _element.Token.Provider;
        }

        protected override void onInit()
        {
            InitializeComponent();

            _spnPeriod.Value = (decimal)EventProvider.Period;
            _spnVariance.Value = (decimal)EventProvider.Variance;
            _element.Token = new ProviderToken(EventProvider.Subordinate, ColorProvider);
        }

        private void _spnPeriod_ValueChanged(object sender, EventArgs e)
        {
            if (!Initializing)
                emitModified();
        }

        private void _spnVariance_ValueChanged(object sender, EventArgs e)
        {
            if (!Initializing)
                emitModified();
        }
    }

    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<UserControl>))]
    [Helper.ConcreteClass(typeof(AEventControl.Concrete))]
    public abstract class APeriodicEventControl : GenericEventControl<PeriodicEventProvider>
    {
        public APeriodicEventControl() { }
        public APeriodicEventControl(PeriodicEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
