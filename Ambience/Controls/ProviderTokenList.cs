using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;
using System.IO;
using Genesis.Common.Tools;
using System.Diagnostics;

namespace Genesis.Ambience.Controls
{
    public partial class ProviderTokenList : UserControl
    {
        public enum Orientation { Horizontal, Vertical }

        #region Class Members
        //private Dictionary<IEventProvider, ProviderTokenTile> _tiles =
        //    new Dictionary<IEventProvider, ProviderTokenTile>();
        private int _offset = 0;
        private int _basePos;
        private Point _lastPoint;
        private ProviderTokenTile _empty = new ProviderTokenTile();
        private bool _scrolling = false;
        private bool _internalDrag = false;
        #endregion

        #region Constructor
        public ProviderTokenList()
        {
            InitializeComponent();

            //_items.ListChanged += new ListChangedEventHandler(_items_ListChanged);
            _items.ItemsAdded += new SignalList<IEventProvider>.ItemsEvent(_items_ItemsAdded);
            _items.ItemsRemoved += new SignalList<IEventProvider>.ItemsEvent(_items_ItemsRemoved);

            _scrollLeft.ValueChanged += new EventHandler(scroll_ValueChanged);
            _scrollRight.ValueChanged += new EventHandler(scroll_ValueChanged);
            _scrollBottom.ValueChanged += new EventHandler(scroll_ValueChanged);

            _view.SizeChanged += new EventHandler(_view_SizeChanged);
            _flow.SizeChanged += new EventHandler(_flow_SizeChanged);

            _empty.AboutToDrop += new ProviderTokenTileDragEventHandler(_empty_AboutToDrop);

            this.Load += new EventHandler(ProviderTokenList_Load);
        }
        #endregion

        #region Properties
        #region Items
        private SignalList<IEventProvider> _items = new SignalList<IEventProvider>();
        [Browsable(false)]
        public ICollection<IEventProvider> Items
        {
            get { return _items; }
        }
        #endregion

        #region ItemHeight
        private const int DefaultItemHeight = 30;
        private int _itemHeight = DefaultItemHeight;
        public int ItemHeight
        {
            get
            {
                if (_orientation == Orientation.Horizontal)
                    return _view.DisplayRectangle.Height;
                else
                    return _itemHeight;
            }
            set
            {
                _itemHeight = value;
                if (_orientation != Orientation.Horizontal)
                {
                    foreach (Control tile in _flow.Controls)
                    {
                        tile.Height = _itemHeight;
                    }
                    _empty.Height = _itemHeight;
                }
            }
        }
        private bool ShouldSerializeItemHeight()
        {
            return (_orientation == Orientation.Horizontal) ? false :
                (_itemHeight != DefaultItemHeight);
        }
        #endregion

        #region ItemWidth
        private const int DefaultItemWidth = 50;
        private int _itemWidth = DefaultItemWidth;
        public int ItemWidth
        {
            get
            {
                if (_orientation == Orientation.Vertical)
                    return _view.DisplayRectangle.Width;
                else
                    return _itemWidth;
            }
            set
            {
                _itemWidth = value;
                if (_orientation != Orientation.Vertical)
                {
                    foreach (Control tile in _flow.Controls)
                    {
                        tile.Width = _itemWidth;
                    }
                    _empty.Width = _itemWidth;
                }
            }
        }
        private bool ShouldSerializeItemWidth()
        {
            return (_orientation == Orientation.Vertical) ? false :
                (_itemWidth != DefaultItemWidth);
        }
        #endregion

