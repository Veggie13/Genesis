using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Scheduler;
using System.Drawing;

namespace Genesis.Ambience.Controls
{
    public class ProviderToken
    {
        private IEventProvider _prov;
        private IEventColorProvider _colorer;

        public ProviderToken(IEventProvider prov, IEventColorProvider colorer)
        {
            _prov = prov;
            _colorer = colorer;
            _name = _prov.Name;
        }

        public void Finish()
        {
            _prov = null;
        }
        
        public IEventProvider Provider
        {
            get { return _prov; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public Color Color
        {
            get
            {
                if (_colorer == null || _prov == null)
                    return Color.Gray;
                return _colorer[_prov];
            }
        }
    }
}
