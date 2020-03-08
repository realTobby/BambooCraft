using BambooCraft.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Packets
{
    public class PacketHandler
    {
        private Logging myLogger = new Logging();

        public byte[] BufferedData = new byte[4096];
        private int _lastByte;
        public int Size = 0;

        public bool IsValidPacket = false;
        public int lastByteIndex = 0;

        public List<byte> responseData = new List<byte>();

		private readonly List<byte> _bffr = new List<byte>();

		internal void ReadPacket(byte[] receivedData)
        {
			IsValidPacket = false;
			BufferedData = receivedData;
            // TODO READ PACKET HERE <---------->

            var packetSize = ReadVarInt();
            var packetID = ReadVarInt();

            myLogger.Log(Severity.Packet, "Reading packet...");
            myLogger.Log(Severity.Packet, "Packet: " + packetID.ToString("X2") + " Size: " + packetSize);

            switch(packetID)
            {
                case 0:
                    myLogger.Log(Severity.Packet, "Handshake-Packet!");

                    var protocol = ReadVarInt();
                    string host = ReadString();
                    ushort port = ReadUShort();
                    var nextState = ReadVarInt();

                    
                    IsValidPacket = true;

                    switch(nextState)
                    {
                        case 1:
                            responseData.Clear();
                            responseData = new List<byte>();
                            // status request
                            myLogger.Log(Severity.Packet, "Building PingPayload...");
                            PingPayload pl = new PingPayload();
                            VersionPayload vpl = new VersionPayload();
                            vpl.name = "1.15.2";
                            vpl.protocol = 578;
                            pl.vpl = vpl;

                            PlayerPayload ppl = new PlayerPayload();
                            ppl.max = 10;
                            ppl.online = 0;
                            ppl.sample = new List<Player>();
                            pl.ppl = ppl;

                            DescriptionPayload dpl = new DescriptionPayload();
                            dpl.text = "Test";
                            pl.dpl = dpl;

                            pl.favicon = "data:image/png;base64,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";

                            string json = JsonConvert.SerializeObject(pl);
                            byte[] packetData = Encoding.UTF8.GetBytes(json);
                            Utils.HexDump.DumpHex(packetData);

							WriteVarInt(packetData.Length + 2);
							WriteVarInt(0);
							WriteString(json);


                            break;
                        case 2:
							// login request
							IsValidPacket = false;
							break;
                    }
                    break;
            }
            System.IO.File.WriteAllText("hexdump.txt", Utils.HexDump.DumpHex(responseData.ToArray()));

        }

		public void SetDataSize(int size)
		{
			Array.Resize(ref BufferedData, size);
			Size = size;
		}

		public void Dispose()
		{
			BufferedData = null;
			_lastByte = 0;
		}

		#region Reader

		public int ReadByte()
		{
				if (BufferedData != null)
				{
					var returnData = BufferedData[_lastByte];
					_lastByte++;
					return returnData;
				}
			
			return -1;
		}

		public byte[] Read(int length)
		{
			var buffered = new byte[length];
			Array.Copy(BufferedData, _lastByte, buffered, 0, length);
			_lastByte += length;
			return buffered;
		}


		public int ReadInt()
		{
			var dat = Read(4);
			var value = BitConverter.ToInt32(dat, 0);
			return IPAddress.NetworkToHostOrder(value);
		}

		public float ReadFloat()
		{
			var almost = Read(4);
			var f = BitConverter.ToSingle(almost, 0);
			return NetworkToHostOrder(f);
		}

		public bool ReadBool()
		{
			var answer = ReadByte();
			if (answer == 1)
				return true;
			return false;
		}

		public double ReadDouble()
		{
			var almostValue = Read(8);
			return NetworkToHostOrder(almostValue);
		}

		public int ReadVarInt()
		{
			var value = 0;
			var size = 0;
			int b;
			while (((b = ReadByte()) & 0x80) == 0x80)
			{
				value |= (b & 0x7F) << (size++ * 7);
				if (size > 5)
				{
					throw new Exception("VarInt too long. Hehe that's punny.");
				}
			}
			return value | ((b & 0x7F) << (size * 7));
		}

		public long ReadVarLong()
		{
			var value = 0;
			var size = 0;
			int b;
			while (((b = ReadByte()) & 0x80) == 0x80)
			{
				value |= (b & 0x7F) << (size++ * 7);
				if (size > 10)
				{
					throw new Exception("VarLong too long. That's what she said.");
				}
			}
			return value | ((b & 0x7F) << (size * 7));
		}

		public short ReadShort()
		{
			var da = Read(2);
			var d = BitConverter.ToInt16(da, 0);
			return IPAddress.NetworkToHostOrder(d);
		}

		public ushort ReadUShort()
		{
			var da = Read(2);
			return NetworkToHostOrder(BitConverter.ToUInt16(da, 0));
		}

		public ushort[] ReadUShort(int count)
		{
			var us = new ushort[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToUInt16(da, 0);
				us[i] = d;
			}
			return NetworkToHostOrder(us);
		}

		public ushort[] ReadUShortLocal(int count)
		{
			var us = new ushort[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToUInt16(da, 0);
				us[i] = d;
			}
			return us;
		}

		public short[] ReadShortLocal(int count)
		{
			var us = new short[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToInt16(da, 0);
				us[i] = d;
			}
			return us;
		}

		public string ReadString()
		{
			var length = ReadVarInt();
			var stringValue = Read(length);

			return Encoding.UTF8.GetString(stringValue);
		}

		public long ReadLong()
		{
			var l = Read(8);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(l, 0));
		}

		//public Vector3 ReadPosition()
		//{
		//	var val = ReadLong();
		//	var x = Convert.ToDouble(val >> 38);
		//	var y = Convert.ToDouble((val >> 26) & 0xFFF);
		//	var z = Convert.ToDouble(val << 38 >> 38);
		//	return new Vector3(x, y, z);
		//}

		private double NetworkToHostOrder(byte[] data)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(data);
			}
			return BitConverter.ToDouble(data, 0);
		}

		private float NetworkToHostOrder(float network)
		{
			var bytes = BitConverter.GetBytes(network);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return BitConverter.ToSingle(bytes, 0);
		}

		private ushort[] NetworkToHostOrder(ushort[] network)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(network);
			return network;
		}

		private ushort NetworkToHostOrder(ushort network)
		{
			var net = BitConverter.GetBytes(network);
			if (BitConverter.IsLittleEndian)
				Array.Reverse(net);
			return BitConverter.ToUInt16(net, 0);
		}

		public byte[] ExportWriter
		{
			get { return _bffr.ToArray(); }
		}

		

		public void Write(byte[] data, int offset, int length)
		{
			for (var i = 0; i < length; i++)
			{
				_bffr.Add(data[i + offset]);
			}
		}

		public void Write(byte[] data)
		{
			foreach (var i in data)
			{
				_bffr.Add(i);
			}
		}

		//public void WritePosition(Vector3 position)
		//{
		//	var x = Convert.ToInt64(position.X);
		//	var y = Convert.ToInt64(position.Y);
		//	var z = Convert.ToInt64(position.Z);
		//	var toSend = ((x & 0x3FFFFFF) << 38) | ((y & 0xFFF) << 26) | (z & 0x3FFFFFF);
		//	WriteLong(toSend);
		//}

		public void WriteVarInt(int integer)
		{
			while ((integer & -128) != 0)
			{
				_bffr.Add((byte)(integer & 127 | 128));
				integer = (int)(((uint)integer) >> 7);
			}
			_bffr.Add((byte)integer);
		}

		public void WriteVarLong(long i)
		{
			var fuck = i;
			while ((fuck & ~0x7F) != 0)
			{
				_bffr.Add((byte)((fuck & 0x7F) | 0x80));
				fuck >>= 7;
			}
			_bffr.Add((byte)fuck);
		}

		public void WriteInt(int data)
		{
			var buffer = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data));
			Write(buffer);
		}

		public void WriteString(string data)
		{
			var stringData = Encoding.UTF8.GetBytes(data);
			WriteVarInt(stringData.Length);
			Write(stringData);
		}

		public void WriteShort(short data)
		{
			var shortData = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data));
			Write(shortData);
		}

		public void WriteUShort(ushort data)
		{
			var uShortData = BitConverter.GetBytes(data);
			Write(uShortData);
		}

		public void WriteByte(byte data)
		{
			_bffr.Add(data);
		}

		public void WriteBool(bool data)
		{
			Write(BitConverter.GetBytes(data));
		}

		public void WriteDouble(double data)
		{
			Write(HostToNetworkOrder(data));
		}

		public void WriteFloat(float data)
		{
			Write(HostToNetworkOrder(data));
		}

		public void WriteLong(long data)
		{
			Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data)));
		}

		public void WriteUuid(Guid uuid)
		{
			var guid = uuid.ToByteArray();
			var long1 = new byte[8];
			var long2 = new byte[8];
			Array.Copy(guid, 0, long1, 0, 8);
			Array.Copy(guid, 8, long2, 0, 8);
			Write(long1);
			Write(long2);
		}

		private byte[] GetVarIntBytes(int integer)
		{
			List<Byte> bytes = new List<byte>();
			while ((integer & -128) != 0)
			{
				bytes.Add((byte)(integer & 127 | 128));
				integer = (int)(((uint)integer) >> 7);
			}
			bytes.Add((byte)integer);
			return bytes.ToArray();
		}

		private byte[] HostToNetworkOrder(double d)
		{
			var data = BitConverter.GetBytes(d);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(data);

			return data;
		}

		private byte[] HostToNetworkOrder(float host)
		{
			var bytes = BitConverter.GetBytes(host);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return bytes;
		}

		public byte[] GetResponse()
		{
			System.IO.File.WriteAllText("hexdump.txt", Utils.HexDump.DumpHex(_bffr.ToArray()));
			return _bffr.ToArray();
		}
	}
}
#endregion