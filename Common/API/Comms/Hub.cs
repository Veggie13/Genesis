using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Remoting.Lifetime;

namespace Genesis.Common.API.Comms
{
    public delegate void RegisterApplicationEvent(uint appId);
    public delegate void UnregisterApplicationEvent(uint appId);

    public delegate void RegisterTriggerEvent(TriggerID id);
    public delegate void UnregisterTriggerEvent(TriggerID id);

    public delegate void ConnectObserverEvent(TriggerID id, IObserver obs);
    public delegate void DisconnectObserverEvent(TriggerID id, IObserver obs);

    public interface IBaseHub : IGenesisObject
    {
        event RegisterApplicationEvent ApplicationRegistered;
        event UnregisterApplicationEvent ApplicationUnregistered;

        IEnumerable<KeyValuePair<uint, string>> GetRegisteredApps();
        IEnumerable<uint> GetRegisteredAppIDs(string appName);
        string GetRegisteredAppName(uint id);

        IEnumerable<TriggerID> GetTriggers();
        IEnumerable<string> GetTriggerNames(uint appId);

        bool TriggerNameExists(string name);
        bool TriggerNameExists(string name, uint appId);

        bool RegisterTrigger(TriggerID id, ITrigger trig);
        event RegisterTriggerEvent TriggerRegistered;
        void UnregisterTrigger(TriggerID id);
        event UnregisterTriggerEvent TriggerUnregistered;

        bool ConnectObserver(TriggerID id, IObserver obs);
        event ConnectObserverEvent ObserverConnected;
        void DisconnectObserver(TriggerID id, IObserver obs);
        event DisconnectObserverEvent ObserverDisconnected;
    }

    public delegate void HubTaskDelegate(IBaseHub hub);
    public class HubTask : GenesisObject
    {
        private HubTaskDelegate _task;
        public HubTask(HubTaskDelegate task)
        {
            _task = task;
        }

        public void DoTask(IBaseHub hub)
        {
            if (_task != null)
                _task(hub);
        }
    }
    
    public interface IHub : IBaseHub
    {
        void DoTask(HubTask task);
    }

    public class Hub : GenesisObject, IHub, IDisposable
    {
        private object _locker = new object();
        private Dictionary<uint, IGenesisApplication> _apps = new Dictionary<uint, IGenesisApplication>();
        private Dictionary<TriggerID, ITrigger> _trigs = new Dictionary<TriggerID, ITrigger>();
        private Dictionary<TriggerID, IList<IObserver>> _obs = new Dictionary<TriggerID, IList<IObserver>>();
        private Timer _heart;

        public Hub()
        {
            _heart = new Timer(this.heartBeat, null, 2000, 500);

            AppRegHelper = new Clean.Helper<RegisterApplicationEvent>(() => (this.ApplicationRegistered), (a) => { this.ApplicationRegistered -= a; });
            AppUnregHelper = new Clean.Helper<UnregisterApplicationEvent>(() => (this.ApplicationUnregistered), (a) => { this.ApplicationUnregistered -= a; });
            TrigRegHelper = new Clean.Helper<RegisterTriggerEvent>(() => (this.TriggerRegistered), (a) => { this.TriggerRegistered -= a; });
            TrigUnregHelper = new Clean.Helper<UnregisterTriggerEvent>(() => (this.TriggerUnregistered), (a) => { this.TriggerUnregistered -= a; });
            ObsConnHelper = new Clean.Helper<ConnectObserverEvent>(() => (this.ObserverConnected), (a) => { this.ObserverConnected -= a; });
            ObsDisconnHelper = new Clean.Helper<DisconnectObserverEvent>(() => (this.ObserverDisconnected), (a) => { this.ObserverDisconnected -= a; });
            DeathHelper = new Clean.Helper<DeathBedEvent>(() => (this.AboutToDie), (a) => { this.AboutToDie -= a; });
        }

        public uint RegisterApplication(IGenesisApplication app)
        {
            uint next;
            lock (_locker)
            {
                next = (_apps.Count == 0) ? 1 : (_apps.Keys.Max() + 1);
                _apps[next] = app;
                app.AppID = next;
            }
            
            EmitApplicationRegistered(next);
            return next;
        }

