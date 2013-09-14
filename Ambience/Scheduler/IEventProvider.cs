using System;
using System.Collections.Generic;
using System.Text;
using Genesis.Common.Tools;

namespace Genesis.Ambience.Scheduler
{
    public interface IEventProvider : IVisitable<IEventProviderVisitor, IEventProvider>
    {
        string Name { get; set; }

        bool DependsOn(IEventProvider dependent);
        IEventProviderInstance CreateInstance();
        IEventProviderInstance CreateInstance(IEventProvider src);
    }

    public interface IEventProviderVisitor : IVisitor<IEventProviderVisitor, IEventProvider>
    {
        void Visit(DelayEventProvider provider);
        void Visit(PeriodicEventProvider provider);
        void Visit(RandomEventSelector provider);
        void Visit(SequentialEventSelector provider);
        void Visit(SimultaneousEventProvider provider);
        void Visit(SoundEvent.Provider provider);
    }

    public abstract class AEventProvider : IEventProvider
    {
        public AEventProvider(string name)
        {
            Name = name;
        }

        #region IEventProvider Members
        public string Name { get; set; }

        public abstract bool DependsOn(IEventProvider dependent);
        public abstract IEventProviderInstance CreateInstance();
        public abstract IEventProviderInstance CreateInstance(IEventProvider src);

        #region IVisitable
        public abstract void Accept(IEventProviderVisitor visitor);
        #endregion
        #endregion
    }

    public interface IEventProviderInstance
    {
        bool Next(IEventScheduler sched, ulong currTimeCode, ulong span);
        IEventProvider Model { get; }
        IEventProvider Source { get; }
    }

    public abstract class AEventProviderInstance<T> : IEventProviderInstance where T : IEventProvider
    {
        public AEventProviderInstance(T parent, IEventProvider src)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            ParentModel = parent;
            _src = src;
        }

        public abstract bool Next(IEventScheduler sched, ulong currTimeCode, ulong span);

        protected T ParentModel
        {
            get;
            private set;
        }

        public IEventProvider Model
        {
            get { return ParentModel; }
        }

        private IEventProvider _src = null;
        public virtual IEventProvider Source
        {
            get { return (_src == null) ? Model : _src; }
        }

    }

}
