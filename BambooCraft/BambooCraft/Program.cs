using BambooCraft;
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
    class Program
    {
        static long ToInt(string addr)
        {
            // careful of sign extension: convert to uint first;
            // unsigned NetworkToHostOrder ought to be provided.
            return (long)(uint)IPAddress.NetworkToHostOrder(
                 (int)IPAddress.Parse(addr).Address);
        }

        static void Main(string[] args)
        {
            string currentMode = "unknown";

#if DEBUG
                currentMode = "[DEV]";
#else
                 currentMode = "[RELEASE]"; 
#endif
            Console.WriteLine("BambooCraft by https://github.com/realTobby " + currentMode);

            Networking bambooCraft = new Networking(new Logging());
            bambooCraft.SetupServer();
            Console.ReadLine();
        }
    }
}
