using System.Xml.Serialization;
using System.Collections.Generic;

namespace Genesis.Ambience.DataModel
{
    public class SequentialEventElement : AEventElement
    {
        [XmlArray("sequence"), XmlArrayItem("item")]
        public List<SubordinateElement> Sequence
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
