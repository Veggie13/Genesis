using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Audio
{
    public class TestResourceManager : SoundEvent.IResourceProvider
    {
        private class TestResource : SoundEvent.IResource
        {
            public event Action PlaybackStopped = () => { };

            public double Length
            {
                get { return 1; }
            }

            public string FullName
            {
                get;
                set;
            }

            public void Play()
            {
                PlaybackStopped();
            }

            public void Stop()
            {
            }
        }

        #region Class Members
        private Dictionary<string, ILibrary> _libs = new Dictionary<string, ILibrary>();
        private Dictionary<string, FileInfo> _indivFiles = new Dictionary<string, FileInfo>();
        private Dictionary<string, TestResource> _loaded = new Dictionary<string, TestResource>();
        #endregion

        #region Properties
        public IEnumerable<SoundEvent.IResourceLibrary> Libraries
        {
            get
            {
                return _libs.Values;
            }
        }
        #endregion

        #region Public Operations
        public void LoadLibrary(string path)
        {
            ILibrary lib = new FileSystemLibrary(path);
            _libs[lib.Name] = lib;
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

        private TestResource GetResource(string name)
        {
            ILibrary lib;
            getSeparatedName(out lib, ref name);

            if (_loaded.ContainsKey(lib.Name + "::" + name))
                return _loaded[lib.Name + "::" + name];

            return loadResource(lib, name);
        }

        SoundEvent.IResource SoundEvent.IResourceProvider.GetResource(string name)
        {
            return GetResource(name);
        }

        public void Start()
        {
        }

        public void Stop()
        {
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

        private TestResource loadResource(ILibrary lib, string resName)
        {
            var res = new TestResource() { FullName = lib.Name + "::" + resName };
            _loaded[res.FullName] = res;
            return res;
        }
        #endregion
    }
}
