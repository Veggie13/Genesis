using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace Genesis.Common.Controls
{
    public static class Resources
    {
        public static Icon CreateGenesisIcon()
        {
            return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Genesis.Common.Controls.genesis.ico"));
        }
    }
}