        public void UnregisterApplication(uint id)
        {
            lock (_locker)
            {
                cleanupApp(id);
            }
            EmitApplicationUnregistered(id);
        }

        public IEnumerable<IGenesisApplication> Applications
        {
            get { return _apps.Values; }
        }

        public IGenesisApplication this[uint id]
        {
            get
            {
                if (_apps.ContainsKey(id))
                {
                    if (!_apps[id].IsConnected())
                    {
                        noisyCleanupApp(id);
                        return null;
                    }
                    return _apps[id];
                }
                else
                    return null;
            }
        }

        #region IHub Members
        #region IBaseHub

        public event RegisterApplicationEvent ApplicationRegistered;
        private Clean.Helper<RegisterApplicationEvent> AppRegHelper;
        private void EmitApplicationRegistered(uint id)
        {
            if (ApplicationRegistered != null)
                Clean.Emit(AppRegHelper, id);
        }

        public event UnregisterApplicationEvent ApplicationUnregistered;
        private Clean.Helper<UnregisterApplicationEvent> AppUnregHelper;
        private void EmitApplicationUnregistered(uint id)
        {
            if (ApplicationUnregistered != null)
                Clean.Emit(AppUnregHelper, id);
        }

        public IEnumerable<KeyValuePair<uint, string>> GetRegisteredApps()
        {
            List<uint> cleanup = new List<uint>();
            var valid = _apps.Where((pair) =>
            {
                if (!pair.Value.IsConnected())
                {
                    cleanup.Add(pair.Key);
                    return false;
                }
                else
                    return true;
            });
            foreach (uint id in cleanup)
                noisyCleanupApp(id);

            return valid.Select((pair) => new KeyValuePair<uint, string>(pair.Key, pair.Value.AppName));
        }

        public IEnumerable<uint> GetRegisteredAppIDs(string name)
        {
            List<uint> cleanup = new List<uint>();
            var valid = _apps.Where((pair) =>
            {
                if (!pair.Value.IsConnected())
                {
                    cleanup.Add(pair.Key);
                    return false;
                }
                else
                    return pair.Value.AppName.Equals(name);
            });
            foreach (uint id in cleanup)
                noisyCleanupApp(id);

            return valid.Select((pair) => pair.Key).ToList();
        }

        public string GetRegisteredAppName(uint id)
        {
            if (!_apps.ContainsKey(id))
                return null;
            if (!_apps[id].IsConnected())
            {
                noisyCleanupApp(id);
                return null;
            }
            return _apps[id].AppName;
        }

        public IEnumerable<TriggerID> GetTriggers()
        {
            return new List<TriggerID>(_trigs.Keys);
        }

        public IEnumerable<string> GetTriggerNames(uint appId)
        {
            return _trigs.Keys.Where((trigId) => (trigId.ApplicationID == appId)).Select<TriggerID, string>((trigID) => trigID.Name);
        }

        public bool TriggerNameExists(string name)
        {
            return _trigs.Keys.Any((trigId) => trigId.Name.Equals(name));
        }

        public bool TriggerNameExists(string name, uint appId)
        {
            return _trigs.Keys.Where((trigId) => (trigId.ApplicationID == appId)).Any((trigId) => trigId.Name.Equals(name));
        }

        public bool RegisterTrigger(TriggerID id, ITrigger trig)
        {
            if (_trigs.ContainsKey(id))
                return false;
            if (!_apps.ContainsKey(id.ApplicationID))
                return false;
            if (!_apps[id.ApplicationID].IsConnected())
            {
                noisyCleanupApp(id.ApplicationID);
                return false;
            }

            _trigs[id] = trig;
            _obs[id] = new List<IObserver>();

            EmitTriggerRegistered(id);
            return true;
        }

