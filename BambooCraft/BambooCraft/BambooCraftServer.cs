using BambooCraft.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BambooCraft
{
    public class BambooCraftServer
    {
        Logging myLogger = new Logging();
        PacketHandler myPacketHandler = new PacketHandler();
        TcpListener listener = new TcpListener(IPAddress.Any, 419);
        public void StartServer()
        {
            listener.Start();

            while (true)
            {
                Socket client = listener.AcceptSocket();
                myLogger.Log(Severity.Network, "New Connection");

                var childSocketThread = new Thread(() =>
                {
                    byte[] data = new byte[4096];
                    int receivedSize = client.Receive(data);
                    myPacketHandler.ReadPacket(data);
                    if(myPacketHandler.Size > 0)
                    {
                        int responseSize = client.Send(myPacketHandler.GetResponse());
                        myPacketHandler.Dispose();
                        myLogger.Log(Severity.Network, "Response sent [" + responseSize + "]");
                    }
                    else
                    {
                        myLogger.Log(Severity.Network, "Last Packet got no response!");
                    }
                });
                childSocketThread.Start();
            }
        }

    }
}
