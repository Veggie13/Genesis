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
    public abstract partial class AEventControl : UserControl
    {
        #region Concrete
        protected class Concrete : AEventControl
        {
            public Concrete() : base(null) { }

            public override IEventProvider Provider
            {
                get { throw new NotImplementedException(); }
            }

            protected override void saveToProvider()
            {
                throw new NotImplementedException();
            }

            protected override void onInit()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        public AEventControl(IEventColorProvider colorer)
        {
            InitializeComponent();

            ColorProvider = colorer;

            this.ModifiedChanged += new Action(AEventControl_ModifiedChanged);
        }

        #region Events
        public event Action ModifiedChanged;
        private void emitModifiedChanged()
        {
            if (ModifiedChanged != null)
                ModifiedChanged();
        }
        #endregion

        #region Properties
        protected IEventColorProvider ColorProvider
        {
            get;
            set;
        }

        protected bool Initializing
        {
            get;
            private set;
        }

        [Browsable(false)]
        public abstract IEventProvider Provider { get; }

        private bool _modified = false;
        [Browsable(false)]
        public bool Modified
        {
            get { return _modified; }
            private set
            {
                if (value != _modified)
                {
                    _modified = value;
                    if (!Initializing)
                        emitModifiedChanged();
                }
            }
        }
        #endregion

        #region Public Operations
        public void ApplyChanges()
        {
            saveToProvider();
            Provider.Name = _txtName.Text;
            Modified = false;
        }

        public void UndoChanges()
        {
            initFromProvider();
        }
        #endregion

        #region Event Handlers
        private void AEventControl_ModifiedChanged()
        {
            _btnApply.Enabled = Modified;
            _btnUndo.Enabled = Modified;
        }

        private void _btnApply_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void _btnUndo_Click(object sender, EventArgs e)
        {
            UndoChanges();
        }

        private void _txtName_TextChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion

        #region Protected Helpers
        protected void setDirty()
        {
            Modified = true;
        }

        protected void initFromProvider()
        {
            Initializing = true;
            onInit();
            _txtName.Text = Provider.Name;
            Initializing = false;
            Modified = false;
        }

        protected abstract void saveToProvider();

        protected abstract void onInit();
        #endregion
    }
}
