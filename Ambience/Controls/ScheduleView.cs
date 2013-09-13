﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;
using System.Drawing.Drawing2D;

namespace Genesis.Ambience.Controls
{
    public partial class ScheduleView : UserControl
    {
        #region Private Members
        private TokenHistory _tokenHistory = new TokenHistory();
        private int _curIndex = -1;
        private Point _lastMousePos = new Point();
        private bool _updatingScroll = false;
        private Bitmap _drawingBuffer;
        #endregion

        #region Constructor
        public ScheduleView()
        {
            InitializeComponent();

            _view.Paint += new PaintEventHandler(_view_Paint);
            _view.MouseHover += new EventHandler(_view_MouseHover);
            _view.MouseMove += new MouseEventHandler(_view_MouseMove);
            _view.MouseEnter += new EventHandler(_view_MouseEnter);
            _view.MouseLeave += new EventHandler(_view_MouseLeave);
            _view.MouseClick += new MouseEventHandler(_view_MouseClick);
            _view.Resize += new EventHandler(_view_Resize);

            _hScroll.ValueChanged += new EventHandler(_hScroll_ValueChanged);
            _vScroll.ValueChanged += new EventHandler(_vScroll_ValueChanged);

            this.Invalidated += new InvalidateEventHandler(ScheduleView_Invalidated);

            // Set defaults
            this.UseDrawingBuffer = DefaultUseDrawingBuffer;
            this._tokenHistory.BlockWidth = 256;

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
        }
        #endregion

        #region Events
        public delegate void TokenMouseEvent(EventToken token, Control sender, Point loc);
        public event TokenMouseEvent TokenMouseEnter;
        public event TokenMouseEvent TokenMouseLeave;
        public event TokenMouseEvent TokenMouseHover;
        public event TokenMouseEvent TokenMouseClick;

        public delegate void ViewValueChangeEvent(ScheduleView sender, int oldValue, int newValue);
        public event ViewValueChangeEvent LeftColumnChanged;
        public event ViewValueChangeEvent TopRowChanged;
        #endregion

        #region Properties
        #region Schedule
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
                    _sched.Tick -= _sched_Tick;
                    _sched.Started -= _sched_Started;
                    _sched.Finished -= _sched_Finished;
                    _sched.ScheduleChanged -= _sched_ScheduleChanged;
                }

                _sched = value;
                _tokenHistory.Clear();

