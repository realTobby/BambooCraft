﻿using BambooCraft.Packets;
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
        Logging myNetworkLogger = new Logging(Severity.Network);
        PacketHandler myPacketHandler = new PacketHandler();

        List<Socket> clientSocketList = new List<Socket>();


        public Networking(string ip, int port, int allowConnections)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            serverSocket.Bind(ep);
            serverSocket.Listen(allowConnections);
            myNetworkLogger.Log("Server listening on: " + ip + " and port: " + port);
            
            while (true)
            {
                Socket clientSocket = default(Socket);
                clientSocket = serverSocket.Accept();
                if(clientSocketList.Where(x=>x == clientSocket).FirstOrDefault() == null)
                {
                    myNetworkLogger.Log(string.Format("New Client connected..."));
                    clientSocketList.Add(clientSocket);
                    Thread newClientThread = new Thread(new ThreadStart(() => HandleClient(clientSocketList.Where(x => x == clientSocket).FirstOrDefault())));
                    newClientThread.Start();
                }
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
                            myNetworkLogger.Log("Response sent [" + responseSize + "]");
                        }
                        else
                        {
                            myNetworkLogger.Log("Last Packet got no response!");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        myNetworkLogger.Log("Server ran into a problem...");
                        myNetworkLogger.Log("Exception: " + ex.Message);
                        break;
                    }
                }
            }
        }

    }
}
