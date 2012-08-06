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

namespace Genesis.Ambience.Controls
{
    public partial class ProviderTokenList : UserControl
    {
        public enum Orientation { Horizontal, Vertical }

        #region Class Members
        private Dictionary<IEventProvider, ProviderTokenTile> _tiles =
            new Dictionary<IEventProvider, ProviderTokenTile>();
        private int _offset = 0;
        private int _basePos;
        private Point _lastPoint;
        #endregion

        #region Constructor
        public ProviderTokenList()
        {
            InitializeComponent();

            _items.ListChanged += new ListChangedEventHandler(_items_ListChanged);

            _scrollLeft.ValueChanged += new EventHandler(scroll_ValueChanged);
            _scrollRight.ValueChanged += new EventHandler(scroll_ValueChanged);
            _scrollBottom.ValueChanged += new EventHandler(scroll_ValueChanged);

            _flow.SizeChanged += new EventHandler(_flow_SizeChanged);

            this.Load += new EventHandler(ProviderTokenList_Load);
        }
        #endregion

        #region Properties
        #region Items
        private BindingList<IEventProvider> _items = new BindingList<IEventProvider>();
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
                    foreach (var tile in _tiles.Values)
                    {
                        tile.Height = _itemHeight;
                    }
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
                    foreach (var tile in _tiles.Values)
                    {
                        tile.Width = _itemWidth;
                    }
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
                if (_orientation == Orientation.Horizontal)
                    return _flow.Left;
                else
                    return _flow.Top;
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
                return ((_orientation == Orientation.Horizontal) ?
                    ItemWidth : ItemHeight) * _items.Count;
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

        private void _items_ListChanged(object sender, ListChangedEventArgs e)
        {
            var removed = _tiles
                .Where((p) => (!_items.Contains(p.Key)))
                .Select((p) => (p.Key)).ToList();
            var added = _items
                .Where((i) => (!_tiles.ContainsKey(i))).ToList();

            foreach (var doomed in removed)
            {
                _tiles[doomed].MouseMove -= _view_MouseMove;
                _flow.Controls.Remove(_tiles[doomed]);
                _tiles.Remove(doomed);
            }

            foreach (var fresh in added)
            {
                var tile = new ProviderTokenTile();
                tile.Token = new ProviderToken(fresh, _colorer);
                tile.Width = ItemWidth;
                tile.Height = ItemHeight;
                tile.Margin = new System.Windows.Forms.Padding(0);
                //tile.Anchor = _flow.Anchor;
                _flow.Controls.Add(tile);
                _tiles[fresh] = tile;

                tile.MouseMove += new MouseEventHandler(_view_MouseMove);
                tile.MouseEnter += new EventHandler(_view_MouseEnter);
                tile.MouseLeave += new EventHandler(_view_MouseLeave);
            }

            for (int i = 0; i < _items.Count; i++)
            {
                var tile = _tiles[_items[i]];
                _flow.Controls.SetChildIndex(tile, i);
            }
        }

        private void _view_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_hoverScroll)
                return;

            Control src = sender as Control;
            Point pt = _view.PointToClient(src.PointToScreen(e.Location));

            if (DisplayedLength != 0)
            {
                double ratio = 1 - (TotalLength / (double)DisplayedLength);
                int pos;
                if (_orientation == Orientation.Horizontal)
                {
                    int p = pt.X;
                    int newOffset = pt.X - _basePos;
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
                            int total = DisplayedLength - _basePos;
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
                }
                else
                {
                    pos = (int)(ratio * pt.Y);
                }
                
                ScrollPosition = pos;
            }
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

            int pos;
            if (_orientation == Orientation.Horizontal)
                pos = pt.X;
            else
                pos = pt.Y;

            double ratio = (double)DisplayedLength / (DisplayedLength - TotalLength);
            int baseOffset = (int)(ScrollPosition * ratio);
            _offset = pos - baseOffset;
            _basePos = baseOffset;
        }

        private void _flow_SizeChanged(object sender, EventArgs e)
        {
            foreach (var tile in _tiles.Values)
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
        }
        #endregion
    }
}
