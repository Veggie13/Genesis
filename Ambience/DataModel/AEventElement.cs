using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Genesis.Ambience.Scheduler;
using System.Runtime.Serialization;
using Genesis.Common.Tools;

namespace Genesis.Ambience.DataModel
{
    public abstract class AEventElement : IVisitable<IEventElementVisitor, AEventElement>
    {
        public abstract IEventProvider Provider { get; }

        public string Name
        {
            get { return Provider.Name; }
        }

        private Color _color = Color.LightGray;
        public Color BaseColor
        {
            get { return _color; }
            set
            {
                _color = value;
                emitBaseColorChanged();
            }
        }
        public event Action BaseColorChanged;
        private void emitBaseColorChanged()
        {
            if (BaseColorChanged != null)
                BaseColorChanged();
        }

        public abstract void Accept(IEventElementVisitor visitor);
    }

    public abstract class AEventElement<T> : AEventElement where T : IEventProvider
    {
        public AEventElement(T provider)
        {
            _provider = provider;
        }

        private T _provider;
        public override IEventProvider Provider
        {
            get { return _provider; }
        }

        public T SpecificProvider
        {
            get { return _provider; }
        }
    }

    public interface IEventElementVisitor : IVisitor<IEventElementVisitor, AEventElement>
    {
        void Visit(DelayEventElement element);
        void Visit(PeriodicEventElement element);
        void Visit(RandomEventElement element);
        void Visit(SequentialEventElement element);
        void Visit(SimultaneousEventElement element);
        void Visit(SoundEventElement element);
    }
}
