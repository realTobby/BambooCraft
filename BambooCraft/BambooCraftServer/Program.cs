using BambooCraft.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraftServer
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Starting BambooCraft Server by realTobby");

            Networking n = new Networking("127.0.0.1", 41900, 10);


            Console.WriteLine("Server stopped...");
            Console.ReadKey();

        }
    }
}
