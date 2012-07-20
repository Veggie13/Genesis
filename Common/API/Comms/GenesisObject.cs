using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Lifetime;

namespace Genesis.Common.API.Comms
{
    public interface IGenesisObject
    {
    }

    public class GenesisObject : MarshalByRefObject, IGenesisObject
    {
    }

    static class GenesisObjectExtensions
    {
        public static bool IsConnected(this IGenesisObject o)
        {
            if (!(o is MarshalByRefObject))
                return true;
            MarshalByRefObject obj = o as MarshalByRefObject;
            try
            {
                ILease lease = obj.GetLifetimeService() as ILease;
                return (lease == null || lease.CurrentState == LeaseState.Active);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
