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
        Networking network = new Networking();
        Logging myLogger = new Logging();

        public void StartupServer()
        {
            string ipAddress = "127.0.0.1";
            int port = 41900;

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            serverSocket.Bind(ep);
            serverSocket.Listen(100);
            myLogger.Log(Severity.Network, "Server listening on: " + ipAddress + " and port: " + port);
            Socket clientSocket = default(Socket);
            while(true)
            {
                clientSocket = serverSocket.Accept();
                myLogger.Log(Severity.Network, string.Format("New Client connected..."));
                Thread newClientThread = new Thread(new ThreadStart(() => network.HandleClient(clientSocket)));
                newClientThread.Start();
            }
        }
    }
}
