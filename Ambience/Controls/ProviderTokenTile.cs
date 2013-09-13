using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Genesis.Common.Tools;
using System.Diagnostics;

namespace Genesis.Ambience.Controls
{
    public partial class ProviderTokenTile : UserControl
    {
        #region Helper Classes
        private class MainPanel : TableLayoutPanel
        {
            private bool _inside = false;
            private DragDropEffects _lastEffect = DragDropEffects.None;

            protected override void OnDragEnter(DragEventArgs drgevent)
            {
                if (!_inside)
                {
                    _inside = true;
                    base.OnDragEnter(drgevent);
                    _lastEffect = drgevent.Effect;
                }
                else
                {
                    drgevent.Effect = _lastEffect;
                }
            }

            protected override void OnDragLeave(EventArgs e)
            {
                if (!_inside || this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                    return;

                _inside = false;
                base.OnDragLeave(e);
            }

            public void setup(Label label)
            {
                label.DragEnter += new DragEventHandler(label_DragEnter);
                label.DragDrop += new DragEventHandler(label_DragDrop);
                label.DragOver += new DragEventHandler(label_DragOver);
                label.DragLeave += new EventHandler(label_DragLeave);
                label.GiveFeedback += new GiveFeedbackEventHandler(label_GiveFeedback);
                label.Click += new EventHandler(label_Click);
                label.MouseDown += new MouseEventHandler(label_MouseDown);
                label.MouseUp += new MouseEventHandler(label_MouseUp);
                label.MouseMove += new MouseEventHandler(label_MouseMove);
            }

            void label_MouseMove(object sender, MouseEventArgs e)
            {
                OnMouseMove(e);
            }

            void label_MouseUp(object sender, MouseEventArgs e)
            {
                OnMouseUp(e);
            }

            void label_MouseDown(object sender, MouseEventArgs e)
            {
                OnMouseDown(e);
            }

            void label_Click(object sender, EventArgs e)
            {
                OnClick(e);
            }

            void label_GiveFeedback(object sender, GiveFeedbackEventArgs e)
            {
                OnGiveFeedback(e);
            }

            void label_DragLeave(object sender, EventArgs e)
            {
                OnDragLeave(e);
            }

            void label_DragOver(object sender, DragEventArgs e)
            {
                OnDragOver(e);
            }

            void label_DragDrop(object sender, DragEventArgs e)
            {
                OnDragDrop(e);
            }

            void label_DragEnter(object sender, DragEventArgs e)
            {
                OnDragEnter(e);
            }
        }
        #endregion

        #region Class Members
        private Point _lastMouseDown;
        private bool _isMouseDown = false;
        #endregion

