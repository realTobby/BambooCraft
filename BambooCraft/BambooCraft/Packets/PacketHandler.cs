using BambooCraft.Utils;
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

        private byte[] bufferedData = new byte[4049];
        private int packetSize = 0;
        private int packetID = -1;
        private string packetInfo = "UNKNOWN PACKET";
        private byte[] packetResponse = new byte[4049];

        public bool IsValidPacket = false;

        internal void ReadPacket(byte[] receivedData)
        {
            myLogger.Log(Severity.Packet, "Reading packet...");

            // TODO READ PACKET HERE <---------->
            var buf = new DataBuffer();
            buf.BufferedData = receivedData;
            buf.Size = ReadVarInt(receivedData[0]);
            packetID = buf.ReadVarInt();
            string PACKID = "0x" + packetID.ToString("X2");
            switch(PACKID)
            {
                default:
                    myLogger.Log(Severity.Critical, "UNKNOWN PACKET [" + PACKID + "]");
                    break;
                case "0x00":
                    packetInfo = "HANDHSHAKE (" + PACKID + ")";
                    IsValidPacket = true;
                    packetResponse = HandshakeResponse();
                    break;
                case "0xFE":
                    packetInfo = "LEGACY PING (" + PACKID + ")";
                    IsValidPacket = true;
                    packetResponse = LegacyPingResponse();
                    break;
            }
            buf.Dispose();
        }

        private int ReadVarInt(byte data)
        {
            var value = 0;
            var size = 0;
            int b;

            while (((b = data) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    return 0;
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }


        private byte[] HandshakeResponse()
        {
            myLogger.Log(Severity.Packet, "Preparing SLP JSON response..");
            string jsonData = System.IO.File.ReadAllText("serverListPacket.json");
            byte[] dataToSend = Encoding.Default.GetBytes(jsonData);
            myLogger.Log(Severity.Packet, "Ready to send SLP JSON response...");
            return dataToSend;
        }

        private byte[] LegacyPingResponse()
        {
            return bufferedData;
        }

        internal byte[] GetResponse()
        {
            myLogger.Log(Severity.Packet, "Responding to Packet ID: " + packetID + " - " + packetInfo);
            return packetResponse;
        }
    }
}
