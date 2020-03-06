using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Packets
{
    public class HandshakePacket : BasePacket
    {
        public HandshakePacket(BasePacket info) : base(info.PacketData)
        {
            base.PacketData = info.PacketData;
            base.PacketID = info.PacketID;
            base.PacketLength = info.PacketLength;
        }

        public override byte[] GetResponse()
        {
            myLogger.Log(Severity.Packet, "Preparing SLP JSON response..");
            string jsonData = System.IO.File.ReadAllText("serverListPacket.json");
            byte[] dataToSend = Encoding.Default.GetBytes(jsonData);
            myLogger.Log(Severity.Packet, "Ready to send SLP JSON response...");
            return dataToSend;
        }

    }
}