        public ProviderTokenTile()
        {
            InitializeComponent();

            _label.Text = DefaultInnerText;

            _panel.Paint += new PaintEventHandler(_panel_Paint);
            _panel.MouseDown += new MouseEventHandler(_panel_MouseDown);
            _panel.MouseMove += new MouseEventHandler(_panel_MouseMove);
            _panel.MouseUp += new MouseEventHandler(_panel_MouseUp);
            _panel.DragEnter += new DragEventHandler(empty_DragEnter);
            _panel.DragDrop += new DragEventHandler(empty_DragDrop);
            _panel.DragOver += new DragEventHandler(empty_DragOver);
            _panel.DragLeave += new EventHandler(empty_DragLeave);
            _panel.setup(_label);

            this.GiveFeedback += new GiveFeedbackEventHandler(dragGiveFeedback);
            _panel.GiveFeedback += new GiveFeedbackEventHandler(dragGiveFeedback);

            this.SizeChanged += new EventHandler(ProviderTokenTile_SizeChanged);

            foreach (Control c in Controls)
            {
                c.Click += new EventHandler(childClick);
            }

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

        public delegate void TokenEvent(ProviderToken token);
        public event TokenEvent TokenChanged;
        private void emitTokenChanged()
        {
            if (TokenChanged != null)
                TokenChanged(_token);
        }

        public event ProviderTokenTileDragEventHandler AboutToDrag = (o, e) => { };
        public event ProviderTokenTileDragEventHandler AboutToDrop = (o, e) => { };
        #endregion

        #region Properties
        #region AllowDrag
        private const bool DefaultAllowDrag = true;
        private bool _allowDrag = DefaultAllowDrag;
        [DefaultValue(DefaultAllowDrag)]
        public bool AllowDrag
        {
            get { return _allowDrag; }
            set { _allowDrag = value; }
        }
        #endregion

        #region AllowDrop
        private const bool DefaultAllowDrop = true;
        private bool _allowDrop = DefaultAllowDrop;
        [DefaultValue(DefaultAllowDrop)]
        public new bool AllowDrop
        {
            get { return _allowDrop; }
            set { _allowDrop = value; }
        }
        #endregion

        #region Token
        private ProviderToken _token = null;
        [Browsable(false)]
        public ProviderToken Token
        {
            get { return _token; }
            set
            {
                if (_token != value)
                {
                    _token = value;
                    if (_token != null)
                        this.Name = _token.Name;
                    _label.Visible = (_token == null);
                    _panel.Invalidate();
                    emitTokenChanged();
                }
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

        #region InnerText
        private const string DefaultInnerText = "Drop Here";
        public string InnerText
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }
        private bool ShouldSerializeInnerText()
        {
            return !DefaultInnerText.Equals(InnerText);
        }
        #endregion
        #endregion

        #region Event Handlers
        private void childClick(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void dragGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
        }

        private void ProviderTokenTile_SizeChanged(object sender, EventArgs e)
        {
            _label.MaximumSize = Size;
        }

        private void empty_DragDrop(object sender, DragEventArgs e)
        {
            if (!AllowDrop || !e.IsDataPresent<ProviderTokenTileDragEventArgs>())
                return;

            var args = e.GetData<ProviderTokenTileDragEventArgs>();
            args.Location = new Point(e.X, e.Y);
            if (args != null)
            {
                AboutToDrop(this, args);
                if (!args.Cancel && !args.Handled)
                    Token = args.TokenTile.Token;
            }
        }

        private void empty_DragEnter(object sender, DragEventArgs e)
        {
            if (!AllowDrop || !e.IsDataPresent<ProviderTokenTileDragEventArgs>())
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;
            var args = e.GetData<ProviderTokenTileDragEventArgs>();

            Size sz = args.TokenTile.DisplayRectangle.Size;
            Bitmap bm = new Bitmap(sz.Width, sz.Height);
            using (var g = Graphics.FromImage(bm))
                args.TokenTile.paint(g);

            Cursor.Current = CreateCursor(bm, 0, 0);

            bm.Dispose();

            OnDragEnter(e);
        }

        private void empty_DragOver(object sender, DragEventArgs e)
        {
            if (e.IsDataPresent<ProviderTokenTileDragEventArgs>())
            {
                var args = e.GetData<ProviderTokenTileDragEventArgs>();
                args.Location = new Point(e.X, e.Y);
            }

            OnDragOver(e);
        }

        private void empty_DragLeave(object sender, EventArgs e)
        {
            OnDragLeave(e);
        }

        private void _panel_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
                return;

            _isMouseDown = true;
            _lastMouseDown = e.Location;
        }

        private void _panel_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
            _isMouseDown = false;
        }

        private void _panel_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);

            Control ctrl = sender as Control;
            if (!_isMouseDown || e.Button != MouseButtons.Left || !AllowDrag || _token == null || ctrl == null)
                return;

            int dX = Math.Abs(e.Location.X - _lastMouseDown.X);
            int dY = Math.Abs(e.Location.Y - _lastMouseDown.Y);
            Point dMax = GetDragThreshold();
            if (dX < dMax.X && dY < dMax.Y)
                return;

            _isMouseDown = false;

            var args = new ProviderTokenTileDragEventArgs()
            {
                TokenTile = this,
                Cancel = false,
                Handled = false,
                Location = ctrl.PointToScreen(e.Location)
            };
            AboutToDrag(this, args);
            if (!args.Cancel)
            {
                Debug.WriteLine(this.Name + " start drag");
                this.DoDragDrop(args, DragDropEffects.Copy);
            }
        }

        private void _panel_Paint(object sender, PaintEventArgs e)
        {
            paint(e.Graphics);
        }
        #endregion

        #region Private Helpers
        #region Cursor Helpers
        private struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        private static Cursor CreateCursor(Bitmap bmp,
            int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new Cursor(CreateIconIndirect(ref tmp));
        }
        #endregion

        #region Drag Helpers
        private const int SM_CXDRAG = 68;
        private const int SM_CYDRAG = 69;
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int index);

        private static Point GetDragThreshold()
        {
            return new Point(GetSystemMetrics(SM_CXDRAG), GetSystemMetrics(SM_CYDRAG));
        }
        #endregion

        private void paint(Graphics g)
        {
            if (_token != null)
            {
                Color main = _token.Color;
                Color lite = ControlPaint.LightLight(main);
                Color dark = ControlPaint.Dark(main, 0.01f);
                Brush bg = new LinearGradientBrush(_panel.DisplayRectangle, lite, dark, LinearGradientMode.Vertical);
                Pen border = new Pen(main);
                g.FillRectangle(bg, _panel.DisplayRectangle);
                g.DrawRectangle(border, _panel.DisplayRectangle);
                g.DrawString(_token.Name, _font, Brushes.Black,
                    new RectangleF(2f, 2f, _panel.DisplayRectangle.Width - 4,
                        _panel.DisplayRectangle.Height - 4));
            }
        }
        #endregion
    }
}
