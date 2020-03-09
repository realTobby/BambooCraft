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
        private TcpListener _serverListener = new TcpListener(IPAddress.Any, 25565);
        private bool _listening = true;

        private PacketHandler myPacketHandler = new PacketHandler();
        private int packetCounter = 0;
        private bool isExcpetionLoggingEnabled = true;
        private byte[] _buffer = new byte[9999];
        private List<Socket> _clientSockets = new List<Socket>();
        private Logging myLogger;
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Networking(Logging logger)
        {
            myLogger = logger;
        }

        private void HandleClientCommNew(object client)
        {
            var tcpClient = (TcpClient)client;
            var clientStream = tcpClient.GetStream();
            myLogger.Log(Severity.Network, "Incoming Packet...");
            while (true)
            {
                byte[] receivedData = new byte[1024];
                using (MemoryStream ms = new MemoryStream())
                {

                    int numBytesRead;
                    while ((numBytesRead = clientStream.Read(receivedData, 0, receivedData.Length)) > 0)
                    {
                        ms.Write(receivedData, 0, numBytesRead);
                    }
                    myLogger.Log(Severity.Network, "Reading packet...");
                    myPacketHandler.ReadPacket(receivedData);
                    byte[] response = myPacketHandler.GetResponse();
                }
            }
        }

        public void SetupServer()
        {
            myLogger.Log(Severity.Information, "Starting up server...");

            _serverListener = new TcpListener(IPAddress.Any, 419);

            _serverListener.Start();
            _listening = true;
            myLogger.Log(Severity.Information, "Ready for connections...");
            while (_listening)
            {
                var client = _serverListener.AcceptTcpClient();
                myLogger.Log(Severity.Information, "A new connection has been made!");

                new Task((() => { HandleClientCommNew(client); })).Start(); //Task instead of Thread
            }

            //try
            //{
            //    _serverSocket.Bind(new IPEndPoint(IPAddress.NetworkToHostOrder((int)ConvertIPToInt("127.0.0.1")), 419));
            //    _serverSocket.Listen(10); // PENDING CONNECTIONS => NOT CLIENT COUNT
            //    myLogger.Log(Severity.Information, "Server is up and running on Adress: 127.0.0.1 on Port: 419");
            //    _serverSocket.BeginAccept(AcceptCallbackAsync, null);
            //}catch(SocketException se)
            //{
            //    if(isExcpetionLoggingEnabled == true)
            //        myLogger.Log(Severity.Exception, "(SE-SETUP) " + se.Message);
            //}
            //catch(ObjectDisposedException ode)
            //{
            //    if (isExcpetionLoggingEnabled == true)
            //        myLogger.Log(Severity.Exception, "(ODE-SETUP) " + ode.Message);
            //}
        }

        [Obsolete]
        private long ConvertIPToInt(string addr)
        {
            // careful of sign extension: convert to uint first;
            // unsigned NetworkToHostOrder ought to be provided.
            return (long)(uint)IPAddress.NetworkToHostOrder(
                 (int)IPAddress.Parse(addr).Address);
        }

        private void AcceptCallbackAsync(IAsyncResult ar)
        {
            Socket newConnection = _serverSocket.EndAccept(ar);
            _clientSockets.Add(newConnection);

            myLogger.Log(Severity.Network, "New Client connected!");
            newConnection.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecieveCallbackAsync, newConnection);
            _serverSocket.BeginAccept(AcceptCallbackAsync, newConnection);
            
        }

        private void RecieveCallbackAsync(IAsyncResult ar)
        {



            Socket clientConnection = (Socket)ar.AsyncState;

            while(true)
            {
                int recieved = clientConnection.EndReceive(ar);
                if (recieved <= 0)
                {
                    return;
                }
                myLogger.Log(Severity.Network, "Packet received!");
                byte[] receivedData = new byte[recieved];
                Array.Copy(_buffer, receivedData, recieved);

                // PACKET HERE
                myPacketHandler.ReadPacket(receivedData);
                if (myPacketHandler.IsValidPacket == true)
                {
                    byte[] responseData = myPacketHandler.GetResponse();
                    try
                    {

                        clientConnection.BeginSend(responseData, 0, responseData.Length, SocketFlags.None, SendCallbackAsync, clientConnection);
                    }
                    catch (SocketException se)
                    {
                        if (isExcpetionLoggingEnabled == true)
                            myLogger.Log(Severity.Exception, "(SE-RECEIVE) " + se.Message);
                    }
                    catch (ObjectDisposedException ode)
                    {
                        if (isExcpetionLoggingEnabled == true)
                            myLogger.Log(Severity.Exception, "(ODE-RECEIVE) " + ode.Message);
                    }
                }
                else
                {
                    myLogger.Log(Severity.Error, "Last Packet is unknown, it was not handled!");
                }
                myPacketHandler.Dispose();
            }
            
            clientConnection.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecieveCallbackAsync, clientConnection);
        }


        private void SendCallbackAsync(IAsyncResult ar)
        {
            Socket clientConnection = (Socket)ar.AsyncState;
            int sent = clientConnection.EndSend(ar);
            if(sent <= 0)
            {
                return;
            }
            else
            {
                myLogger.Log(Severity.Network, "Response sent (" + sent + ")");
            }
        }

    }
}
