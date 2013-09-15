using System.Xml.Serialization;

namespace Genesis.Ambience.DataModel
{
    public class PeriodicEventElement : AEventElement
    {
        [XmlElement("subordinate")]
        public SubordinateElement Subordinate
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