                if (_sched != null)
                {
                    _sched.ScheduleExtended += new EventSchedule.TimeEvent(_sched_ScheduleExtended);
                    _sched.Tick += new EventSchedule.TimeEvent(_sched_Tick);
                    _sched.Started += new EventSchedule.Trigger(_sched_Started);
                    _sched.Finished += new EventSchedule.Trigger(_sched_Finished);
                    _sched.ScheduleChanged += new EventSchedule.TimeEvent(_sched_ScheduleChanged);
                    _rowCount = 0;
                    _colCount = 0;
                    _leftCol = 0;
                    _topRow = 0;
                    updateHistory(0);
                    invalidateView();
                }
            }
        }
        #endregion

        #region ColorProvider
        private IEventColorProvider _colorer;
        [Browsable(false)]
        public IEventColorProvider ColorProvider
        {
            get { return _colorer; }
            set
            {
                _colorer = value;
                invalidateView();
            }
        }
        #endregion

        #region ColumnCount
        private int _colCount;
        [Browsable(false)]
        public int ColumnCount
        {
            get { return (int)_tokenHistory.ColumnCount; }
        }
        #endregion

        #region RowCount
        private int _rowCount;
        [Browsable(false)]
        public int RowCount
        {
            get { return _rowCount; }
        }
        #endregion

        #region UI
        #region ColumnWidth
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
        #endregion

        #region RowHeight
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
        #endregion

        #region LeftColumn
        private int _leftCol = 0;
        [DefaultValue(0)]
        public int LeftColumn
        {
            get { return _leftCol; }
            set
            {
                if (_leftCol != value)
                {
                    int old = _leftCol;
                    _leftCol = value;
                    hScrollUpdate(_leftCol);
                    if (LeftColumnChanged != null)
                        LeftColumnChanged(this, old, _leftCol);
                }
            }
        }
        #endregion

        #region TopRow
        private int _topRow = 0;
        [DefaultValue(0)]
        public int TopRow
        {
            get { return _topRow; }
            set
            {
                if (_topRow != value)
                {
                    int old = _topRow;
                    _topRow = value;
                    vScrollUpdate(_topRow);
                    if (TopRowChanged != null)
                        TopRowChanged(this, old, _topRow);
                }
            }
        }
        #endregion

        public double ActualDisplayedRowCount
        {
            get { return (double)(_view.DisplayRectangle.Height - TrueScaleHeight) / RowHeight; }
        }

        public double ActualDisplayedColumnCount
        {
            get { return (double)_view.DisplayRectangle.Width / ColumnWidth; }
        }

        public int DisplayedRowCount
        {
            get { return (int)Math.Ceiling(ActualDisplayedRowCount); }
        }

        public int DisplayedColumnCount
        {
            get { return (int)Math.Ceiling(ActualDisplayedColumnCount); }
        }

        public int FullDisplayedRowCount
        {
            get { return (int)Math.Floor(ActualDisplayedRowCount); }
        }

        public int FullDisplayedColumnCount
        {
            get { return (int)Math.Floor(ActualDisplayedColumnCount); }
        }

        #region ShowScale
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
        #endregion

        #region ScaleHeight
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
        #endregion

        #region ScaleBackground
        private SolidBrush _scaleBg = new SolidBrush(Color.LightGray);
        [DefaultValue(typeof(Color), "LightGray")]
        public Color ScaleBackground
        {
            get { return _scaleBg.Color; }
            set
            {
                _scaleBg.Color = value;
                invalidateView();
            }
        }
        #endregion

        #region ScaleFont
        private static readonly Font DefaultScaleFont = new Font("Arial", 8f);
        private Font _scaleFont = DefaultScaleFont;
        public Font ScaleFont
        {
            get { return _scaleFont; }
            set
            {
                Font font = (value == null) ? DefaultScaleFont : value;
                _scaleFont = new Font(font, font.Style);
                invalidateView();
            }
        }
        private bool ShouldSerializeScaleFont()
        {
            return !_scaleFont.Equals(DefaultScaleFont);
        }
        #endregion

        #region ScaleFontColor
        private SolidBrush _scaleColor = new SolidBrush(Color.Black);
        [DefaultValue(typeof(Color), "Black")]
        public Color ScaleFontColor
        {
            get { return _scaleColor.Color; }
            set
            {
                _scaleColor.Color = value;
                invalidateView();
            }
        }
        #endregion

        #region ScaleInterval
        private static TimeSpan DefaultScaleInterval = new TimeSpan(0, 0, 1);
        private TimeSpan _scaleInterval = DefaultScaleInterval;
        public TimeSpan ScaleInterval
        {
            get { return _scaleInterval; }
            set
            {
                _scaleInterval = value;
                invalidateView();
            }
        }
        private bool ShouldSerializeScaleInterval()
        {
            return !(_scaleInterval.Equals(DefaultScaleInterval));
        }

        [DefaultValue(1000)]
        public int ScaleIntervalMs
        {
            get { return (int)_scaleInterval.TotalMilliseconds; }
            set
            {
                _scaleInterval = new TimeSpan(0, 0, 0, 0, value);
                invalidateView();
            }
        }
        #endregion

        #region ScaleFormat
        private string _scaleFormat = "M:ss";
        [DefaultValue("M:ss")]
        public string ScaleFormat
        {
            get { return _scaleFormat; }
            set
            {
                _scaleFormat = value;
                invalidateView();
            }
        }
        #endregion

        #region Background
        private SolidBrush _bg = new SolidBrush(Color.White);
        [DefaultValue(typeof(Color), "White")]
        public Color Background
        {
            get { return _bg.Color; }
            set
            {
                _bg.Color = value;
                invalidateView();
            }
        }
        #endregion

        #region Border
        private static Pen DefaultBorder = new Pen(Color.LightGray, 1f);
        private Pen _borders = new Pen(DefaultBorder.Color, DefaultBorder.Width);

        #region BorderColor
        public Color BorderColor
        {
            get { return _borders.Color; }
            set
            {
                _borders.Color = value;
                invalidateView();
            }
        }
        private bool ShouldSerializeBorderColor()
        {
            return !(_borders.Color.Equals(DefaultBorder.Color));
        }
        #endregion

        #region BorderThickness
        public float BorderThickness
        {
            get { return _borders.Width; }
            set
            {
                _borders.Width = value;
                invalidateView();
            }
        }
        private bool ShouldSerializeBorderThickness()
        {
            return (_borders.Width != DefaultBorder.Width);
        }
        #endregion
        #endregion

        #region TokenFont
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
                invalidateView();
            }
        }
        private bool ShouldSerializeTokenFont()
        {
            return !_font.Equals(DefaultTokenFont);
        }
        #endregion

        #region TokenFontColor
        private SolidBrush _fontColor = new SolidBrush(Color.Black);
        [DefaultValue(typeof(Color), "Black")]
        public Color TokenFontColor
        {
            get { return _fontColor.Color; }
            set
            {
                _fontColor.Color = value;
                invalidateView();
            }
        }
        #endregion

        #region TokenFontHighlightColor
        private SolidBrush _fontHighlight = new SolidBrush(Color.White);
        [DefaultValue(typeof(Color), "White")]
        public Color TokenFontHighlightColor
        {
            get { return _fontHighlight.Color; }
            set
            {
                _fontHighlight.Color = value;
                invalidateView();
            }
        }
        #endregion

        #region TrailingIndicatorColor
        private Pen _trail = new Pen(Color.BlueViolet, 2f);
        [DefaultValue(typeof(Color), "BlueViolet")]
        public Color TrailingIndicatorColor
        {
            get { return _trail.Color; }
            set
            {
                _trail.Color = value;
                invalidateView();
            }
        }
        #endregion

        #region LeadingIndicatorColor
        private Pen _lead = new Pen(Color.MediumVioletRed, 2f);
        [DefaultValue(typeof(Color), "MediumVioletRed")]
        public Color LeadingIndicatorColor
        {
            get { return _lead.Color; }
            set
            {
                _lead.Color = value;
                invalidateView();
            }
        }
        #endregion

        #region ShowIndicators
        private bool _showIndicators = true;
        [DefaultValue(true)]
        public bool ShowIndicators
        {
            get { return _showIndicators; }
            set
            {
                _showIndicators = value;
                invalidateView();
            }
        }
        #endregion
        #endregion

        #region UseDrawingBuffer
        private const bool DefaultUseDrawingBuffer = true;
        [DefaultValue(DefaultUseDrawingBuffer)]
        public bool UseDrawingBuffer
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Event Handlers
        #region ScheduleView
        private void ScheduleView_Load(object sender, EventArgs e)
        {
            updateScrollBars();


        }

        private void ScheduleView_Invalidated(object sender, InvalidateEventArgs e)
        {
            invalidateView();
        }
        #endregion

        #region _sched
        private void _sched_Finished(EventSchedule sched)
        {
            _curIndex = -1;
            invalidateView();
        }

        private void _sched_Started(EventSchedule sched)
        {
            _curIndex = 0;
            invalidateView();
        }

        private void _sched_Tick(EventSchedule sched, ulong newTimeCode)
        {
            EventToken[] curTokens = GetInstantTokens((int)newTimeCode);
            for (int col = _curIndex; col < (int)newTimeCode; col++)
            {
                EventToken[] tokens = GetInstantTokens(col);
                for (int row = 0; row < tokens.Length; row++)
                {
                    EventToken token = tokens[row];
                    if (token != null && token.Event != null && (curTokens.Length <= row || curTokens[row] != token))
                        token.Finish();
                }
            }

            bool wasVisible = indexIsVisible(_curIndex);
            _curIndex = (int)newTimeCode;
            if (wasVisible && !indexIsVisible(_curIndex))
            {
                LeftColumn = Math.Min(_curIndex, ColumnCount - FullDisplayedColumnCount + 1);
            }
            else
            {
                invalidateView();
            }
        }

        private void _sched_ScheduleExtended(EventSchedule sched, ulong timeCode)
        {
            updateHistory(timeCode);
        }

        private void _sched_ScheduleChanged(EventSchedule sched, ulong timeCode)
        {
            updateHistory(timeCode);
            invalidateView();
        }
        #endregion

        #region _view
        private void _view_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = UseDrawingBuffer ? Graphics.FromImage(_drawingBuffer) : e.Graphics;

            if (DesignMode)
                drawTable(g);
            else if (_sched != null)
            {
                drawTable(g);
                drawTokens(g);
                if (ShowIndicators)
                    drawIndicators(g);
            }

            if (UseDrawingBuffer)
            {
                e.Graphics.DrawImage(_drawingBuffer, 0, 0);
            }
        }

        private void _view_MouseEnter(object sender, EventArgs e)
        {
            _lastMousePos = _view.PointToClient(Cursor.Position);

            int row, col;
            if (!getVisibleCell(_lastMousePos, out col, out row))
                row = -1;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken cell = (row >= 0 && row < origCol.Length) ? origCol[row] : null;

            if (cell != null && TokenMouseEnter != null)
            {
                Point pt = new Point(_lastMousePos.X, _lastMousePos.Y);
                TokenMouseEnter(cell, _view, pt);
            }
        }

        private void _view_MouseLeave(object sender, EventArgs e)
        {
            int row, col;
            if (!getVisibleCell(_lastMousePos, out col, out row))
                row = -1;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken cell = (row >= 0 && row < origCol.Length) ? origCol[row] : null;

            if (cell != null && TokenMouseLeave != null)
            {
                Point pt = new Point(_lastMousePos.X, _lastMousePos.Y);
                TokenMouseLeave(cell, _view, pt);
            }
        }

        private void _view_MouseClick(object sender, MouseEventArgs e)
        {
            int row, col;
            if (!getVisibleCell(e.Location, out col, out row))
                row = -1;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken orig = (row >= 0 && row < origCol.Length) ? origCol[row] : null;

            if (orig != null && TokenMouseClick != null)
            {
                Point pt = new Point(e.Location.X, e.Location.Y);
                TokenMouseClick(orig, _view, pt);
            }
        }

        private void _view_MouseMove(object sender, MouseEventArgs e)
        {
            int row, col;
            if (!getVisibleCell(_lastMousePos, out col, out row))
                row = -1;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken orig = (row >= 0 && row < origCol.Length) ? origCol[row] : null;

            _lastMousePos.X = e.Location.X;
            _lastMousePos.Y = e.Location.Y;

            if (!getVisibleCell(_lastMousePos, out col, out row))
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

        private void _view_MouseHover(object sender, EventArgs e)
        {
            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            bool rowHangOver = (TopRow > 0 && TopRow + DisplayedRowCount > RowCount);
            int xLeft = colHangOver ? (ViewRectangle.Width - DisplayedColumnCount * _colWidth) : 0;
            int yTop = rowHangOver ? (ViewRectangle.Height - TrueScaleHeight - DisplayedRowCount * _rowHeight) : 0;
            int colOffset = colHangOver ? -1 : 0;
            int rowOffset = rowHangOver ? -1 : 0;

            int col = LeftColumn + colOffset + (_lastMousePos.X - xLeft) / ColumnWidth;
            int row = TopRow + rowOffset + (_lastMousePos.Y - TrueScaleHeight - yTop) / RowHeight;
            EventToken[] origCol = GetInstantTokens(col);
            EventToken orig = (row < origCol.Length) ? origCol[row] : null;

            if (orig != null && TokenMouseHover != null)
            {
                Point pt = new Point(_lastMousePos.X, _lastMousePos.Y);
                TokenMouseHover(orig, _view, pt);
            }
        }

        private void _view_Resize(object sender, EventArgs e)
        {
            if (UseDrawingBuffer)
                resizeDrawingBuffer();
        }
        #endregion

        #region Scrollbars
        private void _vScroll_ValueChanged(object sender, EventArgs e)
        {
            if (!_updatingScroll)
            {
                TopRow = _vScroll.Value;
                invalidateView();
            }
        }

        private void _hScroll_ValueChanged(object sender, EventArgs e)
        {
            if (!_updatingScroll)
            {
                LeftColumn = _hScroll.Value;
                invalidateView();
            }
        }
        #endregion
        #endregion

        #region Private Helpers
        #region Properties
        private int TrueScaleHeight
        {
            get { return ShowScale ? ScaleHeight : 0; }
        }

        private Rectangle ViewRectangle
        {
            get { return _view.DisplayRectangle; }
        }
        #endregion

        #region Scrollbars
        private void hScrollUpdate(int val)
        {
            if (InvokeRequired)
                Invoke(new Action<int>(hScrollUpdate), val);
            else
                _hScroll.Value = val;
        }

        private void vScrollUpdate(int val)
        {
            if (InvokeRequired)
                Invoke(new Action<int>(vScrollUpdate), val);
            else
                _vScroll.Value = val;
        }

        private void updateScrollBars()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(updateScrollBars));
                return;
            }

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
                _vScroll.Visible = (ActualDisplayedRowCount < RowCount);
                if (_vScroll.Visible)
                {
                    _vScroll.Minimum = 0;
                    _vScroll.Maximum = RowCount - 1;
                    _vScroll.LargeChange = FullDisplayedRowCount;
                    _vScroll.Value = TopRow;
                }
                _hScroll.Minimum = 0;
                _hScroll.Maximum = ColumnCount - 1;
                _hScroll.LargeChange = FullDisplayedColumnCount;
                _hScroll.Value = LeftColumn;
            }
            _updatingScroll = false;

            invalidateView();
        }
        #endregion

        #region Drawing
        private void drawTable(Graphics gc)
        {
            gc.FillRectangle(_bg, ViewRectangle);
            
            int scaleBottom = TrueScaleHeight;
            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            int xLeft = colHangOver ? (ViewRectangle.Width - DisplayedColumnCount * _colWidth) : 0;
            for (int x = xLeft; x < ViewRectangle.Width; x += _colWidth)
            {
                gc.DrawLine(_borders, x, 0, x, ViewRectangle.Height);
            }
            
            bool rowHangOver = (TopRow > 0 && TopRow + DisplayedRowCount > RowCount);
            int yTop = rowHangOver ? (ViewRectangle.Height - scaleBottom - DisplayedRowCount * _rowHeight) : 0;
            for (int y = scaleBottom + yTop; y < ViewRectangle.Height; y += _rowHeight)
            {
                gc.DrawLine(_borders, 0, y, ViewRectangle.Width, y);
            }

            if (ShowScale)
            {
                gc.FillRectangle(_scaleBg, 0, 0, ViewRectangle.Width, ScaleHeight);
                if (_sched != null)
                    drawScaleText(gc);
            }
        }

        private void drawScaleText(Graphics gc)
        {
            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            int xLeft = colHangOver ? (ViewRectangle.Width - DisplayedColumnCount * _colWidth) : 0;

            double ticksPerInterval = _sched.TicksPerSec * _scaleInterval.TotalSeconds;
            int colLeft = (colHangOver ? LeftColumn - 1 : LeftColumn);
            int firstInterval = (int)Math.Ceiling(colLeft / ticksPerInterval);
            double pixPerInterval = ticksPerInterval * _colWidth;
            double xFirst = (pixPerInterval * firstInterval) - (colLeft * _colWidth);
            int nInterval = firstInterval;
            for (double x = xFirst + xLeft; x < ViewRectangle.Width; x += pixPerInterval, nInterval++)
            {
                TimeSpan interval = new TimeSpan(_scaleInterval.Ticks * (long)nInterval);
                gc.DrawString(interval.Format((int)_sched.TicksPerSec, _scaleFormat), _scaleFont, _scaleColor,
                    new RectangleF((int)x + 2, 2, _colWidth - 4, _scaleHeight - 4));
            }
        }

        private class TokenTextItem
        {
            public int Row;
            public int Col;
            public int Width;
            public bool Continued;
        }
        
        private void drawTokens(Graphics gc)
        {
            gc.Clip = new Region(new Rectangle(0, TrueScaleHeight, ViewRectangle.Width, ViewRectangle.Height - TrueScaleHeight));

            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            bool rowHangOver = (TopRow > 0 && TopRow + DisplayedRowCount > RowCount);
            int colOffset = colHangOver ? -1 : 0;
            int rowOffset = rowHangOver ? -1 : 0;

            Dictionary<EventToken, TokenTextItem> textItems = new Dictionary<EventToken, TokenTextItem>();

            EventToken[] prevBlock = GetInstantTokens(LeftColumn - 1);
            EventToken[] block = GetInstantTokens(LeftColumn);
            for (int c = 0; c < DisplayedColumnCount; c++)
            {
                int trueCol = LeftColumn + c + colOffset;
                EventToken[] nextBlock = GetInstantTokens(trueCol + 1);
                for (int r = 0; r < DisplayedRowCount; r++)
                {
                    int trueRow = TopRow + r + rowOffset;
                    EventToken token = (block.Length > trueRow) ? block[trueRow] : null;
                    if (token != null)
                    {
                        EventToken prevToken = (prevBlock.Length > trueRow) ? prevBlock[trueRow] : null;
                        EventToken nextToken = (nextBlock.Length > trueRow) ? nextBlock[trueRow] : null;

                        drawLeftToken(gc, r, c, token, (prevToken == null || prevToken != token));
                        drawRightToken(gc, r, c, token, (nextToken == null || nextToken != token));
                        if (prevToken == null || prevToken != token)
                            textItems[token] = new TokenTextItem() { Row = r, Col = c, Width = 1, Continued = false };
                        else if (c == 0)
                            textItems[token] = new TokenTextItem() { Row = r, Col = c, Width = 1, Continued = true };
                        else
                            textItems[token].Width++;
                    }
                }
                prevBlock = block;
                block = nextBlock;
            }

            foreach (var item in textItems)
            {
                drawTokenText(gc, item.Key, item.Value);
            }
        }

        private void drawIndicators(Graphics gc)
        {
            if (_sched.IsRunning && indexIsVisible(_curIndex))
            {
                int x1 = (_curIndex - LeftColumn) * ColumnWidth;
                int x2 = x1 + ColumnWidth;

                gc.DrawLine(_trail, x1, 0, x1, ViewRectangle.Height);
                gc.DrawLine(_lead, x2, 0, x2, ViewRectangle.Height);
            }
        }

        private void drawLeftToken(Graphics gc, int dispRow, int dispCol, EventToken token, bool edge)
        {
            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            bool rowHangOver = (TopRow > 0 && TopRow + DisplayedRowCount > RowCount);
            int xLeft = colHangOver ? (ViewRectangle.Width - DisplayedColumnCount * _colWidth) : 0;
            int yTop = rowHangOver ? (ViewRectangle.Height - TrueScaleHeight - DisplayedRowCount * _rowHeight) : 0;

            int x1 = _colWidth * dispCol + xLeft;
            int x2 = x1 + _colWidth / 2 + 1;
            int y1 = TrueScaleHeight + _rowHeight * dispRow + yTop;
            int y2 = y1 + _rowHeight - 1;

            /*
            gc.FillRectangle(new SolidBrush(token.Color), x1, y1, _colWidth / 2 + 1, _rowHeight);
            gc.DrawLine(Pens.Black, x1, y1, x2, y1);
            gc.DrawLine(Pens.Black, x1, y2, x2, y2);
            if (edge)
                gc.DrawLine(Pens.Black, x1, y1, x1, y2);
             */
            Color def = token.Color, lite = ControlPaint.LightLight(def);
            Color dark = ControlPaint.Dark(def, 0.01f);
            Color main = token.IsHighlighted ? def : lite;
            Brush fillBrush = token.IsHighlighted ? (Brush)(new SolidBrush(main)) :
                new LinearGradientBrush(new Point(0, y1), new Point(0, y2), lite, dark);
            Pen border = new Pen(token.IsHighlighted ? lite : def, token.IsHighlighted ? 2f : 1f);
            if (edge)
            {
                gc.FillRectangle(fillBrush, x1 + 4, y1, _colWidth / 2 - 3, _rowHeight);
                gc.FillRectangle(fillBrush, x1 + 2, y1 + 1, 2, _rowHeight - 2);
                gc.FillRectangle(fillBrush, x1 + 1, y1 + 2, 1, _rowHeight - 4);
                gc.FillRectangle(fillBrush, x1, y1 + 4, 1, _rowHeight - 8);
                gc.DrawLine(border, x1 + 4, y1, x2, y1);
                gc.DrawLine(border, x1 + 2, y1 + 1, x1 + 3, y1 + 1);
                gc.DrawLine(border, x1 + 1, y1 + 2, x1 + 1, y1 + 3);
                gc.DrawLine(border, x1, y1 + 4, x1, y2 - 4);
                gc.DrawLine(border, x1 + 1, y2 - 2, x1 + 1, y2 - 3);
                gc.DrawLine(border, x1 + 2, y2 - 1, x1 + 3, y2 - 1);
                gc.DrawLine(border, x1 + 4, y2, x2, y2);
            }
            else
            {
                gc.FillRectangle(fillBrush, x1, y1, _colWidth / 2 + 1, _rowHeight);
                gc.DrawLine(border, x1, y1, x2, y1);
                gc.DrawLine(border, x1, y2, x2, y2);
            }
        }

        private void drawRightToken(Graphics gc, int dispRow, int dispCol, EventToken token, bool edge)
        {
            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            bool rowHangOver = (TopRow > 0 && TopRow + DisplayedRowCount > RowCount);
            int xLeft = colHangOver ? (ViewRectangle.Width - DisplayedColumnCount * _colWidth) : 0;
            int yTop = rowHangOver ? (ViewRectangle.Height - TrueScaleHeight - DisplayedRowCount * _rowHeight) : 0;

            int x2 = _colWidth * (dispCol + 1) + xLeft;
            int x1 = x2 - _colWidth / 2 - 1;
            int y1 = TrueScaleHeight + _rowHeight * dispRow + yTop;
            int y2 = y1 + _rowHeight - 1;

            /*
            gc.FillRectangle(new SolidBrush(token.Color), x1, y1, _colWidth / 2 + 1, _rowHeight);
            gc.DrawLine(Pens.Black, x1, y1, x2, y1);
            gc.DrawLine(Pens.Black, x1, y2, x2, y2);
            if (edge)
                gc.DrawLine(Pens.Black, x2, y1, x2, y2);
             */
            Color def = token.Color, lite = ControlPaint.LightLight(def);
            Color dark = ControlPaint.Dark(def, 0.01f);
            Color main = token.IsHighlighted ? def : lite;
            Brush fillBrush = token.IsHighlighted ? (Brush)(new SolidBrush(main)) :
                new LinearGradientBrush(new Point(0, y1), new Point(0, y2), lite, dark);
            Pen border = new Pen(token.IsHighlighted ? lite : def, token.IsHighlighted ? 2f : 1f);
            if (edge)
            {
                gc.FillRectangle(fillBrush, x1, y1, _colWidth / 2 - 2, _rowHeight);
                gc.FillRectangle(fillBrush, x2 - 3, y1 + 1, 2, _rowHeight - 2);
                gc.FillRectangle(fillBrush, x2 - 1, y1 + 2, 1, _rowHeight - 4);
                gc.FillRectangle(fillBrush, x2, y1 + 4, 1, _rowHeight - 8);
                gc.DrawLine(border, x1, y1, x2 - 4, y1);
                gc.DrawLine(border, x2 - 2, y1 + 1, x2 - 3, y1 + 1);
                gc.DrawLine(border, x2 - 1, y1 + 2, x2 - 1, y1 + 3);
                gc.DrawLine(border, x2, y1 + 4, x2, y2 - 4);
                gc.DrawLine(border, x2 - 1, y2 - 2, x2 - 1, y2 - 3);
                gc.DrawLine(border, x2 - 2, y2 - 1, x2 - 3, y2 - 1);
                gc.DrawLine(border, x1, y2, x2 - 4, y2);
            }
            else
            {
                gc.FillRectangle(fillBrush, x1, y1, _colWidth / 2 + 1, _rowHeight);
                gc.DrawLine(border, x1, y1, x2, y1);
                gc.DrawLine(border, x1, y2, x2, y2);
            }
        }

        private void drawTokenText(Graphics gc, EventToken token, TokenTextItem item)
        {
            int dispRow = item.Row;
            int dispCol = item.Col;
            int dispWidth = item.Width;
            bool continued = item.Continued;

            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            bool rowHangOver = (TopRow > 0 && TopRow + DisplayedRowCount > RowCount);
            int xLeft = colHangOver ? (ViewRectangle.Width - DisplayedColumnCount * _colWidth) : 0;
            int yTop = rowHangOver ? (ViewRectangle.Height - TrueScaleHeight - DisplayedRowCount * _rowHeight) : 0;

            int x1 = _colWidth * dispCol + xLeft;
            int y1 = TrueScaleHeight + _rowHeight * dispRow + yTop;
            if (x1 < 0)
                x1 = 0;

            string text = (continued ? "<< " : "") + token.Name;
            gc.DrawString(text, (continued ? _italic : _bold),
                token.IsHighlighted ? _fontHighlight : _fontColor,
                new RectangleF(x1 + 2, y1 + 2, dispWidth * _colWidth - 4, _rowHeight - 4),
                new StringFormat() { FormatFlags = StringFormatFlags.NoWrap });
        }

        private void resizeDrawingBuffer()
        {
            _drawingBuffer = new Bitmap(_view.DisplayRectangle.Width, _view.DisplayRectangle.Height);
        }
        #endregion

        #region Accessors
        private EventToken[] GetInstantTokens(int index)
        {
            if (index < 0)
                return new EventToken[0];

            return _tokenHistory[(ulong)index];
        }

        private bool getVisibleCell(Point loc, out int col, out int row)
        {
            bool colHangOver = (LeftColumn > 0 && LeftColumn + DisplayedColumnCount > ColumnCount);
            bool rowHangOver = (TopRow > 0 && TopRow + DisplayedRowCount > RowCount);
            int xLeft = colHangOver ? (ViewRectangle.Width - DisplayedColumnCount * _colWidth) : 0;
            int yTop = rowHangOver ? (ViewRectangle.Height - TrueScaleHeight - DisplayedRowCount * _rowHeight) : 0;
            int colOffset = colHangOver ? -1 : 0;
            int rowOffset = rowHangOver ? -1 : 0;

            col = LeftColumn + colOffset + (loc.X - xLeft) / ColumnWidth;
            row = TopRow + rowOffset + (loc.Y - TrueScaleHeight - yTop) / RowHeight;

            return (loc.Y >= TrueScaleHeight);
        }
        #endregion

        #region Operations
        private void updateHistory(ulong timeCode)
        {
            _tokenHistory.UpdateFuture(_sched, timeCode);
            updateScrollBars();
        }

        private void invalidateView()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => { invalidateView(); }));
            else
                _view.Invalidate();
        }
        #endregion

        #region Flags
        private bool indexIsVisible(int index)
        {
            return (index >= LeftColumn && index - LeftColumn < FullDisplayedColumnCount);
        }
        #endregion
        #endregion
    }
}
