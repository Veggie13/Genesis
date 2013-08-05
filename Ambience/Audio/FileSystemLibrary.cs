using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace Genesis.Ambience.Audio
{
    public class FileSystemLibrary : ILibrary
    {
        private string _name;
        private DirectoryInfo _location;
        private Dictionary<string, FileInfo> _files;

        [DllImport("Shlwapi.dll")]
        private static extern bool PathRelativePathTo(
            StringBuilder pszPath,
            string pszFrom,
            FileAttributes dwAttrFrom,
            string pszTo,
            FileAttributes dwAttrTo
        );
        
        public FileSystemLibrary(string path)
        {
            string subpath = path;
            if (Directory.Exists(path))
            {
                _location = new DirectoryInfo(subpath);

                _files = new Dictionary<string, FileInfo>();
                FileInfo[] allFiles = _location.GetFiles("*", SearchOption.AllDirectories);
                foreach (var file in allFiles.Where(fi => fi.Extension.Equals(".wav") || fi.Extension.Equals(".mp3")))
                {
                    //StringBuilder relpath = new StringBuilder();
                    //PathRelativePathTo(relpath, subpath, FileAttributes.Directory, file.FullName, 0);

                    _files[file.Name] = file;
                }

                _name = _location.Name;
            }
            else if (File.Exists(path))
            {
                _location = new FileInfo(path).Directory;

                StreamReader reader = new StreamReader(path);
                XElement root = XElement.Parse(reader.ReadToEnd());
                reader.Close();
                if (!root.Name.LocalName.Equals("assl"))
                    throw new ArgumentException("Library file not assl format.");
                _files = root.Elements().ToDictionary((k) => (k.Name.LocalName), (v) => (new FileInfo(v.Value)));
                foreach (string key in _files.Where((p) => (!p.Value.Exists)).Select((p) => (p.Key)).ToList())
                    _files.Remove(key);
                _name = root.Attribute(XName.Get("name")).Value;
            }
            else
            {
                throw new FileNotFoundException("FileSystemLibrary.ctor(path)");
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Path
        {
            get { return _location.FullName; }
        }

        public IEnumerable<string> Sounds
        {
            get { return _files.Keys; }
        }

        public System.IO.Stream OpenStream(string name)
        {
            if (!_files.ContainsKey(name))
                throw new ArgumentException("name");

            FileInfo fi = _files[name];
            Stream stream = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
            return stream;
        }

        public Format FileFormat(string name)
        {
            if (!_files.ContainsKey(name))
                throw new ArgumentException("name");

            FileInfo fi = _files[name];
            if (fi.Extension.ToLower().Equals(".mp3"))
                return Format.MP3;
            if (fi.Extension.ToLower().Equals(".wav"))
                return Format.WAV;
            throw new InvalidDataException("format of " + name);
        }

        public ILibrary Export(LibraryType type, IEnumerable<string> names)
        {
            throw new NotImplementedException();
        }
    }
}
