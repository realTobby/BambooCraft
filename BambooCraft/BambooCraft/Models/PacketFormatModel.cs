using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Models
{
    public class PacketFormatModel
    {
        public int PacketLength { get; set; }
        public int PacketID { get; set; }
        public byte[] PacketData { get; set; } = new byte[1024];
    }
}
