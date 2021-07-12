using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BambooCraft.Core.Logic
{
    public class Networking
    {
        PacketHandler myPacketHandler = new PacketHandler();
        public Networking(string ip, int port, int allowConnections)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            serverSocket.Bind(ep);
            serverSocket.Listen(allowConnections);
            Console.WriteLine("Server listening on ");

            while (true)
            {
                Socket clientSocket = default(Socket);
                clientSocket = serverSocket.Accept();
                Console.WriteLine("Client Connection established!");
                Thread newClientThread = new Thread(new ThreadStart(() => HandleClient(clientSocket)));
                newClientThread.Start();
            }
        }

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
                            Console.WriteLine("Server to client ===> Response sent [" + responseSize + "]");
                        }
                        else
                        {
                            Console.WriteLine("Last Packet got no response!");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Server ran into a problem...");
                        Console.WriteLine("Exception: " + ex.Message);
                        break;
                    }
                }
            }
        }
    }
}
