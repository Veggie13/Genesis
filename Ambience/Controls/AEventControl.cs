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
    [TypeDescriptionProvider(typeof(Helper.ConcreteClassProvider<UserControl>))]
    [Helper.ConcreteClass(typeof(AEventControl.Concrete))]
    public abstract partial class AEventControl : UserControl
    {
        internal class Concrete : AEventControl
        {
            public Concrete() : base(null) { }

            public override IEventProvider Provider
            {
                get { throw new NotImplementedException(); }
            }

            public override void ApplyChanges()
            {
                throw new NotImplementedException();
            }

            protected override void onInit()
            {
                throw new NotImplementedException();
            }
        }

        public AEventControl(IEventColorProvider colorer)
        {
            InitializeComponent();

            ColorProvider = colorer;
        }

        protected IEventColorProvider ColorProvider
        {
            get;
            set;
        }

        public abstract IEventProvider Provider { get; }

        public abstract void ApplyChanges();

        public void UndoChanges()
        {
            initFromProvider();
        }

        public event Action Modified;
        protected void emitModified()
        {
            if (Modified != null)
                Modified();
        }

        protected bool Initializing
        {
            get;
            private set;
        }

        protected void initFromProvider()
        {
            Initializing = true;
            onInit();
            Initializing = false;
        }

        protected abstract void onInit();
    }
}
