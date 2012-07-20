using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.API.Comms
{
    public enum ChannelMode
    {
        Local,
        NamedPipe,
        TCPIP
    }

    public interface IClientChannelDetailProvider
    {
        ChannelMode Mode { get; }
        string URL { get; }
    }
}
