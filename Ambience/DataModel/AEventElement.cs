using System.Drawing;
using Genesis.Common.Tools;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace Genesis.Ambience.DataModel
{
    public abstract class AEventElement : IVisitable<IEventElementVisitor, AEventElement>
    {
        #region Static Members
        private static Dictionary<Guid, AEventElement> _elements = new Dictionary<Guid, AEventElement>();
        #endregion

        #region Class Members
        private Guid _id;
        #endregion

        public AEventElement()
        {
            ID = Guid.NewGuid();
            _elements[ID] = this;
        }

        [XmlIgnore]
        public Guid ID
        {
            get { return _id; }
            set
            {
                _elements.Remove(_id);
                _id = value;
                _elements[_id] = this;
            }
        }

        [XmlAttribute("id")]
        public string ID_XML
        {
            get { return ID.Format(); }
            set { ID = new Guid(value); }
        }

        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlIgnore]
        public Color BaseColor
        {
            get;
            set;
        }

        [XmlAttribute("color")]
        public string BaseColor_XML
        {
            get { return FromColor(BaseColor); }
            set { BaseColor = ToColor(value); }
        }

        public abstract void Accept(IEventElementVisitor visitor);

        #region Static Helpers
        private static Color ToColor(string x)
        {
            try
            {
                if (x[0] == '#')
                {
                    return Color.FromArgb((x.Length <= 7 ? unchecked((int)0xFF000000) : 0) +
                        Int32.Parse(x.Substring(1), System.Globalization.NumberStyles.HexNumber));
                }
                else
                {
                    return Color.FromName(x);
                }
            }
            catch (Exception)
            {
                return Color.Black;
            }
        }

        private static string FromColor(Color c)
        {
            if (c.IsNamedColor)
                return c.Name;

            int value = c.ToArgb();

            if (((uint)value >> 24) == 0xFF)
                return string.Format("#{0:X6}", value & 0x00FFFFFF);
            else
                return string.Format("#{0:X8}", value);
        }

        internal static AEventElement GetElement(Guid id)
        {
            return _elements[id];
        }
        #endregion
    }

    public interface IEventElementVisitor : IVisitor<IEventElementVisitor, AEventElement>
    {
        void Visit(DelayEventElement element);
        void Visit(PeriodicEventElement element);
        void Visit(RandomEventElement element);
        void Visit(SequentialEventElement element);
        void Visit(SimultaneousEventElement element);
        void Visit(SoundEventElement element);
    }

    internal static class GuidExtensions
    {
        public static string Format(this Guid g)
        {
            return g.ToString().ToUpper();
        }
    }
}
