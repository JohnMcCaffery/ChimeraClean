using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace Chimera.OpenSim.Packets {
    /// <exclude/>
    public sealed class SetCameraPacket: Packet {
        /// <exclude/>
        public sealed class CameraBlock : PacketBlock {
            public Vector3 Position;
            public Vector3 PositionDelta;
            public Vector3 LookAt;
            public Vector3 LookAtDelta;
            public uint TickLength;
            public UUID Source;

            public override int Length {
                get {
                    //Matrix (4x4 floats (4bit) + 4 vector3s (3x floats (4bits)) + (1 xint) UUID
                    return (sizeof(float) * 3 * 4) + sizeof(int) + Source.GetBytes().Length;
                }
            }

            public CameraBlock() { }
            public CameraBlock(byte[] bytes, ref int i) {
                FromBytes(bytes, ref i);
            }

            public override void FromBytes(byte[] bytes, ref int i) {
                try {
                    Position.FromBytes(bytes, i); i += sizeof(float) * 3;
                    PositionDelta.FromBytes(bytes, i); i += sizeof(float) * 3;
                    LookAt.FromBytes(bytes, i); i += sizeof(float) * 3;
                    LookAtDelta.FromBytes(bytes, i); i += sizeof(float) * 3;

                    TickLength = Utils.BytesToUInt(bytes, i); i += sizeof(int);

                    Source.FromBytes(bytes, i);
                } catch (Exception) {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i) {
                Position.ToBytes(bytes, i); i += sizeof(float) * 3;
                PositionDelta.ToBytes(bytes, i); i += sizeof(float) * 3;
                LookAt.ToBytes(bytes, i); i += sizeof(float) * 3;
                LookAtDelta.ToBytes(bytes, i); i += sizeof(float) * 3;

                Utils.UIntToBytes(TickLength, bytes, i); i += sizeof(int);

                Source.ToBytes(bytes, i);
            }
        }

        public override int Length {
            get {
                int length = 10;
                length += Camera.Length;
                return length;
            }
        }
        public CameraBlock Camera;

        public SetCameraPacket () {
            HasVariableBlocks = false;
            //Type = PacketType.SetFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 427;
            Header.Reliable = true;
            Camera = new CameraBlock();
        }

        public SetCameraPacket(CameraBlock camera) : this() {
            Camera = camera;
        }

        public SetCameraPacket(byte[] bytes, ref int i)
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
            Camera.FromBytes(bytes, ref i);
        }

        public SetCameraPacket(Header head, byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd) {
            Header = header;
            Camera.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes() {
            int length = 10;
            length += Camera.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            Camera.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple() {
            return new byte[][] { ToBytes() };
        }
    }
}
