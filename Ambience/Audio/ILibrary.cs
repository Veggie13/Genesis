using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Audio
{
    public enum LibraryType
    {
        FileSystem,
        Zip
    }

    public interface ILibrary : SoundEvent.IResourceLibrary
    {
        string Name { get; }
        IEnumerable<string> Sounds { get; }
        Stream OpenStream(string name);
        Format FileFormat(string name);
        ILibrary Export(LibraryType type, IEnumerable<string> names);
    }
}
