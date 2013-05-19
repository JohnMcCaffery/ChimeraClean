using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace Chimera.OpenSim.Packets {
    /// <exclude/>
    public sealed class RemoteControlPacket : Packet {
        /// <exclude/>
        public sealed class DeltaBlock : PacketBlock {
            public Vector3 Position;
            public float Pitch;
            public float Yaw;

            public override int Length {
                get {
                    return (sizeof(float) * 5);
                }
            }

            public DeltaBlock() { }
            public DeltaBlock(byte[] bytes, ref int i) {
                FromBytes(bytes, ref i);
            }

            public override void FromBytes(byte[] bytes, ref int i) {
                try {
                    Position.FromBytes(bytes, i); i += sizeof(float) * 3;

                    Pitch = Utils.BytesToFloat(bytes, i); i += sizeof(float);
                    Yaw = Utils.BytesToFloat(bytes, i); i += sizeof(float);
                } catch (Exception) {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i) {
                Position.ToBytes(bytes, i); i += sizeof(float) * 3;

                Utils.FloatToBytes(Pitch, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(Yaw, bytes, i); i += sizeof(float);
            }
        }

        public override int Length {
            get {
                int length = 10;
                length += Delta.Length;
                return length;
            }
        }
        public DeltaBlock Delta;

        public RemoteControlPacket() {
            HasVariableBlocks = false;
            //Type = PacketType.SetFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 432;
            Header.Reliable = true;
            Delta = new DeltaBlock();
        }

        public RemoteControlPacket(byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(bytes, ref i, ref packetEnd, null);
        }

        override public void FromBytes(byte[] bytes, ref int i, ref int packetEnd, byte[] zeroBuffer) {
            Header.FromBytes(bytes, ref i, ref packetEnd);
            if (Header.Zerocoded && zeroBuffer != null) {
                packetEnd = Helpers.ZeroDecode(bytes, packetEnd + 1, zeroBuffer) - 1;
                bytes = zeroBuffer;
            }
            Delta.FromBytes(bytes, ref i);
        }

        public RemoteControlPacket(Header head, byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd) {
            Header = header;
            Delta.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes() {
            int length = 10;
            length += Delta.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            Delta.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple() {
            return new byte[][] { ToBytes() };
        }
    }
}
