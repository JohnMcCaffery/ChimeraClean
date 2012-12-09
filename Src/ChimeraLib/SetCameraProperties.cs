using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace ChimeraLib {

    /// <exclude/>
    public sealed class SetCameraPropertiesPacket : Packet {
        /// <exclude/>
        public sealed class CameraPropertyBlock : PacketBlock {
            public float FrustumOffsetH;
            public float FrustumOffsetV;
            public float FrustumNear;
            public float CameraAngle;
            public float AspectRatio;
            public bool AspectSet = false;
            public bool SetNear = false;

            public override int Length {
                get {
                    return 22;
                }
            }

            public CameraPropertyBlock() { }
            public CameraPropertyBlock(byte[] bytes, ref int i) {
                FromBytes(bytes, ref i);
            }

            public override void FromBytes(byte[] bytes, ref int i) {
                try {
                    FrustumOffsetH = Utils.BytesToFloat(bytes, i); i += 4;
                    FrustumOffsetV = Utils.BytesToFloat(bytes, i); i += 4;
                    FrustumNear = Utils.BytesToFloat(bytes, i); i += 4;
                    CameraAngle = Utils.BytesToFloat(bytes, i); i += 4;
                    AspectRatio = Utils.BytesToFloat(bytes, i); i += 4;
                    AspectSet = bytes[i++] == 0 ? false : true;
                    SetNear = bytes[i++] == 0 ? false : true;
                } catch (Exception) {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i) {
                Utils.FloatToBytes(FrustumOffsetH, bytes, i); i += 4;
                Utils.FloatToBytes(FrustumOffsetV, bytes, i); i += 4;
                Utils.FloatToBytes(FrustumNear, bytes, i); i += 4;
                Utils.FloatToBytes(CameraAngle, bytes, i); i += 4;
                Utils.FloatToBytes(AspectRatio, bytes, i); i += 4;
                bytes[i++] = (byte)(AspectSet ? 1 : 0);
                bytes[i++] = (byte)(SetNear ? 1 : 0);
            }

        }

        public override int Length {
            get {
                int length = 10;
                length += CameraProperty.Length;
                return length;
            }
        }
        public CameraPropertyBlock CameraProperty;

        public SetCameraPropertiesPacket() {
            HasVariableBlocks = false;
            Type = PacketType.SetCameraProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 427;
            Header.Reliable = true;
            CameraProperty = new CameraPropertyBlock();
        }

        public SetCameraPropertiesPacket(byte[] bytes, ref int i)
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
            CameraProperty.FromBytes(bytes, ref i);
        }

        public SetCameraPropertiesPacket(Header head, byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd) {
            Header = header;
            CameraProperty.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes() {
            int length = 10;
            length += CameraProperty.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            CameraProperty.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple() {
            return new byte[][] { ToBytes() };
        }
    }
}
