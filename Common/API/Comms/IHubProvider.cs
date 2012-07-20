using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.API.Comms
{
    public interface IHubProvider
    {
        bool Initialize(IGenesisApplication app);
        void Finish(IGenesisApplication app);

        IHub Hub { get; }
    }
}
