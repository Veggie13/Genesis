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
                throw new NotImplementedException();
            }

            public void Visit(SimultaneousEventProvider provider)
            {
                throw new NotImplementedException();
            }

            public void Visit(IVisitable<IEventProviderVisitor, IEventProvider> item)
            {
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
