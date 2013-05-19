using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;

namespace Chimera.OpenSim.Packets {
    /// <exclude/>
    public sealed class SetWindowPacket: Packet {
        public override int Length {
            get {
                int length = 10;
                length += Frustum.Length;
                length += Camera.Length;
                return length;
            }
        }
        public SetFrustumPacket.FrustumBlock Frustum;
        public SetCameraPacket.CameraBlock Camera;

        public SetWindowPacket () {
            HasVariableBlocks = false;
            //Type = PacketType.SetFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 431;
            Header.Reliable = true;
            Frustum = new SetFrustumPacket.FrustumBlock();
            Camera = new SetCameraPacket.CameraBlock();
        }

        public SetWindowPacket(byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(bytes, ref i, ref packetEnd, null);
        }

        public SetWindowPacket(Matrix4 matrix, SetCameraPacket.CameraBlock camera)
            : this() {

            Frustum.ProjectionMatrix = matrix;
            Camera = camera;
        }

        override public void FromBytes(byte[] bytes, ref int i, ref int packetEnd, byte[] zeroBuffer) {
            Header.FromBytes(bytes, ref i, ref packetEnd);
            if (Header.Zerocoded && zeroBuffer != null) {
                packetEnd = Helpers.ZeroDecode(bytes, packetEnd + 1, zeroBuffer) - 1;
                bytes = zeroBuffer;
            }
            Frustum.FromBytes(bytes, ref i);
            Camera.FromBytes(bytes, ref i);
        }

        public SetWindowPacket(Header head, byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd) {
            Header = header;
            Frustum.FromBytes(bytes, ref i);
            Camera.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes() {
            int length = 10;
            length += Frustum.Length;
            length += Camera.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            Frustum.ToBytes(bytes, ref i);
            Camera.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple() {
            return new byte[][] { ToBytes() };
        }
    }
}
