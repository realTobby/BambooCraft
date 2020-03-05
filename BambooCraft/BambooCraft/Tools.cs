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
        public static int ReadVarInt(byte data)
        {
            int numRead = 0;
            int result = 0;
            do
            {
                int value = (data & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((data & 0b10000000) != 0);

            return result;
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
