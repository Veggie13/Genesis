using System.Xml.Serialization;
using System.Collections.Generic;

namespace Genesis.Ambience.DataModel
{
    public class RandomEventElement : AEventElement
    {
        [XmlArray("selection"), XmlArrayItem("item")]
        public List<SubordinateElement> Selection
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