        public void UnregisterTrigger(TriggerID id)
        {
            if (!_trigs.ContainsKey(id))
                return;

            if (_trigs[id].IsConnected())
            {
                foreach (IObserver o in _obs[id])
                    _trigs[id].RemoveObserver(o);
            }
            _obs[id].Clear();

            _trigs.Remove(id);
            _obs.Remove(id);

            EmitTriggerUnregistered(id);
        }

        public event RegisterTriggerEvent TriggerRegistered;
        private Clean.Helper<RegisterTriggerEvent> TrigRegHelper;
        private void EmitTriggerRegistered(TriggerID id)
        {
            if (TriggerRegistered != null)
                Clean.Emit(TrigRegHelper, id);
        }

        public event UnregisterTriggerEvent TriggerUnregistered;
        private Clean.Helper<UnregisterTriggerEvent> TrigUnregHelper;
        private void EmitTriggerUnregistered(TriggerID id)
        {
            if (TriggerUnregistered != null)
                Clean.Emit(TrigUnregHelper, id);
        }

        public bool ConnectObserver(TriggerID id, IObserver obs)
        {
            if (!_trigs.ContainsKey(id))
                return false;
            if (!_trigs[id].IsConnected())
            {
                UnregisterTrigger(id);
                return false;
            }
            if (_obs[id].Contains(obs))
                return false;

            _trigs[id].AddObserver(obs);
            _obs[id].Add(obs);

            EmitObserverConnected(id, obs);
            return true;
        }

        public void DisconnectObserver(TriggerID id, IObserver obs)
        {
            if (!_trigs.ContainsKey(id))
                return;
            if (!_trigs[id].IsConnected())
            {
                UnregisterTrigger(id);
                return;
            }
            if (obs.IsConnected() && _obs[id].Contains(obs))
            {
                _trigs[id].RemoveObserver(obs);
                _obs[id].Remove(obs);
            }

            EmitObserverDisconnected(id, obs);
        }

        public event ConnectObserverEvent ObserverConnected;
        private Clean.Helper<ConnectObserverEvent> ObsConnHelper;
        private void EmitObserverConnected(TriggerID id, IObserver obs)
        {
            if (ObserverConnected != null)
                Clean.Emit(ObsConnHelper, id, obs);
        }

        public event DisconnectObserverEvent ObserverDisconnected;
        private Clean.Helper<DisconnectObserverEvent> ObsDisconnHelper;
        private void EmitObserverDisconnected(TriggerID id, IObserver obs)
        {
            if (ObserverDisconnected != null)
                Clean.Emit(ObsDisconnHelper, id, obs);
        }

        #endregion

        public void DoTask(HubTask task)
        {
            if (task != null)
            {
                lock (_locker)
                {
                    task.DoTask(this);
                }
            }
        }
        #endregion

        public delegate void DeathBedEvent();
        public event DeathBedEvent AboutToDie;
        private Clean.Helper<DeathBedEvent> DeathHelper;

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                _heart.Dispose();
                if (AboutToDie != null)
                    AboutToDie();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        private void cleanupApp(uint id)
        {
            foreach (TriggerID trigId in new List<TriggerID>(_trigs.Keys))
            {
                foreach (IObserver obs in new List<IObserver>(_obs[trigId].Where((obs) => (obs.IsConnected() && obs.ApplicationID == id))))
                    DisconnectObserver(trigId, obs);
            }
            foreach (TriggerID trigId in new List<TriggerID>(_trigs.Keys.Where((trigId) => (trigId.ApplicationID == id))))
            {
                UnregisterTrigger(trigId);
            }

            if (_apps.ContainsKey(id))
            {
                IGenesisApplication app = _apps[id];
                if (app.IsConnected())
                {
                    app.OnDisconnect();
                    app.AppID = 0;
                }
                _apps.Remove(id);
            }
        }

        private void noisyCleanupApp(uint id)
        {
            cleanupApp(id);
            EmitApplicationUnregistered(id);
        }

        private void heartBeat(object o)
        {
            List<uint> killed = new List<uint>();
            lock (_locker)
            {
                foreach (KeyValuePair<uint, IGenesisApplication> pair in _apps.ToList())
                {
                    if (!pair.Value.IsConnected())
                    {
                        cleanupApp(pair.Key);
                        killed.Add(pair.Key);
                    }
                }
            }
            foreach (uint id in killed)
                EmitApplicationUnregistered(id);
        }
    }

