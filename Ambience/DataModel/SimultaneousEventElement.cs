using System.Xml.Serialization;
using System.Collections.Generic;

namespace Genesis.Ambience.DataModel
{
    public class SimultaneousEventElement : AEventElement
    {
        [XmlArray("group"), XmlArrayItem("item")]
        public List<SubordinateElement> Group
        {
            get;
            set;
        }

        public override void Accept(IEventElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
