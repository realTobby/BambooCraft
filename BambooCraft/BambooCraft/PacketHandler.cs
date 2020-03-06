using BambooCraft.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft
{
    public class PacketHandler
    {
        public bool isSendable = false;
        private Logging myLogger = new Logging();
        byte[] dataToSend;

        public PacketHandler()
        {
            dataToSend = new byte[1024];
        }

        public void ReadPacket(byte[] packetData)
        {
            PacketFormatModel nextPacket = new PacketFormatModel();
            nextPacket.PacketLength = Tools.ReadVarInt(packetData[0]);
            nextPacket.PacketID = Tools.ReadVarInt(packetData[packetData.Length - nextPacket.PacketLength]);
            Array.Copy(packetData, packetData.Length - nextPacket.PacketLength-1, nextPacket.PacketData, 0, nextPacket.PacketLength);

            isSendable = false;
            int packetID = Tools.ReadVarInt(packetData[1]);
            string packetInfo = "{ID: " + packetID + "} " + Encoding.UTF8.GetString(packetData);
            bool packetFound = false;

            switch(packetID)
            {
                case 0: // HANDSHAKE
                    packetFound = true;
                    packetInfo = "HANDSHAKE PACKET RECEIVED! " + packetInfo;
                    isSendable = false; // NOTHING IMPLEMENTED YET

                    int protocolVersion = Tools.ReadVarInt(packetData[2]);

                    break;
            }



            //switch (packetData[0])
            //{
            //    case 0xFE: // 254  ==> Legacy Server Ping
            //        packetInfo = " (LegacyPing) >> " + packetInfo;
            //        //WorkLegacyPingPacket();
            //        packetFound = true;
            //        isSendable = true;
            //        break;

            //}
            
            if(packetFound == false)
            {
                packetInfo = "(Unkown/NotSupportedYet) >> " + packetInfo;
            }
            myLogger.Log(Severity.Packet, packetInfo);

        }



        

        private void WorkSLP()
        {
            // PACKET ID => 0x00
            // Protocol Version => 578 => 
            //
            //
            //

            myLogger.Log(Severity.Packet, "Preparing SLP JSON response..");
            string jsonData = System.IO.File.ReadAllText("serverListPacket.json");
            byte[] dataToSend = Encoding.Default.GetBytes(jsonData);
            myLogger.Log(Severity.Packet, "Ready to send SLP JSON response...");
        }

        private void WorkLegacyPingPacket()
        {
            List<byte> byteData = new List<byte>();
            // kick bytes
            byteData.Add(0xFF);
            byteData.Add(0x00);
            byteData.Add(0x00);

            // Start of text bytes
            byteData.Add(0x00);
            byteData.Add(0xa7);
            byteData.Add(0x00);
            byteData.Add(0x31);
            byteData.Add(0x00);
            byteData.Add(0x00);

            // other fields
            byteData.Add(0x00);
            byteData.Add(0x34); // ==> PROTOCOL VERSION
            byteData.Add(0x00);
            byteData.Add(0x37); // ==> PROTOCOL VERSION
            byteData.Add(0x00);
            byteData.Add(0x00);
            byteData.Add(0x00);

            byteData.Add(0x31);
            byteData.Add(0x00);
            byteData.Add(0x2e);
            byteData.Add(0x00);
            byteData.Add(0x34);
            byteData.Add(0x00);
            byteData.Add(0x2e);
            byteData.Add(0x00);
            byteData.Add(0x32);
            byteData.Add(0x00);
            byteData.Add(0x00);
            byteData.Add(0x00);
            byteData.Add(0x41);
            byteData.Add(0x00);
            byteData.Add(0x20);
            byteData.Add(0x00);

            byteData.Add(0x4d);
            byteData.Add(0x00);
            byteData.Add(0x69);
            byteData.Add(0x00);
            byteData.Add(0x6e);
            byteData.Add(0x00);
            byteData.Add(0x65);
            byteData.Add(0x00);
            byteData.Add(0x63);
            byteData.Add(0x00);
            byteData.Add(0x72);
            byteData.Add(0x00);
            byteData.Add(0x61);
            byteData.Add(0x00);
            byteData.Add(0x66);

            byteData.Add(0x74);
            byteData.Add(0x00);
            byteData.Add(0x20);
            byteData.Add(0x00);
            byteData.Add(0x53);
            byteData.Add(0x00);
            byteData.Add(0x65);
            byteData.Add(0x00);
            byteData.Add(0x72);
            byteData.Add(0x00);
            byteData.Add(0x76);
            byteData.Add(0x00);
            byteData.Add(0x65);
            byteData.Add(0x00);
            byteData.Add(0x72);
            byteData.Add(0x00);

            byteData.Add(0x00);
            byteData.Add(0x00);
            byteData.Add(0x30);
            byteData.Add(0x00);
            byteData.Add(0x00);
            byteData.Add(0x00);
            byteData.Add(0x32);
            byteData.Add(0x00);
            byteData.Add(0x30);

            //0000000: ff00 2300 a700 3100 00 00 3400 3700 0000  ....§.1...4.7...
            //0000010: 3100 2e00 3400 2e00 3200 0000 4100 2000  1...4...2...A. .
            //0000020: 4d00 6900 6e00 6500 6300 7200 6100 6600  M.i.n.e.c.r.a.f.
            //0000030: 7400 2000 5300 6500 7200 7600 6500 7200  t. .S.e.r.v.e.r.
            //0000040: 0000 3000 0000 3200 30                   ..0...2.0


            dataToSend = byteData.ToArray();
        }

        public byte[] GetResponse()
        {
            return dataToSend;
        }

    }
}