    public class HubException : Exception
    {
        public HubException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }

    public class HubProxy : GenesisObject, IHub
    {
        #region Deferred Delegates
        private interface IDeferredDelegate
        {
            void Execute();
        }

        private class DeferredDelegate1<T> : MarshalByRefObject, IDeferredDelegate
        {
            public delegate void D(T a1);

            private D _method;
            private T _a1;
            public DeferredDelegate1(D method, T a1)
            {
                _method = method;
                _a1 = a1;
            }

            public void Execute()
            {
                _method(_a1);
            }
        }

        private class DeferredDelegate2<T1, T2> : MarshalByRefObject, IDeferredDelegate
        {
            public delegate void D(T1 a1, T2 a2);

            private D _method;
            private T1 _a1;
            private T2 _a2;
            public DeferredDelegate2(D method, T1 a1, T2 a2)
            {
                _method = method;
                _a1 = a1;
                _a2 = a2;
            }

            public void Execute()
            {
                _method(_a1, _a2);
            }
        }
        #endregion

        private bool _inTask = false;
        private List<IDeferredDelegate> _deferred = new List<IDeferredDelegate>();
        private Hub _hub;
        public HubProxy(Hub hub)
        {
            _hub = hub;
            _hub.ApplicationRegistered += new RegisterApplicationEvent(_hub_ApplicationRegistered);
            _hub.ApplicationUnregistered += new UnregisterApplicationEvent(_hub_ApplicationUnregistered);
            _hub.ObserverConnected += new ConnectObserverEvent(_hub_ObserverConnected);
            _hub.ObserverDisconnected += new DisconnectObserverEvent(_hub_ObserverDisconnected);
            _hub.TriggerRegistered += new RegisterTriggerEvent(_hub_TriggerRegistered);
            _hub.TriggerUnregistered += new UnregisterTriggerEvent(_hub_TriggerUnregistered);
        }

        private void DoTask(HubTaskDelegate task)
        {
            DoTask(new HubTask(task));
        }

        public uint RegisterApplication(IGenesisApplication app)
        {
            _inTask = true;
            uint result = _hub.RegisterApplication(app);
            _inTask = false;
            return result;
        }

        public void UnregisterApplication(uint id)
        {
            _inTask = true;
            _hub.UnregisterApplication(id);
            _inTask = false;
        }

        public void DisconnectHub()
        {
            _hub.ApplicationRegistered -= _hub_ApplicationRegistered;
            _hub.ApplicationUnregistered -= _hub_ApplicationUnregistered;
            _hub.TriggerRegistered -= _hub_TriggerRegistered;
            _hub.TriggerUnregistered -= _hub_TriggerUnregistered;
            _hub.ObserverConnected -= _hub_ObserverConnected;
            _hub.ObserverDisconnected -= _hub_ObserverDisconnected;
            _hub = null;
        }

        #region Event Deferral
        public void _hub_TriggerUnregistered(TriggerID id)
        {
            if (_inTask)
                _deferred.Add(new DeferredDelegate1<TriggerID>(_hub_TriggerUnregistered, id));
            else if (TriggerUnregistered != null)
                TriggerUnregistered(id);
        }

        public void _hub_TriggerRegistered(TriggerID id)
        {
            if (_inTask)
                _deferred.Add(new DeferredDelegate1<TriggerID>(_hub_TriggerRegistered, id));
            else if (TriggerRegistered != null)
                TriggerRegistered(id);
        }

        public void _hub_ObserverDisconnected(TriggerID id, IObserver obs)
        {
            if (_inTask)
                _deferred.Add(new DeferredDelegate2<TriggerID, IObserver>(_hub_ObserverDisconnected, id, obs));
            else if (ObserverDisconnected != null)
                ObserverDisconnected(id, obs);
        }

