using BambooCraft.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BambooCraft
{
    public class BambooCraftServer
    {
        public void StartupServer()
        {
            Networking network = new Networking("127.0.0.1", 41900, 10);
        }
    }
}
