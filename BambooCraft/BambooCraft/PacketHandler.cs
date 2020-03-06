using BambooCraft.Models;
using BambooCraft.Packets;
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
        private PacketFactory pf = new PacketFactory();
        public bool isSendable = false;
        private Logging myLogger = new Logging();
        byte[] dataToSend;

        public PacketHandler()
        {
            dataToSend = new byte[1024];
        }

        public void NextPacket(byte[] packetData)
        {
            IPackets nextPacket = pf.ReadPacket(packetData);
            myLogger.Log(Severity.Packet, Encoding.UTF8.GetString(packetData));
            dataToSend = nextPacket.GetResponse();
        }

        public byte[] GetResponse()
        {
            return dataToSend;
        }

    }
}
