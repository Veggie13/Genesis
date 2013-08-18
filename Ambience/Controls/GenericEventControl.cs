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
            initFromProvider();
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

    }
}
