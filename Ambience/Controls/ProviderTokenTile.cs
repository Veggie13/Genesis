using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Genesis.Ambience.Controls
{
    public partial class ProviderTokenTile : UserControl
    {
        public ProviderTokenTile()
        {
            InitializeComponent();

            _panel.Paint += new PaintEventHandler(_panel_Paint);
            _panel.MouseDown += new MouseEventHandler(_panel_MouseDown);
            
            _panel.DragEnter += new DragEventHandler(empty_DragEnter);
            _panel.DragDrop += new DragEventHandler(empty_DragDrop);
            
            _label.DragEnter += new DragEventHandler(empty_DragEnter);
            _label.DragDrop += new DragEventHandler(empty_DragDrop);

            this.DoubleBuffered = true;
        }

        #region Events
        public new event MouseEventHandler MouseMove
        {
            add
            {
                _panel.MouseMove += value;
                _label.MouseMove += value;
                base.MouseMove += value;
            }
            remove
            {
                _panel.MouseMove -= value;
                _label.MouseMove -= value;
                base.MouseMove -= value;
            }
        }

        public new event EventHandler MouseEnter
        {
            add
            {
                _panel.MouseEnter += value;
                _label.MouseEnter += value;
                base.MouseEnter += value;
            }
            remove
            {
                _panel.MouseEnter -= value;
                _label.MouseEnter -= value;
                base.MouseEnter -= value;
            }
        }

        public new event EventHandler MouseLeave
        {
            add
            {
                _panel.MouseLeave += value;
                _label.MouseLeave += value;
                base.MouseLeave += value;
            }
            remove
            {
                _panel.MouseLeave -= value;
                _label.MouseLeave -= value;
                base.MouseLeave -= value;
            }
        }
        #endregion

        #region Properties
        #region Token
        private ProviderToken _token = null;
        [Browsable(false)]
        public ProviderToken Token
        {
            get { return _token; }
            set
            {
                _token = value;
                _label.Visible = (_token == null);
                _panel.Invalidate();
            }
        }
        #endregion

        #region TokenFont
        private static Font DefaultTokenFont = new Font("Arial", 8f);
        private Font _font = new Font(DefaultTokenFont, DefaultTokenFont.Style);
        public Font TokenFont
        {
            get { return new Font(_font, _font.Style); }
            set
            {
                _font = (value == null) ?
                    new Font(DefaultTokenFont, DefaultTokenFont.Style) :
                    new Font(value, value.Style);
                this.Invalidate();
            }
        }
        private bool ShouldSerializeTokenFont()
        {
            return !_font.Equals(DefaultTokenFont);
        }
        #endregion
        #endregion

        #region Event Handlers
        private void empty_DragDrop(object sender, DragEventArgs e)
        {
            Token = (ProviderToken)e.Data.GetData(typeof(ProviderToken));
        }

        private void empty_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ProviderToken)))
                e.Effect = DragDropEffects.Copy;
        }

        private void _panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (_token != null)
                this.DoDragDrop(_token, DragDropEffects.Copy);
        }

        private void _panel_Paint(object sender, PaintEventArgs e)
        {
            if (_token != null)
            {
                Color main = _token.Color;
                Color lite = ControlPaint.LightLight(main);
                Color dark = ControlPaint.Dark(main, 0.01f);
                Brush bg = new LinearGradientBrush(_panel.DisplayRectangle, lite, dark, LinearGradientMode.Vertical);
                Pen border = new Pen(main);
                e.Graphics.FillRectangle(bg, _panel.DisplayRectangle);
                e.Graphics.DrawRectangle(border, _panel.DisplayRectangle);
                e.Graphics.DrawString(_token.Name, _font, Brushes.Black,
                    new RectangleF(2f, 2f, _panel.DisplayRectangle.Width - 4,
                        _panel.DisplayRectangle.Height - 4));
            }
        }
        #endregion
    }
}
