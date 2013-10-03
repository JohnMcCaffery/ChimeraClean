using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace Chimera.OpenSim.Packets {
    /// <exclude/>
    public sealed class SetFrustumPacket : Packet {
        /// <exclude/>
        public sealed class FrustumBlock : PacketBlock {
            public Matrix4 ProjectionMatrix;

            public override int Length {
                get {
                    //Matrix (4x4 floats (4bit) + 4 vector3s (3x floats (4bits)) + (1 xint) UUID
                    return (sizeof(float) * 16);
                }
            }

            public FrustumBlock() { }
            public FrustumBlock(byte[] bytes, ref int i) {
                FromBytes(bytes, ref i);
            }

            public override void FromBytes(byte[] bytes, ref int i) {
                try {
                    Vector4 r1 = new Vector4(), r2 = new Vector4(), r3 = new Vector4(), r4 = new Vector4();
                    r1.FromBytes(bytes, i); i += sizeof(float) * 4;
                    r2.FromBytes(bytes, i); i += sizeof(float) * 4;
                    r3.FromBytes(bytes, i); i += sizeof(float) * 4;
                    r4.FromBytes(bytes, i); i += sizeof(float) * 4;

                    ProjectionMatrix.M11 = r1.X;
                    ProjectionMatrix.M12 = r1.Y;
                    ProjectionMatrix.M13 = r1.Z;
                    ProjectionMatrix.M14 = r1.W;

                    ProjectionMatrix.M21 = r2.X;
                    ProjectionMatrix.M22 = r2.Y;
                    ProjectionMatrix.M23 = r2.Z;
                    ProjectionMatrix.M24 = r2.W;

                    ProjectionMatrix.M31 = r3.X;
                    ProjectionMatrix.M32 = r3.Y;
                    ProjectionMatrix.M33 = r3.Z;
                    ProjectionMatrix.M34 = r3.W;

                    ProjectionMatrix.M41 = r4.X;
                    ProjectionMatrix.M42 = r4.Y;
                    ProjectionMatrix.M43 = r4.Z;
                    ProjectionMatrix.M44 = r4.W;
                } catch (Exception) {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i) {
                //Vector4 r1;
                //ProjectionMatrix.UpAxis
                Utils.FloatToBytes(ProjectionMatrix.M11, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M12, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M13, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M14, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M21, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M22, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M23, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M34, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M31, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M32, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M33, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M34, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M41, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M42, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M43, bytes, i); i += sizeof(float);
                Utils.FloatToBytes(ProjectionMatrix.M44, bytes, i); i += sizeof(float);
            }
        }

        public override int Length {
            get {
                int length = 10;
                length += Frustum.Length;
                return length;
            }
        }
        public FrustumBlock Frustum;

        public SetFrustumPacket() {
            HasVariableBlocks = false;
            //Type = PacketType.SetFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 429;
            Header.Reliable = true;
            Frustum = new FrustumBlock();
        }

        public SetFrustumPacket(byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(bytes, ref i, ref packetEnd, null);
        }

        public SetFrustumPacket(Matrix4 matrix)
            : this() {

            Frustum.ProjectionMatrix = matrix;
        }

        override public void FromBytes(byte[] bytes, ref int i, ref int packetEnd, byte[] zeroBuffer) {
            Header.FromBytes(bytes, ref i, ref packetEnd);
            if (Header.Zerocoded && zeroBuffer != null) {
                packetEnd = Helpers.ZeroDecode(bytes, packetEnd + 1, zeroBuffer) - 1;
                bytes = zeroBuffer;
            }
            Frustum.FromBytes(bytes, ref i);
        }

        public SetFrustumPacket(Header head, byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd) {
            Header = header;
            Frustum.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes() {
            int length = 10;
            length += Frustum.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            Frustum.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple() {
            return new byte[][] { ToBytes() };
        }
    }
}
