using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft
{
    public static class Tools
    {
        public static int ReadVarInt(NetworkStream stream)
        {
            var value = 0;
            var size = 0;
            int b;

            while (((b = stream.ReadByte()) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("VarInt too long. Hehe that's punny.");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }

        public static byte writeVarInt(int value)
        {
            do
            {
                byte temp = (byte)(value & 0b01111111);
                // Note: >>> means that the sign bit is shifted with the rest of the number rather than being left alone
                value >>= 7;
                if (value != 0)
                {
                    temp |= 0b10000000;
                }
                return temp;
            } while (value != 0);
        }
    }
}
