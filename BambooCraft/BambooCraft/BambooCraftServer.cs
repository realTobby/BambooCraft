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
        BambooCraftServer bcs;
        Logging myLogger = new Logging();

        private void HandleNewClient(Socket client)
        {
            while (client.Connected == true)
            {
                myLogger.Log(Severity.Network, "Looking for data...");
                byte[] data = new byte[1024];
                if (client.Connected == true)
                {
                    try
                    {
                        int size = client.Receive(data);
                        myLogger.Log(Severity.Network, "Data recieved: " + System.Text.Encoding.ASCII.GetString(data).Trim());
                        client.Send(data, 0, size, SocketFlags.None);
                    }
                    catch (Exception ex)
                    {
                        myLogger.Log(Severity.Exception, "Connection was closed...");
                    }

                }
                myLogger.Log(Severity.Network, "Connection was closed");
            }
        }

        public void StartupServer()
        {

            string ipAddress = "127.0.0.1";
            int port = 41900;

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            serverSocket.Bind(ep);
            serverSocket.Listen(100);
            myLogger.Log(Severity.Network, "Server started...");
            Socket clientSocket = default(Socket);
            while(true)
            {
                clientSocket = serverSocket.Accept();
                myLogger.Log(Severity.Network, string.Format("New Client connected..."));
                Thread newClientThread = new Thread(new ThreadStart(() => HandleNewClient(clientSocket)));
                Thread connectionSafe = new Thread(new ThreadStart(() => CheckConnection(clientSocket, newClientThread)));
                connectionSafe.Start();
                newClientThread.Start();
            }
        }

        private void CheckConnection(Socket clientSocket, Thread newClientThread)
        {
            while(true)
            {
                if(clientSocket.Connected == false)
                {
                    myLogger.Log(Severity.Warning, "Client disconnected!");
                    newClientThread.Abort();
                }
            } 
        }
    }
}