        #region ViewOrientation
        private Orientation _orientation = Orientation.Vertical;
        [DefaultValue(typeof(Orientation), "Vertical")]
        public Orientation ViewOrientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                adjustControls();
            }
        }
        #endregion

        #region UseLeftScrollBar
        private bool _useScrollLeft = false;
        [DefaultValue(false)]
        public bool UseLeftScrollBar
        {
            get { return _useScrollLeft; }
            set
            {
                _useScrollLeft = value;
                adjustControls();
            }
        }
        #endregion

        #region UseHoverScroll
        private bool _hoverScroll = false;
        [DefaultValue(false)]
        public bool UseHoverScroll
        {
            get { return _hoverScroll; }
            set
            {
                _hoverScroll = value;
                adjustControls();
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
                this.Invalidate(true);
            }
        }
        #endregion

        #region Private
        private ScrollBar Scroller
        {
            get
            {
                if (_orientation == Orientation.Horizontal)
                    return _scrollBottom;
                else if (_useScrollLeft)
                    return _scrollLeft;
                else
                    return _scrollRight;
            }
        }

        private int ScrollPosition
        {
            get
            {
                int pos;
                if (_orientation == Orientation.Horizontal)
                    pos = _flow.Left;
                else
                    pos = _flow.Top;

                if (pos < Scroller.Minimum)
                    return Scroller.Minimum;
                if (pos > Scroller.Maximum)
                    return Scroller.Maximum;
                return pos;
            }
            set
            {
                if (_orientation == Orientation.Horizontal)
                    _flow.Left = value;
                else
                    _flow.Top = value;
            }
        }

        private int DisplayedLength
        {
            get
            {
                return (_orientation == Orientation.Horizontal) ?
                    _view.DisplayRectangle.Width :
                    _view.DisplayRectangle.Height;
            }
        }

        private int TotalLength
        {
            get
            {
                int itemCount = _items.Count;
                if (_flow.Controls.Contains(_empty))
                    itemCount++;
                return ((_orientation == Orientation.Horizontal) ?
                    ItemWidth : ItemHeight) * itemCount;
            }
        }
        #endregion
        #endregion

        #region Event Handlers
        private void ProviderTokenList_Load(object sender, EventArgs e)
        {
            adjustControls();
        }

        private void scroll_ValueChanged(object sender, EventArgs e)
        {
            ScrollPosition = -Scroller.Value;
        }

        private void _items_ItemsAdded(IEnumerable<Tuple<int, IEventProvider>> items)
        {
            if (items.Count() > 0 && _flow.Controls.Count == 1 && _flow.Controls.Contains(_empty))
                _flow.Controls.Remove(_empty);

            foreach (var item in items)
            {
                var fresh = item.Item2;
                var tile = new ProviderTokenTile();
                tile.Token = new ProviderToken(fresh, _colorer);
                tile.Width = ItemWidth;
                tile.Height = ItemHeight;
                tile.Margin = new System.Windows.Forms.Padding(0);
                //tile.Anchor = _flow.Anchor;
                _flow.Controls.Add(tile);
                _flow.Controls.SetChildIndex(tile, item.Item1);
                //_tiles[fresh] = tile;

                tile.DragEnter += new DragEventHandler(_view_DragEnter);
                tile.MouseMove += new MouseEventHandler(_view_MouseMove);
                tile.MouseEnter += new EventHandler(_view_MouseEnter);
                tile.MouseLeave += new EventHandler(_view_MouseLeave);
                tile.AboutToDrag += new ProviderTokenTileDragEventHandler(tile_AboutToDrag);
            }
        }

        private void _items_ItemsRemoved(IEnumerable<Tuple<int, IEventProvider>> items)
        {
            foreach (var doomed in items)
            {
                var tile = (ProviderTokenTile)_flow.Controls[doomed.Item1];
                tile.MouseMove -= _view_MouseMove;
                tile.MouseEnter -= _view_MouseEnter;
                tile.MouseLeave -= _view_MouseLeave;
                tile.AboutToDrag -= tile_AboutToDrag;
                _flow.Controls.RemoveAt(doomed.Item1);
            }
            if (_flow.Controls.Count < 1)
                _flow.Controls.Add(_empty);
        }

        private void tile_AboutToDrag(ProviderTokenTile sender, ProviderTokenTileDragEventArgs e)
        {
            sender.QueryContinueDrag += new QueryContinueDragEventHandler(tile_QueryContinueDrag);
            _internalDrag = true;
            int index = _flow.Controls.IndexOf(sender);
            _items.RemoveAt(index);
            _flow.Controls.Add(_empty);
            _flow.Controls.SetChildIndex(_empty, index);
            _flow.Controls.Remove(sender);
        }

        private void tile_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            ProviderTokenTile tile = sender as ProviderTokenTile;
            if (tile == null)
                return;

            Point pt = this.PointToClient(Control.MousePosition);
            if (!this.ClientRectangle.Contains(pt))
            {
                if (_flow.Controls.Count > 1)
                    _flow.Controls.Remove(_empty);
                scrollView(_lastPoint);
                if (!_internalDrag)
                {
                    tile.QueryContinueDrag -= tile_QueryContinueDrag;
                }
            }
            else
            {
                _lastPoint = pt;
                scrollView(pt);
            }
        }

        private void _view_DragEnter(object sender, DragEventArgs e)
        {
            ProviderTokenTile tile = sender as ProviderTokenTile;
            if (_scrolling || tile == null || tile == _empty || !e.IsDataPresent<ProviderTokenTileDragEventArgs>())
                return;

            if (!_internalDrag)
            {
                var args = e.GetData<ProviderTokenTileDragEventArgs>();
                args.TokenTile.QueryContinueDrag += new QueryContinueDragEventHandler(tile_QueryContinueDrag);
            }

            int index = _flow.Controls.IndexOf(tile);
            _flow.Controls.Add(_empty);
            _flow.Controls.SetChildIndex(_empty, index);
        }

        private void _view_MouseMove(object sender, MouseEventArgs e)
        {
            Control src = sender as Control;
            Point pt = _view.PointToClient(src.PointToScreen(e.Location));

            if (_flow.Controls.Count > 1 && _flow.Controls.Contains(_empty))
                _flow.Controls.Remove(_empty);
            
            scrollView(pt);
        }

        private void _view_MouseLeave(object sender, EventArgs e)
        {
            Control src = sender as Control;
            Point pt = this.PointToClient(Cursor.Position);
            _lastPoint = pt;
        }

        private void _view_MouseEnter(object sender, EventArgs e)
        {
            Control src = sender as Control;
            Point pt = this.PointToClient(Cursor.Position);
            if (pt.Equals(_lastPoint))
                return;

            int pos, dispLen = DisplayedLength;
            if (_orientation == Orientation.Horizontal)
            {
                pos = Math.Max(pt.X, ItemWidth);
                pos = Math.Min(pos, DisplayedLength - ItemWidth);
                pos -= ItemWidth;
                dispLen -= 2 * ItemWidth;
            }
            else
            {
                pos = Math.Max(pt.Y, ItemHeight);
                pos = Math.Min(pos, DisplayedLength - ItemHeight);
                pos -= ItemHeight;
                dispLen -= 2 * ItemHeight;
            }

            double ratio = (double)dispLen / (DisplayedLength - TotalLength);
            int baseOffset = (int)(ScrollPosition * ratio);
            _offset = pos - baseOffset;
            _basePos = baseOffset;
        }

        private void _view_SizeChanged(object sender, EventArgs e)
        {
            _flow.Location = new Point(0, 0);
            if (ViewOrientation == Orientation.Horizontal)
                _flow.Height = _view.DisplayRectangle.Height;
            else
                _flow.Width = _view.DisplayRectangle.Width;
        }

        private void _empty_AboutToDrop(ProviderTokenTile sender, ProviderTokenTileDragEventArgs e)
        {
            int index = _flow.Controls.IndexOf(_empty);
            _flow.Controls.Remove(_empty);
            if (index >= 0)
                _items.Insert(index, e.TokenTile.Token.Provider);
            e.Handled = true;
            _internalDrag = false;
        }

        private void _flow_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control tile in _flow.Controls)
                tile.Size = new Size(ItemWidth, ItemHeight);
        }
        #endregion

        #region Private Helpers
        private void adjustControls()
        {
            _scrollLeft.Visible = !_hoverScroll && (_orientation == Orientation.Vertical) && _useScrollLeft;
            _scrollRight.Visible = !_hoverScroll && (_orientation == Orientation.Vertical) && !_useScrollLeft;
            _scrollBottom.Visible = !_hoverScroll && (_orientation == Orientation.Horizontal);
            
            if (_orientation == Orientation.Horizontal)
            {
                _flow.FlowDirection = FlowDirection.LeftToRight;
                _flow.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;// | AnchorStyles.Left;
            }
            else
            {
                _flow.FlowDirection = FlowDirection.TopDown;
                _flow.Anchor = AnchorStyles.Left | AnchorStyles.Right;// | AnchorStyles.Top;
            }
            
            Scroller.Minimum = 0;
            Scroller.Maximum = TotalLength;
            Scroller.LargeChange = DisplayedLength;
            Scroller.Value = ScrollPosition;

            _empty.Height = ItemHeight;
            _empty.Width = ItemWidth;
        }

        private void scrollView(Point pt)
        {
            if (!_hoverScroll || DisplayedLength > TotalLength)
                return;

            _scrolling = true;
            int p, dispLen = DisplayedLength, totLen = TotalLength;
            if (_orientation == Orientation.Horizontal)
            {
                p = Math.Max(pt.X, ItemWidth);
                p = Math.Min(p, DisplayedLength - ItemWidth);
                p -= ItemWidth;
                dispLen -= 2 * ItemWidth;
                totLen -= 2 * ItemWidth;
            }
            else
            {
                p = Math.Max(pt.Y, ItemHeight);
                p = Math.Min(p, DisplayedLength - ItemHeight);
                p -= ItemHeight;
                dispLen -= 2 * ItemHeight;
                totLen -= 2 * ItemHeight;
            }
            if (dispLen != 0)
            {
                double ratio = 1 - (totLen / (double)dispLen);
                int pos;
                int newOffset = p - _basePos;
                if (newOffset * _offset < 0)
                {
                    newOffset = 0;
                }
                else if (newOffset != 0)
                {
                    if (Math.Abs(newOffset) <= Math.Abs(_offset))
                    {
                        p = _basePos;
                    }
                    else if (newOffset > _offset)
                    {
                        int total = dispLen - _basePos;
                        int subtotal = total - _offset;
                        double subratio = (p - (_basePos + _offset)) / (double)subtotal;
                        int newPos = _basePos + (int)(subratio * total);
                        newOffset = p - newPos;
                        p = newPos;
                    }
                    else
                    {
                        int total = _basePos;
                        int subtotal = total + _offset;
                        double subratio = ((_basePos + _offset) - p) / (double)subtotal;
                        int newPos = _basePos - (int)(subratio * total);
                        newOffset = p - newPos;
                        p = newPos;
                    }
                }

                _basePos = p;
                _offset = newOffset;
                pos = (int)(ratio * p);

                ScrollPosition = pos;
            }
            _scrolling = false;
        }
        #endregion
    }
}
