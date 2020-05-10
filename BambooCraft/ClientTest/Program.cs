using System;
using System.Net;
using System.Net.Sockets;


namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 13000;
            string ipAddress = "127.0.0.1";

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            clientSocket.Connect(ep);
            Console.WriteLine("Client is connected!");

            while(true)
            {
                Console.WriteLine("Enter message:");
                string messageFromClient = Console.ReadLine();
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient), 0, messageFromClient.Length, SocketFlags.None);

                byte[] messageFromServer = new byte[1024];
                int size = clientSocket.Receive(messageFromServer);

                Console.WriteLine("The Server returned: " + System.Text.Encoding.ASCII.GetString(messageFromServer, 0, size));

            }
            

        }
    }
}
