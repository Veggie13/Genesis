using System.ComponentModel;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Controls
{
    public abstract class GenericEventControl<T> : AEventControl where T : IEventProvider
    {
        public GenericEventControl()
            : base(null)
        {
        }

        public GenericEventControl(T prov, IEventColorProvider colorer)
            : base(colorer)
        {
            EventProvider = prov;
            this.Load += new System.EventHandler(GenericEventControl_Load);
        }

        public override IEventProvider Provider
        {
            get { return EventProvider; }
        }

        protected T EventProvider
        {
            get;
            private set;
        }

        private void GenericEventControl_Load(object sender, System.EventArgs e)
        {
            initFromProvider();
        }

    }
}
