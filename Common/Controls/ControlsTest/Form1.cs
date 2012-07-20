using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Common.Controls;

namespace ControlsTest
{
    public partial class Form1 : AppMainForm
    {
        public Form1()
            : base("Controls Library Test")
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = connection1.URL;
        }
    }
}
