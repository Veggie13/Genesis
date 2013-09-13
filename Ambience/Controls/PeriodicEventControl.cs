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
        #region Constructors
        public PeriodicEventControl()
            : base()
        {
            InitializeComponent();
        }

        public PeriodicEventControl(PeriodicEventProvider prov, IEventColorProvider colorer)
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

        private void _spnPeriod_ValueChanged(object sender, EventArgs e)
        {
            setDirty();
        }

        private void _spnVariance_ValueChanged(object sender, EventArgs e)
        {
            setDirty();
        }
        #endregion

        #region AEventControl
        protected override void saveToProvider()
        {
            EventProvider.Period = (uint)_spnPeriod.Value;
            EventProvider.Variance = (uint)_spnVariance.Value;
            if (_element.Token != null)
                EventProvider.Subordinate = _element.Token.Provider;
        }

        protected override void onInit()
        {
            _spnPeriod.Value = (decimal)EventProvider.Period;
            _spnVariance.Value = (decimal)EventProvider.Variance;
            if (EventProvider.Subordinate != null)
                _element.Token = new ProviderToken(EventProvider.Subordinate, ColorProvider);
        }
        #endregion
    }

    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<AEventControl, AEventControl.Concrete>))]
    public abstract class APeriodicEventControl : GenericEventControl<PeriodicEventProvider>
    {
        public APeriodicEventControl() { }
        public APeriodicEventControl(PeriodicEventProvider prov, IEventColorProvider colorer)
            : base(prov, colorer)
        {
        }
    }
}