        public void _hub_ObserverConnected(TriggerID id, IObserver obs)
        {
            if (_inTask)
                _deferred.Add(new DeferredDelegate2<TriggerID, IObserver>(_hub_ObserverConnected, id, obs));
            else if (ObserverConnected != null)
                ObserverConnected(id, obs);
        }

        public void _hub_ApplicationUnregistered(uint appId)
        {
            if (_inTask)
                _deferred.Add(new DeferredDelegate1<uint>(_hub_ApplicationUnregistered, appId));
            else if (ApplicationUnregistered != null)
                ApplicationUnregistered(appId);
        }

        public void _hub_ApplicationRegistered(uint appId)
        {
            if (_inTask)
                _deferred.Add(new DeferredDelegate1<uint>(_hub_ApplicationRegistered, appId));
            else if (ApplicationRegistered != null)
                ApplicationRegistered(appId);
        }
        #endregion

        #region IHub
        public void DoTask(HubTask task)
        {
            _inTask = true;
            try
            {
                _hub.DoTask(task);
            }
            catch (SocketException ex)
            {
                throw new HubException("Connection failed.", ex);
            }
            catch (Exception ex)
            {
                throw new HubException("Task threw exception.", ex);
            }
            finally
            {
                _inTask = false;
                foreach (IDeferredDelegate del in _deferred)
                    del.Execute();
                _deferred.Clear();
            }
        }

        #region IBaseHub
        public event RegisterApplicationEvent ApplicationRegistered;

        public event UnregisterApplicationEvent ApplicationUnregistered;

        public IEnumerable<KeyValuePair<uint, string>> GetRegisteredApps()
        {
            IEnumerable<KeyValuePair<uint, string>> result = null;
            DoTask((hub) => { result = hub.GetRegisteredApps(); });
            return result;
        }

        public IEnumerable<uint> GetRegisteredAppIDs(string appName)
        {
            IEnumerable<uint> result = null;
            DoTask((hub) => { result = hub.GetRegisteredAppIDs(appName); });
            return result;
        }

        public string GetRegisteredAppName(uint id)
        {
            string result = null;
            DoTask((hub) => { result = hub.GetRegisteredAppName(id); });
            return result;
        }

        public IEnumerable<TriggerID> GetTriggers()
        {
            IEnumerable<TriggerID> result = null;
            DoTask((hub) => { result = hub.GetTriggers(); });
            return result;
        }

        public IEnumerable<string> GetTriggerNames(uint appId)
        {
            IEnumerable<string> result = null;
            DoTask((hub) => { result = hub.GetTriggerNames(appId); });
            return result;
        }

        public bool TriggerNameExists(string name)
        {
            bool result = false;
            DoTask((hub) => { result = hub.TriggerNameExists(name); });
            return result;
        }

        public bool TriggerNameExists(string name, uint appId)
        {
            bool result = false;
            DoTask((hub) => { result = hub.TriggerNameExists(name, appId); });
            return result;
        }

        public bool RegisterTrigger(TriggerID id, ITrigger trig)
        {
            bool result = false;
            DoTask((hub) => { result = hub.RegisterTrigger(id, trig); });
            return result;
        }

        public event RegisterTriggerEvent TriggerRegistered;

        public void UnregisterTrigger(TriggerID id)
        {
            DoTask((hub) => { hub.UnregisterTrigger(id); });
        }

        public event UnregisterTriggerEvent TriggerUnregistered;

        public bool ConnectObserver(TriggerID id, IObserver obs)
        {
            bool result = false;
            DoTask((hub) => { result = hub.ConnectObserver(id, obs); });
            return result;
        }

        public event ConnectObserverEvent ObserverConnected;

        public void DisconnectObserver(TriggerID id, IObserver obs)
        {
            DoTask((hub) => { hub.DisconnectObserver(id, obs); });
        }

        public event DisconnectObserverEvent ObserverDisconnected;
        #endregion
        #endregion
    }

    public class HubDisconnectContext : MarshalByRefObject
    {
        private AutoResetEvent _event = new AutoResetEvent(false);

        public void Lock() { _event.WaitOne(); }
        public void Release() { _event.Set(); }
    }
}
