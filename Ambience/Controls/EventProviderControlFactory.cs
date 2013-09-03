using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Scheduler;
using Genesis.Common.Tools;

namespace Genesis.Ambience.Controls
{
    public static class EventProviderControlFactory
    {
        private class ProviderVisitor : IEventProviderVisitor
        {
            private IEventColorProvider _colorProvider;

            public ProviderVisitor(IEventColorProvider colorer)
            {
                _colorProvider = colorer;
            }
            
            public AEventControl Control
            {
                get;
                private set;
            }

            public void Visit(DelayEventProvider provider)
            {
                Control = new DelayEventControl(provider, _colorProvider);
            }

            public void Visit(PeriodicEventProvider provider)
            {
                Control = new PeriodicEventControl(provider, _colorProvider);
            }

            public void Visit(RandomEventSelector provider)
            {
                Control = new RandomEventControl(provider, _colorProvider);
            }

            public void Visit(SequentialEventSelector provider)
            {
                Control = new SequentialEventControl(provider, _colorProvider);
            }

            public void Visit(SimultaneousEventProvider provider)
            {
                Control = new SimultaneousEventControl(provider, _colorProvider);
            }

            public void Visit(SoundEvent.Provider provider)
            {
                Control = new SoundEventControl(provider, _colorProvider);
            }

            public void Visit(IVisitable<IEventProviderVisitor, IEventProvider> item)
            {
                // If this happens, there's a new type of visitable that this visitor
                // hasn't been set up to visit yet...
                throw new NotImplementedException();
            }
        }

        public static AEventControl Create(IEventProvider provider, IEventColorProvider colorer)
        {
            ProviderVisitor visitor = new ProviderVisitor(colorer);
            visitor.TryVisit(provider);
            return visitor.Control;
        }
    }
}
