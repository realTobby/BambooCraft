using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Packets
{
    public class BasePacket : IPackets
    {
        internal Logging myLogger = new Logging();
        public int PacketLength { get; set; }
        public int PacketID { get; set; }
        public byte[] PacketData { get; set; }

        public BasePacket(byte[] data)
        {
            UnwrapPacket(data);
        }
        public void UnwrapPacket(byte[] packetData)
        {
            PacketLength = Tools.ReadVarInt(packetData[0]);
            PacketID = Tools.ReadVarInt(packetData[packetData.Length - PacketLength]);
            Array.Copy(packetData, packetData.Length - PacketLength - 1, PacketData, 0, PacketLength);
        }

        public virtual byte[] GetResponse()
        {
            return PacketData;
        }


    }
}
