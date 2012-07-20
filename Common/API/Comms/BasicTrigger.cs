using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.API.Comms
{
    public class BasicTrigger : ATrigger
    {
        public BasicTrigger(string name)
        {
            _name = name;
        }

        private string _name;
        public override string Name
        {
            get { return _name; }
        }

        public void Emit(IDictionary<string, object> parms)
        {
            EmitSignal(parms);
        }
    }
}
