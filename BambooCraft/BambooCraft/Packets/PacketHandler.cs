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
                    packetResponse = HandshakeResponse();
                    IsValidPacket = true;
                    packetInfo = "Handshake";
                    break;
            }

            

        }
        private byte[] HandshakeResponse()
        {
            PingPayload pl = new PingPayload();
            pl.Icon = "";
            pl.Motd = "BambooCraft 1.15.2";
            PlayersPayload ppl = new PlayersPayload();
            ppl.Max = 10;
            ppl.Online = 0;
            ppl.Sample = new List<Player>();
            pl.Players = ppl;
            VersionPayload vpl = new VersionPayload();
            vpl.Name = "1.15.2";
            vpl.Protocol = 578;
            pl.Version = vpl;

            string json = JsonConvert.SerializeObject(pl);
            myLogger.Log(Severity.Packet, "Building PingPayload...");
            byte[] dataToSend = Encoding.Default.GetBytes(json);
            return dataToSend;
        }

        internal byte[] GetResponse()
        {
            return packetResponse;
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
    }
}
