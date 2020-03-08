using BambooCraft.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Packets
{
    public class PacketHandler
    {
        private Logging myLogger = new Logging();

        private byte[] packetData = new byte[4049];
        private int packetSize = 0;
        private int packetID = -1;
        private string packetInfo = "UNKNOWN PACKET";
        private byte[] packetResponse = new byte[4049];

        public bool IsValidPacket = false;
        public int lastByteIndex = 0;

        public List<byte> responseData = new List<byte>();

        internal void ReadPacket(byte[] receivedData)
        {
            packetData = receivedData;
            // TODO READ PACKET HERE <---------->

            packetSize = readVarInt();
            packetID = readVarInt();

            myLogger.Log(Severity.Packet, "Reading packet...");
            myLogger.Log(Severity.Packet, "Packet: " + packetID.ToString("X2") + " Size: " + packetSize);

            switch(packetID)
            {
                case 0:
                    myLogger.Log(Severity.Packet, "Handshake-Packet!");

                    var protocol = readVarInt();
                    string host = ReadString();
                    ushort port = ReadUShort();
                    var nextState = readVarInt();

                    
                    IsValidPacket = true;
                    packetInfo = "Handshake";

                    switch(nextState)
                    {
                        case 1:
                            // status request
                            packetResponse = HandshakeResponse();
                            break;
                        case 2:
                            // login request
                            break;
                    }

                    break;
            }
        }
        private byte[] HandshakeResponse()
        {
            myLogger.Log(Severity.Packet, "Building PingPayload...");
            PingPayload pl = new PingPayload();
            VersionPayload vpl = new VersionPayload();
            vpl.name = "1.15.2";
            vpl.protocol = 578;
            pl.vpl = vpl;

            PlayerPayload ppl = new PlayerPayload();
            ppl.max = 10;
            ppl.online = 0;
            ppl.sample = new List<Player>();
            pl.ppl = ppl;

            DescriptionPayload dpl = new DescriptionPayload();
            dpl.text = "Test";
            pl.dpl = dpl;

            pl.favicon = "data:image/png;base64,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";


            string json = JsonConvert.SerializeObject(pl);
            System.IO.File.WriteAllText("lastJSON.txt", json);
            byte[] dataToSend = Encoding.UTF8.GetBytes(json);

            WriteString(json);

            return dataToSend;
        }

        public void WriteString(string data)
        {
            responseData.Clear();
            responseData = new List<byte>();
            var stringData = Encoding.UTF8.GetBytes(data);
            WriteVarInt(stringData.Length);
            Write(stringData);
        }

        public void Write(byte[] data, int offset, int length)
        {
            for (var i = 0; i < length; i++)
            {
                responseData.Add(data[i + offset]);
            }
        }

        public void Write(byte[] data)
        {
            foreach (var i in data)
            {
                responseData.Add(i);
            }
        }

        public void WriteVarInt(int integer)
        {
            while ((integer & -128) != 0)
            {
                responseData.Add((byte)(integer & 127 | 128));
                integer = (int)(((uint)integer) >> 7);
            }
            responseData.Add((byte)integer);
        }

        internal byte[] GetResponse()
        {
            if(packetID == 0)
            {
                return responseData.ToArray();
            }


            return packetResponse;
        }

        public ushort ReadUShort()
        {
            var da = Read(2);
            return NetworkToHostOrder(BitConverter.ToUInt16(da, 0));
        }

        public int readVarInt()
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = ReadByte();
                int value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public string ReadString()
        {
            var length = readVarInt();
            var stringValue = Read(length);
            return Encoding.UTF8.GetString(stringValue);
        }

        public byte[] Read(int length)
        {
            var buffered = new byte[length];
            Array.Copy(packetData, lastByteIndex, buffered, 0, length);
            lastByteIndex += length;
            return buffered;
        }

        public byte ReadByte()
        {
            byte nextByte = packetData[lastByteIndex];
            lastByteIndex++;
            return nextByte;
        }

        public void Refresh()
        {
            packetData = new byte[4069];
            lastByteIndex = 0;
            packetID = -1;
            packetInfo = "UNKNOWN PACKET";
            packetSize = 0;
        }

        private double NetworkToHostOrder(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
            return BitConverter.ToDouble(data, 0);
        }

        private float NetworkToHostOrder(float network)
        {
            var bytes = BitConverter.GetBytes(network);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        private ushort[] NetworkToHostOrder(ushort[] network)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(network);
            return network;
        }

        private ushort NetworkToHostOrder(ushort network)
        {
            var net = BitConverter.GetBytes(network);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(net);
            return BitConverter.ToUInt16(net, 0);
        }
    }
}
