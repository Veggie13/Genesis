using System.Xml.Serialization;

namespace Genesis.Ambience.DataModel
{
    public class SoundEventElement : AEventElement
    {
        [XmlAttribute("resource")]
        public string Resource
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
