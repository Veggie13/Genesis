using System.Collections.Generic;
using System.Xml.Serialization;
using System;

namespace Genesis.Ambience.DataModel
{
    [Serializable]
    [XmlRoot("assp", Namespace = "", IsNullable = false)]
    public class Project
    {
        [XmlArray("libraries"),
         XmlArrayItem("folder", Type = typeof(FolderLibraryElement))]
        public List<ALibraryElement> Libraries
        {
            get;
            set;
        }

        [XmlArray("events"),
         XmlArrayItem("delay", Type = typeof(DelayEventElement)),
         XmlArrayItem("periodic", Type = typeof(PeriodicEventElement)),
         XmlArrayItem("random", Type = typeof(RandomEventElement)),
         XmlArrayItem("sequence", Type = typeof(SequentialEventElement)),
         XmlArrayItem("simultaneous", Type = typeof(SimultaneousEventElement)),
         XmlArrayItem("sound", Type = typeof(SoundEventElement))]
        public List<AEventElement> Events
        {
            get;
            set;
        }

        [XmlArray("soundboard"), XmlArrayItem("button")]
        public List<SoundBoardElement> SoundBoard
        {
            get;
            set;
        }

        public static XmlSerializer GetSerializer()
        {
            return new XmlSerializer(typeof(Project));
        }
    }
}
