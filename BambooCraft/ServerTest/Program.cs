using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            int port = 13000;
            string ipAddress = "127.0.0.1";
            Socket socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            socketListener.Bind(ep);
            socketListener.Listen(100);

            Console.WriteLine("Server is running...");

            Console.WriteLine("Waiting for connections...");


            Socket clientSocket = default(Socket);
            int connections = 0;
            while(true)
            {
                connections++;
                clientSocket = socketListener.Accept();
                Console.WriteLine("New connection made!");
                Console.WriteLine("Connected clients: " + connections);

                Thread clientConnection = new Thread(new ThreadStart(() => p.HandleConnection(clientSocket)));
                clientConnection.Start();

            }



        }

        public void HandleConnection(Socket client)
        {
            while(true)
            {
                byte[] msg = new byte[1024];
                int size = client.Receive(msg);
                client.Send(msg, 0, size, SocketFlags.None);
            }

        }
    }
}
