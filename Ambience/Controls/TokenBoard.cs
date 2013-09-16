using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Common.Tools;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Controls
{
    public partial class TokenBoard : UserControl
    {
        #region Class Members
        private ProviderTokenButton[,] _buttons = new ProviderTokenButton[1, 1];
        private Size _itemSize = new Size();
        #endregion

        public interface ITokenBoardProvider
        {
            event Action<int, int> CellChanged;

            IEventProvider this[int row, int col] { get; set; }
            int RowCount { get; set; }
            int ColumnCount { get; set; }
        }

        public TokenBoard()
        {
            InitializeComponent();

            initBoard();

            ColumnWidth = DefaultColumnWidth;
            RowHeight = DefaultRowHeight;
        }

        #region Events
        public event ProviderTokenButton.Event ButtonRightClicked = (b) => { };
        public event ProviderTokenButton.TileClickedEvent TileClicked = (t) => { };
        #endregion

        #region Properties
        #region RowCount
        private const int DefaultRowCount = 1;
        [DefaultValue(DefaultRowCount)]
        public int RowCount
        {
            get { return _content.RowCount; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("RowCount");

                int oldCount = _content.RowCount;
                _content.RowCount = value;
                _buttons = _buttons.Resize(RowCount, ColumnCount);
                while (_content.RowStyles.Count > RowCount)
                {
                    _content.RowStyles.RemoveAt(value);
                }
                _content.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
                for (int row = oldCount; row < value; row++)
                {
                    _content.RowStyles.Add(new RowStyle(_content.RowStyles[0].SizeType, _content.RowStyles[0].Height));
                    for (int col = 0; col < ColumnCount; col++)
                    {
                        addButton(row, col);
                    }
                }
                _content.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

                if (_tokenBoardProvider != null)
                    _tokenBoardProvider.RowCount = value;

                PerformLayout();
            }
        }
        #endregion

        #region ColumnCount
        private const int DefaultColumnCount = 1;
        [DefaultValue(DefaultColumnCount)]
        public int ColumnCount
        {
            get { return _content.ColumnCount; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("ColumnCount");

                int oldCount = _content.ColumnCount;
                _content.ColumnCount = value;
                _buttons = _buttons.Resize(RowCount, ColumnCount);
                while (_content.ColumnStyles.Count > ColumnCount)
                {
                    _content.ColumnStyles.RemoveAt(value);
                }
                _content.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
                for (int col = oldCount; col < value; col++)
                {
                    _content.ColumnStyles.Add(new ColumnStyle(_content.ColumnStyles[0].SizeType, _content.ColumnStyles[0].Width));
                    for (int row = 0; row < RowCount; row++)
                    {
                        addButton(row, col);
                    }
                }
                _content.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

                if (_tokenBoardProvider != null)
                    _tokenBoardProvider.ColumnCount = value;

                PerformLayout();
            }
        }
        #endregion

        #region ColumnWidth
        private const int DefaultColumnWidth = 70;
        [DefaultValue(DefaultColumnWidth)]
        public int ColumnWidth
        {
            get { return _itemSize.Width + _content.Padding.Left + _content.Padding.Right; }
            set
            {
                _itemSize.Width = value - _content.Padding.Left - _content.Padding.Right;
                setButtonSizes();
            }
        }
        #endregion

        #region RowHeight
        private const int DefaultRowHeight = 50;
        [DefaultValue(DefaultRowHeight)]
        public int RowHeight
        {
            get { return _itemSize.Height + _content.Padding.Top + _content.Padding.Bottom; }
            set
            {
                _itemSize.Height = value - _content.Padding.Top - _content.Padding.Bottom;
                setButtonSizes();
            }
        }
        #endregion

        #region MinimumSize
        public override Size MinimumSize
        {
            get
            {
                Size size = new Size();
                size.Width = Math.Max(ColumnCount * ColumnWidth, base.MinimumSize.Width);
                size.Height = Math.Max(RowCount * RowHeight, base.MinimumSize.Height);
                return size;
            }
            set
            {
                base.MinimumSize = value;
            }
        }
        #endregion

        #region Indexer
        public ProviderTokenButton this[int row, int col]
        {
            get
            {
                return _buttons[row, col];
            }
        }
        #endregion

        #region TokenBoardProvider
        private ITokenBoardProvider _tokenBoardProvider = null;
        [Browsable(false)]
        public ITokenBoardProvider TokenBoardProvider
        {
            get { return _tokenBoardProvider; }
            set
            {
                SuspendLayout();
                if (_tokenBoardProvider != null)
                {
                    _tokenBoardProvider.CellChanged -= _tokenBoardProvider_CellChanged;
                    _tokenBoardProvider = null;
                }
                RowCount = 1;
                ColumnCount = 1;
                _content.Controls.Clear();

                _tokenBoardProvider = value;
                if (_tokenBoardProvider != null)
                {
                    _tokenBoardProvider.CellChanged += new Action<int, int>(_tokenBoardProvider_CellChanged);
                }
                initBoard();
                ResumeLayout(true);
            }
        }
        #endregion
        #endregion

        #region Event Handlers
        private void btn_RightClicked(ProviderTokenButton button)
        {
            ButtonRightClicked(button);
        }

        private void btn_TokenChanged(ProviderTokenButton button)
        {
            if (_tokenBoardProvider != null)
            {
                var index = _buttons.IndexOf(button);
                _tokenBoardProvider[index.Item1, index.Item2] = button.Token.Provider;
            }
        }

        private void btn_TileClicked(ProviderToken token)
        {
            TileClicked(token);
        }

        private void _tokenBoardProvider_CellChanged(int row, int col)
        {
            _buttons[row, col].Token = new ProviderToken(_tokenBoardProvider[row, col], null);
        }
        #endregion

        #region Private Helpers
        private void addButton(int row, int col)
        {
            var btn = new ProviderTokenButton();
            btn.MinimumSize = _itemSize;
            btn.MaximumSize = _itemSize;
            btn.Margin = new System.Windows.Forms.Padding(0);
            _buttons[row, col] = btn;
            _content.Controls.Add(btn, col, row);

            btn.RightClicked += new ProviderTokenButton.Event(btn_RightClicked);
            btn.TokenChanged += new ProviderTokenButton.Event(btn_TokenChanged);
            btn.TileClicked += new ProviderTokenButton.TileClickedEvent(btn_TileClicked);
        }

        private void initBoard()
        {
            addButton(0, 0);

            SuspendLayout();
            if (_tokenBoardProvider != null)
            {
                ColumnCount = _tokenBoardProvider.ColumnCount;
                RowCount = _tokenBoardProvider.RowCount;

                for (int row = 0; row < RowCount; row++)
                    for (int col = 0; col < ColumnCount; col++)
                    {
                        var prov = _tokenBoardProvider[row, col];
                        if (prov != null)
                        {
                            _buttons[row, col].Token = new ProviderToken(prov, null);
                        }
                    }
            }
            else
            {
                ColumnCount = DefaultColumnCount;
                RowCount = DefaultRowCount;
            }
            ResumeLayout(true);
        }

        private void setButtonSizes()
        {
            SuspendLayout();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    _buttons[row, col].MinimumSize = _itemSize;
                    _buttons[row, col].MaximumSize = _itemSize;
                }
            ResumeLayout(true);
        }
        #endregion
    }
}
