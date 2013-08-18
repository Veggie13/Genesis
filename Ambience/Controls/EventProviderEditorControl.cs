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
    public partial class EventProviderEditorControl : UserControl
    {
        public EventProviderEditorControl()
        {
            InitializeComponent();
        }

        private IEventProvider _provider;
        public IEventProvider Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                if (_provider == value)
                    return;
                if (_provider != null)
                {
                    _contents.Controls.Clear();
                }

                _provider = value;

                if (_provider != null)
                {
                    AEventControl ctrl = EventProviderControlFactory.Create(_provider, ColorProvider);
                    ctrl.Margin = new System.Windows.Forms.Padding(0);
                    //ctrl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    //ctrl.Size = _contents.DisplayRectangle.Size;
                    ctrl.Dock = DockStyle.Fill;
                    ctrl.Location = new Point(0, 0);

                    _contents.Controls.Add(ctrl);
                }
            }
        }

        public IEventColorProvider ColorProvider
        {
            get;
            set;
        }
    }
}
