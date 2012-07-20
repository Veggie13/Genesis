using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;

namespace Genesis.Common.API.Comms
{
    public struct ServerChannelDetails
    {
        public ServerChannelDetails(ChannelMode mode, int port)
        {
            Mode = mode;
            Port = port;
        }

        public ChannelMode Mode;
        public int Port;
    }

    public static class ChannelFactory
    {
        public static IChannel CreateClientChannel(IClientChannelDetailProvider details)
        {
            var props = new Dictionary<string, string>();
            props["port"] = "0";
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            switch (details.Mode)
            {
                case ChannelMode.NamedPipe:
                    return new IpcChannel(props, null, serverProvider);
                case ChannelMode.TCPIP:
                    return new TcpChannel(props, null, serverProvider);
                default:
                    return null;
            }
        }

        public static IChannel CreateServerChannel(ServerChannelDetails details)
        {
            var props = new Dictionary<string, string>();
            props["port"] = details.Port.ToString();
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            switch (details.Mode)
            {
                case ChannelMode.NamedPipe:
                    return new IpcChannel(props, null, serverProvider);
                case ChannelMode.TCPIP:
                    return new TcpChannel(props, null, serverProvider);
                default:
                    return null;
            }
        }
    }
}
