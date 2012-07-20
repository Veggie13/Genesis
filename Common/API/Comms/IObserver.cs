using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.API.Comms
{
    public interface IObserver : IGenesisObject
    {
        uint ApplicationID { get; }
        void Trigger(ITrigger sender, IDictionary<string, object> parms);
    }

    public abstract class AObserver : GenesisObject, IObserver
    {
        #region IObserver Members
        public abstract uint ApplicationID { get; }
        public abstract void Trigger(ITrigger sender, IDictionary<string, object> parms);
        #endregion
    }
}
