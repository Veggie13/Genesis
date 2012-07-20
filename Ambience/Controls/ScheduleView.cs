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
    public partial class ScheduleView : UserControl
    {
        private Dictionary<int, EventToken[,]> _history = new Dictionary<int, EventToken[,]>();

        public ScheduleView()
        {
            InitializeComponent();

            _view.Paint += new PaintEventHandler(_view_Paint);
            _view.MouseHover += new EventHandler(_view_MouseHover);
            _view.MouseMove += new MouseEventHandler(_view_MouseMove);
            _view.MouseClick += new MouseEventHandler(_view_MouseClick);

            _hScroll.ValueChanged += new EventHandler(_hScroll_ValueChanged);
            _vScroll.ValueChanged += new EventHandler(_vScroll_ValueChanged);

            this.Invalidated += new InvalidateEventHandler(ScheduleView_Invalidated);

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
        }

        void _view_MouseClick(object sender, MouseEventArgs e)
        {
            int col = LeftColumn + e.Location.X / ColumnWidth;
            int row = TopRow + (e.Location.Y - TrueScaleHeight) / RowHeight;
            if (e.Location.Y < TrueScaleHeight)
                row = -1;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken orig = (row >= 0 && row < origCol.Length) ? origCol[row] : null;

            if (orig != null && TokenMouseClick != null)
            {
                Point pt = new Point(e.Location.X, e.Location.Y);
                TokenMouseClick(orig, _view, pt);
            }
        }

        void ScheduleView_Invalidated(object sender, InvalidateEventArgs e)
        {
            _view.Invalidate();
        }

        public delegate void TokenMouseEvent(EventToken token, Control sender, Point loc);
        public event TokenMouseEvent TokenMouseEnter;
        public event TokenMouseEvent TokenMouseLeave;
        public event TokenMouseEvent TokenMouseHover;
        public event TokenMouseEvent TokenMouseClick;

        private Point _lastMousePos = new Point();
        void _view_MouseMove(object sender, MouseEventArgs e)
        {
            int col = LeftColumn + _lastMousePos.X / ColumnWidth;
            int row = TopRow + (_lastMousePos.Y - TrueScaleHeight) / RowHeight;
            if (_lastMousePos.Y < TrueScaleHeight)
                row = -1;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken orig = (row >= 0 && row < origCol.Length) ? origCol[row] : null;

            _lastMousePos.X = e.Location.X;
            _lastMousePos.Y = e.Location.Y;

            col = LeftColumn + _lastMousePos.X / ColumnWidth;
            row = TopRow + (_lastMousePos.Y - TrueScaleHeight) / RowHeight;
            if (_lastMousePos.Y < TrueScaleHeight)
                row = -1;
            EventToken[] nextCol = GetInstantTokens(col);
            EventToken next = (row >= 0 && row < nextCol.Length) ? nextCol[row] : null;

            if (orig != next)
            {
                if (orig != null && TokenMouseLeave != null)
                {
                    Point pt1 = new Point(e.Location.X, e.Location.Y);
                    TokenMouseLeave(orig, _view, pt1);
                }
                if (next != null && TokenMouseEnter != null)
                {
                    Point pt2 = new Point(e.Location.X, e.Location.Y);
                    TokenMouseEnter(next, _view, pt2);
                }
            }
        }

        void _view_MouseHover(object sender, EventArgs e)
        {
            int col = LeftColumn + _lastMousePos.X / ColumnWidth;
            int row = TopRow + (_lastMousePos.Y - TrueScaleHeight) / RowHeight;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken orig = (row < origCol.Length) ? origCol[row - TopRow] : null;

            if (orig != null && TokenMouseHover != null)
            {
                Point pt = new Point(_lastMousePos.X, _lastMousePos.Y);
                TokenMouseHover(orig, _view, pt);
            }
        }

        void _vScroll_ValueChanged(object sender, EventArgs e)
        {
            if (!_updatingScroll)
            {
                _topRow = _vScroll.Value;
                _view.Invalidate();
            }
        }

        void _hScroll_ValueChanged(object sender, EventArgs e)
        {
            if (!_updatingScroll)
            {
                _leftCol = _hScroll.Value;
                _view.Invalidate();
            }
        }

        private EventSchedule _sched;
        [Browsable(false)]
        public EventSchedule Schedule
        {
            get { return _sched; }
            set
            {
                if (_sched != null)
                {
                    _sched.ScheduleExtended -= _sched_ScheduleExtended;
                }

                _sched = value;
                _history.Clear();

                if (_sched != null)
                {
                    _sched.ScheduleExtended += new EventSchedule.ScheduleExtend(_sched_ScheduleExtended);
                    _rowCount = 0;
                    _colCount = 0;
                    _leftCol = 0;
                    _topRow = 0;
                    updateHistory();
                }
            }
        }

        private IEventColorProvider _colorer;
        [Browsable(false)]
        public IEventColorProvider ColorProvider
        {
            get { return _colorer; }
            set
            {
                _colorer = value;
                _view.Invalidate();
            }
        }

        void  _sched_ScheduleExtended(EventSchedule sched)
        {
            updateHistory();
        }

        private int _colWidth = 50;
        [DefaultValue(50)]
        public int ColumnWidth
        {
            get { return _colWidth; }
            set
            {
                _colWidth = value;
                updateScrollBars();
            }
        }

        private int _rowHeight = 15;
        [DefaultValue(15)]
        public int RowHeight
        {
            get { return _rowHeight; }
            set
            {
                _rowHeight = value;
                updateScrollBars();
            }
        }

        private int _leftCol = 0;
        [DefaultValue(0)]
        public int LeftColumn
        {
            get { return _leftCol; }
            set
            {
                _leftCol = value;
                _hScroll.Value = _leftCol;
            }
        }

        private int _topRow = 0;
        [DefaultValue(0)]
        public int TopRow
        {
            get { return _topRow; }
            set
            {
                _topRow = value;
                _vScroll.Value = _topRow;
            }
        }

        private int _colCount;
        [Browsable(false)]
        public int ColumnCount
        {
            get { return _colCount; }
        }

        private int _rowCount;
        [Browsable(false)]
        public int RowCount
        {
            get { return _rowCount; }
        }

        public int DisplayedRowCount
        {
            get { return (DisplayRectangle.Height - TrueScaleHeight) / RowHeight; }
        }

        public int DisplayedColumnCount
        {
            get { return DisplayRectangle.Width / ColumnWidth; }
        }

        private bool _showScale = false;
        [DefaultValue(false)]
        public bool ShowScale
        {
            get { return _showScale; }
            set
            {
                _showScale = value;
                updateScrollBars();
            }
        }

        private int _scaleHeight = 15;
        [DefaultValue(15)]
        public int ScaleHeight
        {
            get { return _scaleHeight; }
            set
            {
                _scaleHeight = value;
                updateScrollBars();
            }
        }

        private int TrueScaleHeight
        {
            get { return ShowScale ? ScaleHeight : 0; }
        }

        private SolidBrush _scaleBg = new SolidBrush(Color.LightGray);
        [DefaultValue(typeof(Color), "LightGray")]
        public Color ScaleBackground
        {
            get { return _scaleBg.Color; }
            set
            {
                _scaleBg.Color = value;
                _view.Invalidate();
            }
        }

        private SolidBrush _bg = new SolidBrush(Color.White);
        [DefaultValue(typeof(Color), "White")]
        public Color Background
        {
            get { return _bg.Color; }
            set
            {
                _bg.Color = value;
                _view.Invalidate();
            }
        }

        private static Pen DefaultBorder = new Pen(Color.LightGray, 1f);
        private Pen _borders = new Pen(DefaultBorder.Color, DefaultBorder.Width);
        public Color BorderColor
        {
            get { return _borders.Color; }
            set
            {
                _borders.Color = value;
                _view.Invalidate();
            }
        }
        private bool ShouldSerializeBorderColor()
        {
            return !(_borders.Color.Equals(DefaultBorder.Color));
        }

        public float BorderThickness
        {
            get { return _borders.Width; }
            set
            {
                _borders.Width = value;
                _view.Invalidate();
            }
        }
        private bool ShouldSerializeBorderThickness()
        {
            return (_borders.Width != DefaultBorder.Width);
        }

        private static readonly Font DefaultTokenFont = new Font("Arial", 8f);
        private Font _font = DefaultTokenFont;
        private Font _bold = new Font(DefaultTokenFont, FontStyle.Bold);
        private Font _italic = new Font(DefaultTokenFont, FontStyle.Italic);
        public Font TokenFont
        {
            get { return _font; }
            set
            {
                Font font = (value == null) ? DefaultTokenFont : value;
                _font = new Font(font, FontStyle.Regular);
                _bold = new Font(font, FontStyle.Bold);
                _italic = new Font(font, FontStyle.Italic);
                _view.Invalidate();
            }
        }
        private bool ShouldSerializeTokenFont()
        {
            return !_font.Equals(DefaultTokenFont);
        }

        private SolidBrush _fontColor = new SolidBrush(Color.Black);
        [DefaultValue(typeof(Color), "Black")]
        public Color TokenFontColor
        {
            get { return _fontColor.Color; }
            set
            {
                _fontColor.Color = value;
                _view.Invalidate();
            }
        }

        private SolidBrush _fontHighlight = new SolidBrush(Color.White);
        [DefaultValue(typeof(Color), "White")]
        public Color TokenFontHighlightColor
        {
            get { return _fontHighlight.Color; }
            set
            {
                _fontHighlight.Color = value;
                _view.Invalidate();
            }
        }

        void _view_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode)
                drawTable(e.Graphics);
            else if (_sched != null)
            {
                drawTable(e.Graphics);
                drawTokens(e.Graphics);
            }
        }

        private void drawTable(Graphics gc)
        {
            gc.FillRectangle(_bg, DisplayRectangle);
            int scaleBottom = TrueScaleHeight;
            if (ShowScale)
            {
                gc.FillRectangle(_scaleBg, 0, 0, DisplayRectangle.Width, ScaleHeight);
            }
            for (int x = 0; x < DisplayRectangle.Width; x += _colWidth)
            {
                gc.DrawLine(_borders, x, 0, x, DisplayRectangle.Height);
            }
            for (int y = scaleBottom; y < DisplayRectangle.Height; y += _rowHeight)
            {
                gc.DrawLine(_borders, 0, y, DisplayRectangle.Width, y);
            }
        }

        private void drawTokens(Graphics gc)
        {
            EventToken[] prevBlock = GetInstantTokens(LeftColumn - 1);
            EventToken[] block = GetInstantTokens(LeftColumn);
            for (int c = 0; c < DisplayedColumnCount; c++)
            {
                int trueCol = LeftColumn + c;
                EventToken[] nextBlock = GetInstantTokens(trueCol + 1);
                for (int r = 0; r < DisplayedRowCount; r++)
                {
                    int trueRow = TopRow + r;
                    EventToken token = (block.Length > trueRow) ? block[trueRow] : null;
                    if (token != null)
                    {
                        EventToken prevToken = (prevBlock.Length > trueRow) ? prevBlock[trueRow] : null;
                        EventToken nextToken = (nextBlock.Length > trueRow) ? nextBlock[trueRow] : null;

                        drawLeftToken(gc, r, c, token, (prevToken == null || prevToken != token));
                        drawRightToken(gc, r, c, token, (nextToken == null || nextToken != token));
                        if (prevToken == null || prevToken != token)
                            drawTokenText(gc, r, c, token, false);
                        else if (c == 0)
                            drawTokenText(gc, r, c, token, true);
                    }
                }
                prevBlock = block;
                block = nextBlock;
            }
        }

        private void drawLeftToken(Graphics gc, int dispRow, int dispCol, EventToken token, bool edge)
        {
            int x1 = _colWidth * dispCol;
            int x2 = x1 + _colWidth / 2 + 1;
            int y1 = TrueScaleHeight + _rowHeight * dispRow;
            int y2 = y1 + _rowHeight;

            /*
            gc.FillRectangle(new SolidBrush(token.Color), x1, y1, _colWidth / 2 + 1, _rowHeight);
            gc.DrawLine(Pens.Black, x1, y1, x2, y1);
            gc.DrawLine(Pens.Black, x1, y2, x2, y2);
            if (edge)
                gc.DrawLine(Pens.Black, x1, y1, x1, y2);
             */
            Color def = token.Color, lite = ControlPaint.LightLight(def);
            Color main = token.IsHighlighted ? def : lite;
            Brush fillBrush = new SolidBrush(main);
            if (edge)
            {
                gc.FillRectangle(fillBrush, x1 + 4, y1, _colWidth / 2 - 3, _rowHeight);
                gc.FillRectangle(fillBrush, x1 + 2, y1 + 1, 2, _rowHeight - 2);
                gc.FillRectangle(fillBrush, x1 + 1, y1 + 2, 1, _rowHeight - 4);
                gc.FillRectangle(fillBrush, x1, y1 + 4, 1, _rowHeight - 8);
            }
            else
            {
                gc.FillRectangle(fillBrush, x1, y1, _colWidth / 2 + 1, _rowHeight);
            }
        }

        private void drawRightToken(Graphics gc, int dispRow, int dispCol, EventToken token, bool edge)
        {
            int x2 = _colWidth * (dispCol + 1);
            int x1 = x2 - _colWidth / 2 - 1;
            int y1 = TrueScaleHeight + _rowHeight * dispRow;
            int y2 = y1 + _rowHeight;

            /*
            gc.FillRectangle(new SolidBrush(token.Color), x1, y1, _colWidth / 2 + 1, _rowHeight);
            gc.DrawLine(Pens.Black, x1, y1, x2, y1);
            gc.DrawLine(Pens.Black, x1, y2, x2, y2);
            if (edge)
                gc.DrawLine(Pens.Black, x2, y1, x2, y2);
             */
            Color def = token.Color, lite = ControlPaint.LightLight(def);
            Color main = token.IsHighlighted ? def : lite;
            Brush fillBrush = new SolidBrush(main);
            if (edge)
            {
                gc.FillRectangle(fillBrush, x1, y1, _colWidth / 2 - 2, _rowHeight);
                gc.FillRectangle(fillBrush, x2 - 3, y1 + 1, 2, _rowHeight - 2);
                gc.FillRectangle(fillBrush, x2 - 1, y1 + 2, 1, _rowHeight - 4);
                gc.FillRectangle(fillBrush, x2, y1 + 4, 1, _rowHeight - 8);
            }
            else
            {
                gc.FillRectangle(fillBrush, x1, y1, _colWidth / 2 + 1, _rowHeight);
            }
        }

        private void drawTokenText(Graphics gc, int dispRow, int dispCol, EventToken token, bool continued)
        {
            int x1 = _colWidth * dispCol;
            int y1 = TrueScaleHeight + _rowHeight * dispRow;

            string text = (continued ? "<< " : "") + token.Name;
            gc.DrawString(text, (continued ? _italic : _bold),
                token.IsHighlighted ? _fontHighlight : _fontColor,
                new RectangleF(x1, y1, _colWidth, _rowHeight));
        }

        private bool _updatingScroll = false;
        private void updateScrollBars()
        {
            _updatingScroll = true;
            if (_sched == null)
            {
                _vScroll.Visible = false;
                _hScroll.Minimum = 0;
                _hScroll.Maximum = 0;
                TopRow = 0;
                LeftColumn = 0;
            }
            else
            {
                _vScroll.Visible = (DisplayedRowCount < RowCount);
                if (_vScroll.Visible)
                {
                    _vScroll.Minimum = 0;
                    _vScroll.Maximum = RowCount - DisplayedRowCount;
                    _vScroll.Value = TopRow;
                }
                _hScroll.Minimum = 0;
                _hScroll.Maximum = (DisplayedColumnCount < ColumnCount) ?
                    (ColumnCount - DisplayedColumnCount) :
                    0;
                _hScroll.Value = LeftColumn;
            }
            _updatingScroll = false;

            _view.Invalidate();
        }

        private void ScheduleView_Load(object sender, EventArgs e)
        {
            updateScrollBars();
        }

        private EventToken[] GetInstantTokens(int index)
        {
            var lessKeys = _history.Keys.Where((k) => (k <= index));
            if (lessKeys.Count() < 1)
                return new EventToken[0];
            int group = lessKeys.Max();
            EventToken[,] all = _history[group];
            if (index - group >= all.GetLength(1))
                return new EventToken[0];
            int len = all.GetLength(0);
            var result = new EventToken[len];
            for (int i = 0; i < len; i++)
                result[i] = all[i, index - group];
            return result;
        }

        private void updateHistory()
        {
            int curCol;
            var block = _sched.GetCurrentBlock(out curCol);
            int start = _colCount - curCol;

            var result = new EventToken[_rowCount, block.Length - start];
            
            var lastTokens = GetInstantTokens(_colCount - 1);
            for (int r = 0; r < lastTokens.Length; r++)
            {
                if (lastTokens[r] != null && lastTokens[r].Last >= _colCount)
                {
                    for (int c = 0; c < result.GetLength(1) && _colCount + c <= lastTokens[r].Last; c++)
                        result[r, c] = lastTokens[r];
                }
            }

            for (int c = 0; c < block.Length; c++)
            {
                foreach (IScheduleEvent evt in block[c])
                {
                    EventToken token = new EventToken(_colCount + c, evt, _colorer);
                    bool invalid = true;
                    int r;
                    for (r = 0; invalid && r < result.GetLength(0); r++)
                    {
                        invalid = false;
                        for (int cc = c; cc < block.Length && _colCount + cc <= token.Last; cc++)
                        {
                            if (result[r, cc] != null)
                            {
                                invalid = true;
                                break;
                            }
                        }
                    }
                    if (invalid)
                    {
                        r = (++_rowCount) - 1;
                        var newResult = new EventToken[_rowCount, result.GetLength(1)];
                        Array.Copy(result, newResult, result.Length);
                        result = newResult;
                    }

                    for (int cc = c; cc < block.Length && _colCount + cc <= token.Last; cc++)
                        result[r, cc] = token;
                }
            }

            _history[_colCount] = result;
            _colCount += result.GetLength(1);

            updateScrollBars();
        }
    }
}
