using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;

namespace Genesis.Ambience.Audio
{
    public class ResourceManager
    {
        #region Class Members
        private struct Basket
        {
            public SoundResource res;
        }

        private Dictionary<string, ILibrary> _libs = new Dictionary<string, ILibrary>();
        private Dictionary<string, FileInfo> _indivFiles = new Dictionary<string, FileInfo>();
        private Dictionary<string, SoundResource> _loaded = new Dictionary<string, SoundResource>();

        private bool _running = false;
        private bool _synced = false;
        private Thread _eventThread;
        private AutoResetEvent _eventTrigger = new AutoResetEvent(false);
        private Queue<ResourceEvent> _events = new Queue<ResourceEvent>();
        #endregion

        #region Public Operations
        public void LoadLibrary(string path)
        {
            ILibrary lib = new FileSystemLibrary(path);
            _libs[lib.Name] = lib;
        }

        public void UnloadLibrary(string path)
        {
            throw new NotImplementedException("UnloadLibrary");
        }

        public List<string> GetAllSounds()
        {
            var result = new List<string>();
            foreach (var lib in _libs.Values)
            {
                result.AddRange(lib.Sounds.Select((s) => (lib.Name + "::" + s)));
            }

            return result;
        }

        public SoundResource GetResource(string name)
        {
            ILibrary lib;
            getSeparatedName(out lib, ref name);

            if (_loaded.ContainsKey(lib.Name + "::" + name))
                return _loaded[lib.Name + "::" + name];

            return loadResource(lib, name);
        }

        public void Start()
        {
            _eventThread = new Thread(new ThreadStart(eventThread));
            _eventTrigger.Reset();
            _eventThread.Start();
            _running = true;
        }

        public void Stop()
        {
            _running = false;
            _eventTrigger.Set();
            _eventThread.Join();
            _eventThread = null;
        }
        #endregion

        #region Private Helpers
        private void getSeparatedName(out ILibrary lib, ref string resName)
        {
            if (resName.Contains("::"))
            {
                int nSep = resName.LastIndexOf("::");
                string libName = resName.Substring(0, nSep);
                resName = resName.Substring(nSep + 2);

                if (!_libs.ContainsKey(libName))
                    throw new ArgumentException("name");
                lib = _libs[libName];
                return;
            }

            string name = resName.ToLower();
            lib = _libs.Where(p => p.Value.Sounds.Contains(name)).Select(p => p.Value).FirstOrDefault();
            if (lib == null)
                throw new ArgumentException("name");
        }

        private SoundResource loadResource(ILibrary lib, string resName)
        {
            Basket b = new Basket();
            ResourceManager mgr = this;
            AutoResetEvent signal = new AutoResetEvent(false);
            syncAction(() =>
            {
                b.res = new SoundResource(mgr, lib.OpenStream(resName), lib.FileFormat(resName));
                b.res.init();
                signal.Set();
            });
            signal.WaitOne();

            _loaded[lib.Name + "::" + resName] = b.res;

            return b.res;
        }

        internal delegate void ResourceEvent();
        internal void syncAction(ResourceEvent evt)
        {
            if (_synced)
            {
                evt();
                return;
            }

            lock (_events)
            {
                _events.Enqueue(evt);
            }

            _eventTrigger.Set();
        }

        private void allocateResource(string filename)
        {
        }

        private void eventThread()
        {
            while (_eventTrigger.WaitOne() && _running)
            {
                lock (_events)
                {
                    _synced = true;
                    while (_events.Count > 0)
                    {
                        var evt = _events.Dequeue();
                        evt();
                    }
                    _synced = false;
                }
            }

            _synced = true;
            foreach (var res in _loaded.Values)
            {
                res.Dispose();
            }
            _synced = false;
        }
        #endregion
    }
}
