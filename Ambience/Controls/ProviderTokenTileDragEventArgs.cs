using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Genesis.Ambience.Controls
{
    public delegate void ProviderTokenTileDragEventHandler(ProviderTokenTile sender, ProviderTokenTileDragEventArgs e);

    public class ProviderTokenTileDragEventArgs : EventArgs
    {
        public ProviderTokenTile TokenTile
        {
            get;
            set;
        }

        public bool Cancel
        {
            get;
            set;
        }

        public bool Handled
        {
            get;
            set;
        }

        public Point Location
        {
            get;
            internal set;
        }
    }
}
