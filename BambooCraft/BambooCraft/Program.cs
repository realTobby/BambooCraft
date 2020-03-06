using BambooCraft;
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
    class Program
    {
        static long ToInt(string addr)
        {
            // careful of sign extension: convert to uint first;
            // unsigned NetworkToHostOrder ought to be provided.
            return (long)(uint)IPAddress.NetworkToHostOrder(
                 (int)IPAddress.Parse(addr).Address);
        }

        static void Main(string[] args)
        {
            string currentMode = "unknown";

#if DEBUG
                currentMode = "[DEV]";
#else
                 currentMode = "[RELEASE]"; 
#endif
            Console.WriteLine("BambooCraft " + currentMode);

            //SocketListener bambooCraftServer = new SocketListener();
            //bambooCraftServer.StartListening();

            TcpListener server = new TcpListener(new IPAddress((long)ToInt("1.0.0.127")), 419);
            // we set our IP address as server's address, and we also set the port: 9999

            server.Start();  // this will start the server

            while (true)   //we wait for a connection
            {
                TcpClient client = server.AcceptTcpClient();  //if a connection exists, the server will accept it

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                while (client.Connected)  //while the client is connected, we look for incoming messages
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                    Console.WriteLine(Encoding.BigEndianUnicode(msg).ToString()); //now , we write the message as string
                }
            }
        }
    }
}
