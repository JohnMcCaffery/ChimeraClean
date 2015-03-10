using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace Chimera.OpenSim.Packets
{
    /// <exclude/>
    public sealed class MuteAudioPacket : Packet
    {
        /// <exclude/>
        public sealed class MuteBlock : PacketBlock
        {
            public bool Mute;

            public override int Length
            {
                get
                {
                    return 1;
                }
            }

            public MuteBlock() { }
            public MuteBlock(byte[] bytes, ref int i)
            {
                FromBytes(bytes, ref i);
            }

            public override void FromBytes(byte[] bytes, ref int i)
            {
                try
                {
                   Mute = (bytes[i++] != 0) ? (bool)true : (bool)false;
                }
                catch (Exception)
                {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i)
            {
                bytes[i++] = (byte)((Mute) ? 1 : 0);
            }

        }

        public override int Length
        {
            get
            {
                int length = 10;
                length += Mute.Length;
                return length;
            }
        }
        public MuteBlock Mute;

        public MuteAudioPacket()
        {
            HasVariableBlocks = false;
            //Type = PacketType.ClearFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 435;
            Header.Reliable = true;
            Mute = new MuteBlock();
        }

        public MuteAudioPacket(bool mute)
            : this()
        {
            Mute.Mute = mute;
        }

        public MuteAudioPacket(byte[] bytes, ref int i)
            : this()
        {
            int packetEnd = bytes.Length - 1;
            FromBytes(bytes, ref i, ref packetEnd, null);
        }

        override public void FromBytes(byte[] bytes, ref int i, ref int packetEnd, byte[] zeroBuffer)
        {
            Header.FromBytes(bytes, ref i, ref packetEnd);
            if (Header.Zerocoded && zeroBuffer != null)
            {
                packetEnd = Helpers.ZeroDecode(bytes, packetEnd + 1, zeroBuffer) - 1;
                bytes = zeroBuffer;
            }
            Mute.FromBytes(bytes, ref i);
        }

        public MuteAudioPacket(Header head, byte[] bytes, ref int i)
            : this()
        {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd)
        {
            Header = header;
            Mute.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes()
        {
            int length = 10;
            length += Mute.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            Mute.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple()
        {
            return new byte[][] { ToBytes() };
        }
    }
}
