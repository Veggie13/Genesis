using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Controls;

namespace ControlsTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            tokenBoard1.ButtonRightClicked += new ProviderTokenButton.RightClickEvent(tokenBoard1_ButtonRightClicked);
        }

        void tokenBoard1_ButtonRightClicked(ProviderTokenButton button)
        {
            ProviderToken token = button.Token;
            MessageBox.Show((token == null) ? "<empty>" : token.Name);
        }
    }
}
