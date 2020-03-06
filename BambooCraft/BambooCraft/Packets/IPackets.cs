using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Packets
{
    public interface IPackets
    {
        void UnwrapPacket(byte[] packetData);
        byte[] GetResponse();
    }
}
