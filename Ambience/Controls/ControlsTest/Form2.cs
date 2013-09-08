using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Controls;
using Genesis.Ambience.Scheduler;

namespace ControlsTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public IEventColorProvider ColorProvider { get; set; }
        
        private AEventControl eventControl;
        public IEventProvider Provider
        {
            get { return (eventControl == null) ? null : eventControl.Provider; }
            set
            {
                if (eventControl != null)
                {
                    Controls.Remove(eventControl);
                    eventControl = null;
                }

                if (value != null)
                {
                    eventControl = EventProviderControlFactory.Create(value, ColorProvider);
                    eventControl.Dock = DockStyle.Fill;
                    Controls.Add(eventControl);
                }
            }
        }
    }
}
