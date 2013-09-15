using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Genesis.Ambience.DataModel
{
    public abstract class ALibraryElement
    {
        [XmlAttribute("path")]
        public string Path
        {
            get;
            set;
        }
    }
}
