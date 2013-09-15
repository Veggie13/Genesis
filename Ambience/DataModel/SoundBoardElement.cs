using System.Xml.Serialization;

namespace Genesis.Ambience.DataModel
{
    public class SoundBoardElement : SubordinateElement
    {
        [XmlAttribute("row")]
        public int Row
        {
            get;
            set;
        }

        [XmlAttribute("col")]
        public int Col
        {
            get;
            set;
        }
    }

    public static class SoundBoardExtensions
    {
        public static SoundBoardElement ToSoundBoard(this AEventElement el, int row, int col)
        {
            return new SoundBoardElement() { Subordinate = el, Row = row, Col = col };
        }
    }
}
