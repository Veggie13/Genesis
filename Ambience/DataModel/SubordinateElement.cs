using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Genesis.Ambience.DataModel
{
    public class SubordinateElement
    {
        [XmlIgnore]
        public AEventElement Subordinate
        {
            get { return AEventElement.GetElement(ID); }
            set { ID = value.ID; }
        }

        [XmlIgnore]
        public Guid ID
        {
            get;
            set;
        }

        [XmlText]
        public string Text
        {
            get { return ID.Format(); }
            set { ID = new Guid(value); }
        }

        public static implicit operator SubordinateElement(AEventElement el)
        {
            return new SubordinateElement() { Subordinate = el };
        }
    }
}
