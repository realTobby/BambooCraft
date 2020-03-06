using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Packets
{
    public class PacketFactory
    {
        public IPackets ReadPacket(byte[] packetData)
        {
            BasePacket packetInformation = new BasePacket(packetData);
            IPackets responsePacket = null;
            switch(packetInformation.PacketID)
            {
                case 0:
                    responsePacket = new HandshakePacket(packetInformation);
                    break;
            }

            if(responsePacket != null)
                return responsePacket;
            return packetInformation;
        }

    }
}
