using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;

namespace ControlsTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public IEventProvider Provider
        {
            get { return eventProviderEditorControl1.Provider; }
            set
            {
                eventProviderEditorControl1.Provider = value;
            }
        }
    }
}
