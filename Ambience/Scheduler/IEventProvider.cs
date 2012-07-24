using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public interface IEventProvider
    {
        string Name { get; }

        bool DependsOn(IEventProvider dependent);
        IEventProviderInstance CreateInstance();
        IEventProviderInstance CreateInstance(IEventProvider src);
    }

    public abstract class AEventProvider : IEventProvider
    {
        public AEventProvider(string name)
        {
            _name = name;
        }

        #region IEventProvider Members
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public abstract bool DependsOn(IEventProvider dependent);
        public abstract IEventProviderInstance CreateInstance();
        public abstract IEventProviderInstance CreateInstance(IEventProvider src);
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
        public AEventProviderInstance(T parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            _parent = parent;
        }

        public AEventProviderInstance(T parent, IEventProvider src)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            _parent = parent;
            _src = src;
        }

        public abstract bool Next(IEventScheduler sched, ulong currTimeCode, ulong span);
        
        protected T _parent;
        public IEventProvider Model
        {
            get { return _parent; }
        }

        private IEventProvider _src = null;
        public virtual IEventProvider Source
        {
            get { return (_src == null) ? Model : _src; }
        }

    }

}
