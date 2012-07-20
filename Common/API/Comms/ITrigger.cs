using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Common.API.Comms
{
    public delegate void TriggerSignal(ITrigger sender, IDictionary<string, object> parms);

    [Serializable()]
    public struct TriggerID
    {
        public readonly string Name;
        public readonly uint ApplicationID;

        public TriggerID(string name, uint appId)
        {
            Name = name;
            ApplicationID = appId;
        }

        public bool Equals(TriggerID other)
        {
            return Name.Equals(other.Name) && (ApplicationID == other.ApplicationID);
        }
    }

    public interface ITrigger : IGenesisObject
    {
        string Name { get; }
        void AddObserver(IObserver obs);
        void RemoveObserver(IObserver obs);
    }

    public abstract class ATrigger : GenesisObject, ITrigger
    {
        public event TriggerSignal Signal;

        #region ITrigger Members
        
        public abstract string Name { get; }

        public void AddObserver(IObserver obs)
        {
            Signal += new TriggerSignal(obs.Trigger);
        }

        public void RemoveObserver(IObserver obs)
        {
            Signal -= obs.Trigger;
        }

        #endregion

        protected void EmitSignal(IDictionary<string, object> parms)
        {
            if (Signal != null)
                Signal(this, parms);
        }
    }
}
