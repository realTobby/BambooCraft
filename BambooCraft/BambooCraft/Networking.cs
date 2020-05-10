using BambooCraft.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BambooCraft
{
    public class Networking
    {
        Logging myLogger = new Logging();
        PacketHandler myPacketHandler = new PacketHandler();
        public void HandleClient(Socket client)
        {
            while (client.Connected == true)
            {
                byte[] data = new byte[1024];
                if (client.Connected == true)
                {
                    try
                    {
                        int size = client.Receive(data);
                        myPacketHandler.ReadPacket(data);
                        if (myPacketHandler.Size > 0)
                        {
                            int responseSize = client.Send(myPacketHandler.GetResponse());
                            myPacketHandler.Dispose();
                            myLogger.Log(Severity.Network, "Response sent [" + responseSize + "]");
                        }
                        else
                        {
                            myLogger.Log(Severity.Network, "Last Packet got no response!");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        myLogger.Log(Severity.Exception, "Connection was closed...");
                        break;
                    }
                }
            }
        }

    }
}
