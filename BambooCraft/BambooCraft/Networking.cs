﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft
{
    public class Networking
    {
        private PacketHandler myPacketHandler = new PacketHandler();
        private bool isExcpetionLoggingEnabled = true;
        private byte[] _buffer = new byte[1024];
        private List<Socket> _clientSockets = new List<Socket>();
        private Logging myLogger;
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Networking(Logging logger)
        {
            myLogger = logger;
        }

        public void SetupServer()
        {
            myLogger.Log(Severity.Information, "Starting up server...");
            try
            {
                _serverSocket.Bind(new IPEndPoint(ConvertIPToInt("1.0.0.127"), 419));
                _serverSocket.Listen(100); // PENDING CONNECTIONS => NOT CLIENT COUNT
                myLogger.Log(Severity.Information, "Server is up and running on Adress: 127.0.0.1 on Port: 419");
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallbackAsync), null);
            }catch(SocketException se)
            {
                if(isExcpetionLoggingEnabled == true)
                    myLogger.Log(Severity.Exception, "(SE-SETUP) " + se.Message);
            }
            catch(ObjectDisposedException ode)
            {
                if (isExcpetionLoggingEnabled == true)
                    myLogger.Log(Severity.Exception, "(ODE-SETUP) " + ode.Message);
            }
        }

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
            myLogger.Log(Severity.Network, "Client connected to Socket");
            newConnection.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallbackAsync), newConnection);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallbackAsync), null);
        }

        private void RecieveCallbackAsync(IAsyncResult ar)
        {
            Socket clientConnection = (Socket)ar.AsyncState;
            int recieved = clientConnection.EndReceive(ar);

            if(recieved <= 0)
            {
                return;
            }

            byte[] receivedData = new byte[recieved];
            Array.Copy(_buffer, receivedData, recieved);
            
            myPacketHandler.ReadPacket(receivedData);
            byte[] responseData = myPacketHandler.GetResponse();
            if(myPacketHandler.isSendable == true)
            {
                try
                {
                    clientConnection.BeginSend(responseData, 0, responseData.Length, SocketFlags.None, new AsyncCallback(SendCallbackAsync), clientConnection);
                    clientConnection.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallbackAsync), clientConnection);
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
                myLogger.Log(Severity.Network, "NO RESPONSE SENT TO LAST PACKET!");
            }
            
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
                myLogger.Log(Severity.Network, "RESPONSE SENT! (" + sent + ")");
            }


            
        }

    }
}