using System.Xml.Serialization;
using System;

namespace Genesis.Ambience.DataModel
{
    public class DelayEventElement : AEventElement
    {
        [XmlElement("subordinate")]
        public SubordinateElement Subordinate
        {
            get;
            set;
        }

        [XmlAttribute("delay")]
        public uint Delay
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
