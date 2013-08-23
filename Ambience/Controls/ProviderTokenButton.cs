using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Genesis.Ambience.Controls
{
    public class ProviderTokenButton : Panel
    {
        #region Class Members
        private ProviderTokenTile _tile = new ProviderTokenTile();
        private bool _pushed = false;
        #endregion

        public ProviderTokenButton()
        {
            this.DoubleBuffered = true;
            this.Padding = new System.Windows.Forms.Padding(5);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.MouseDown += new MouseEventHandler(mouseDown);
            this.MouseUp += new MouseEventHandler(mouseUp);
            this.MouseLeave += new EventHandler(mouseLeave);

            _tile.Dock = DockStyle.Fill;
            _tile.Margin = new System.Windows.Forms.Padding(5);
            _tile.AllowDrop = true;
            _tile.AllowDrag = false;
            Controls.Add(_tile);

            _tile.TokenChanged += new ProviderTokenTile.TokenEvent(_tile_TokenChanged);
            _tile.Click += new EventHandler(_tile_Click);
            _tile.MouseDown += new MouseEventHandler(mouseDown);
            _tile.MouseUp += new MouseEventHandler(mouseUp);
            _tile.MouseLeave += new EventHandler(mouseLeave);
        }

        #region Events
        public delegate void TileClickedEvent(ProviderToken token);
        public event TileClickedEvent TileClicked = (t) => { };
        #endregion

        #region Event Handlers
        void mouseLeave(object sender, EventArgs e)
        {
            if (!RectangleToScreen(ClientRectangle).Contains(MousePosition) && _pushed)
            {
                _pushed = false;
                Invalidate();
            }
        }

        void mouseUp(object sender, MouseEventArgs e)
        {
            _pushed = false;
            Invalidate();
        }

        void mouseDown(object sender, MouseEventArgs e)
        {
            _pushed = true;
            Invalidate();
        }

        void _tile_Click(object sender, EventArgs e)
        {
            if (_tile.Token != null)
            {
                TileClicked(_tile.Token);
            }
        }

        void _tile_TokenChanged(ProviderToken token)
        {
            Invalidate();
        }
        #endregion

        #region Protected Overrides
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_tile.Token == null)
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
            }
            else
            {
                ControlPaint.DrawButton(e.Graphics, ClientRectangle, _pushed ? ButtonState.Pushed : ButtonState.Normal);
            }
        }
        #endregion
    }
}
