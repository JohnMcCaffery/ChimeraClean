/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;
using UtilLib;

namespace ChimeraLib {
    /// <exclude/>
    public sealed class SetWindowPacket: Packet {
        /// <exclude/>
        public sealed class WindowBlock : PacketBlock {
            public Matrix4 ProjectionMatrix;
            public Vector3 Position;
            public Vector3 PositionDelta;
            public Vector3 LookAt;
            public Vector3 LookAtDelta;
            public uint TickLength;
            public UUID Source;

            public override int Length {
                get {
                    //Matrix (4x4 floats (4bit) + 4 vector3s (3x floats (4bits)) + (1 xint) UUID
                    return (sizeof(float) * 16) + (sizeof(float) * 3 * 4) + sizeof(int) + Source.GetBytes().Length;
                }
            }

            public WindowBlock() { }
            public WindowBlock(byte[] bytes, ref int i) {
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

                    Position.FromBytes(bytes, i); i += 12;
                    PositionDelta.FromBytes(bytes, i); i += 12;
                    LookAt.FromBytes(bytes, i); i += 12;
                    LookAtDelta.FromBytes(bytes, i); i += 12;

                    TickLength = Utils.BytesToUInt(bytes, i); i += sizeof(int);

                    Source.FromBytes(bytes, i);
                } catch (Exception) {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i) {
                //Vector4 r1;
                //ProjectionMatrix.UpAxis
                Utils.FloatToBytes(ProjectionMatrix.M11, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M12, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M13, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M14, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M21, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M22, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M23, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M34, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M31, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M32, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M33, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M34, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M41, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M42, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M43, bytes, i); i += 4;
                Utils.FloatToBytes(ProjectionMatrix.M44, bytes, i); i += 4;

                Position.ToBytes(bytes, i); i += 12;
                PositionDelta.ToBytes(bytes, i); i += 12;
                LookAt.ToBytes(bytes, i); i += 12;
                LookAtDelta.ToBytes(bytes, i); i += 12;

                Utils.UIntToBytes(TickLength, bytes, i); i += sizeof(int);

                Source.ToBytes(bytes, i);
            }
        }

        public override int Length {
            get {
                int length = 10;
                length += Window.Length;
                return length;
            }
        }
        public WindowBlock Window;

        public SetWindowPacket () {
            HasVariableBlocks = false;
            //Type = PacketType.SetFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 427;
            Header.Reliable = true;
            Window = new WindowBlock();
        }

        public SetWindowPacket(byte[] bytes, ref int i)
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
            Window.FromBytes(bytes, ref i);
        }

        public SetWindowPacket(Header head, byte[] bytes, ref int i)
            : this() {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd) {
            Header = header;
            Window.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes() {
            int length = 10;
            length += Window.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            Window.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple() {
            return new byte[][] { ToBytes() };
        }
    }

    /// <exclude/>
    public sealed class ClearWindowPacket : Packet
    {
        /// <exclude/>
        public sealed class ObjectDataBlock : PacketBlock
        {
            public UUID ObjectID;

            public override int Length
            {
                get
                {
                    return 16;
                }
            }

            public ObjectDataBlock() { }
            public ObjectDataBlock(byte[] bytes, ref int i)
            {
                FromBytes(bytes, ref i);
            }

            public override void FromBytes(byte[] bytes, ref int i)
            {
                try
                {
                    ObjectID.FromBytes(bytes, i); i += 16;
                }
                catch (Exception)
                {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i)
            {
                ObjectID.ToBytes(bytes, i); i += 16;
            }

        }

        public override int Length
        {
            get
            {
                int length = 10;
                length += ObjectData.Length;
                return length;
            }
        }
        public ObjectDataBlock ObjectData;

        public ClearWindowPacket()
        {
            HasVariableBlocks = false;
            Type = PacketType.ClearFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 428;
            Header.Reliable = true;
            ObjectData = new ObjectDataBlock();
        }

        public ClearWindowPacket(UUID id)
            : this() {
            ObjectData.ObjectID = id;
        }

        public ClearWindowPacket(byte[] bytes, ref int i) : this()
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
            ObjectData.FromBytes(bytes, ref i);
        }

        public ClearWindowPacket(Header head, byte[] bytes, ref int i): this()
        {
            int packetEnd = bytes.Length - 1;
            FromBytes(head, bytes, ref i, ref packetEnd);
        }

        override public void FromBytes(Header header, byte[] bytes, ref int i, ref int packetEnd)
        {
            Header = header;
            ObjectData.FromBytes(bytes, ref i);
        }

        public override byte[] ToBytes()
        {
            int length = 10;
            length += ObjectData.Length;
            if (Header.AckList != null && Header.AckList.Length > 0) { length += Header.AckList.Length * 4 + 1; }
            byte[] bytes = new byte[length];
            int i = 0;
            Header.ToBytes(bytes, ref i);
            ObjectData.ToBytes(bytes, ref i);
            if (Header.AckList != null && Header.AckList.Length > 0) { Header.AcksToBytes(bytes, ref i); }
            return bytes;
        }

        public override byte[][] ToBytesMultiple()
        {
            return new byte[][] { ToBytes() };
        }
    }

    public static class PacketExtensions {
        public static SetFollowCamPropertiesPacket CreateSetFollowCamPropertiesPacket(this Window window, Vector3 position, Rotation rotation, bool combinePosition = true, bool combineRotation = true) {
            SetFollowCamPropertiesPacket cameraPacket = new SetFollowCamPropertiesPacket();
            cameraPacket.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
            for (int i = 0; i < 22; i++) {
                cameraPacket.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                cameraPacket.CameraProperty[i].Type = i + 1;
            }

            Rotation finalRot = new Rotation(rotation.Pitch, rotation.Yaw);
            if (combineRotation) {
                finalRot.Pitch += window.RotationOffset.Pitch;
                finalRot.Yaw += window.RotationOffset.Yaw;
            }
            if (combinePosition) {
                Vector3 rotatatedLookAt = ((window.EyePosition * new Vector3(1f, -1f, 1f)) / 1000f) * rotation.Quaternion;
                position += rotatatedLookAt;
            }
            Vector3 focus = position + finalRot.LookAtVector;
            cameraPacket.CameraProperty[0].Value = 0;
            cameraPacket.CameraProperty[1].Value = 0f;
            cameraPacket.CameraProperty[2].Value = 0f;
            cameraPacket.CameraProperty[3].Value = 0f;
            cameraPacket.CameraProperty[4].Value = 0f;
            cameraPacket.CameraProperty[5].Value = 0f;
            cameraPacket.CameraProperty[6].Value = 0f;
            cameraPacket.CameraProperty[7].Value = 0f;
            cameraPacket.CameraProperty[8].Value = 0f;
            cameraPacket.CameraProperty[9].Value = 0f;
            cameraPacket.CameraProperty[10].Value = 0f;
            cameraPacket.CameraProperty[11].Value = 1f; //enable
            cameraPacket.CameraProperty[12].Value = 0f;
            cameraPacket.CameraProperty[13].Value = position.X;
            cameraPacket.CameraProperty[14].Value = position.Y;
            cameraPacket.CameraProperty[15].Value = position.Z;
            cameraPacket.CameraProperty[16].Value = 0f;
            cameraPacket.CameraProperty[17].Value = focus.X;
            cameraPacket.CameraProperty[18].Value = focus.Y;
            cameraPacket.CameraProperty[19].Value = focus.Z;
            cameraPacket.CameraProperty[20].Value = 1f;
            cameraPacket.CameraProperty[21].Value = 1f;
            return cameraPacket;
        }

        public static SetWindowPacket CreateWindowPacket(this Window window, 
            Vector3 position, 
            Vector3 positionDelta, 
            Rotation rotation, 
            Vector3 lookAtDelta, 
            uint tickLength) {

            Vector3 upperRight = new Vector3(0f, (float)(window.Width / 2.0), (float)(window.Height / 2.0));
            Vector3 lowerLeft = new Vector3(0f, (float)(window.Width / -2.0), (float)(window.Height / -2.0));
            Vector3 diff = window.ScreenPosition - window.EyePosition;

            diff *= -window.RotationOffset.Quaternion;
            //diff *= window.RotationOffset.Quaternion;

            upperRight += diff;
            lowerLeft += diff;

            //upperRight /= (Math.Abs(diff.X) - .01f);
            //lowerLeft /= (Math.Abs(diff.X) - .01f);
            upperRight /= (float) (diff.X * 10.0);
            lowerLeft /= (float) (diff.X * 10.0);

            float x1 = Math.Min(upperRight.Y, lowerLeft.Y);
            float x2 = Math.Max(upperRight.Y, lowerLeft.Y);
            float y1 = Math.Max(upperRight.Z, lowerLeft.Z);
            float y2 = Math.Min(upperRight.Z, lowerLeft.Z);
            float dn = (diff.Length() / diff.X) * .1f;
            float df = (512f * 100f) * dn;

            /*
            SetFrustumPacket fp = window.CreateFrustumPacket(512f);
            float x1 = fp.Frustum.x1;
            float x2 = fp.Frustum.x2;
            float y1 = fp.Frustum.y1;
            float y2 = fp.Frustum.y2;
            float dn = fp.Frustum.dn;
            float df = fp.Frustum.df;
            */

            Vector3 lookAt = new Rotation(rotation.Pitch + window.RotationOffset.Pitch, rotation.Yaw + window.RotationOffset.Yaw).LookAtVector;

            SetWindowPacket p = new SetWindowPacket();
            p.Window.Position = position;
            p.Window.PositionDelta = positionDelta;
            p.Window.LookAt = lookAt;
            p.Window.LookAtDelta = lookAtDelta;
            p.Window.TickLength = tickLength * 1000;
		    p.Window.ProjectionMatrix = new Matrix4(
    			(2*dn) / (x2-x1),   0,              (x2+x1)/(x2-x1),   0,
    			0,                  (2*dn)/(y1-y2), (y1+y2)/(y1-y2),   0,
    			0,                  0,              -(df+dn)/(df-dn),   -(2.0f*df*dn)/(df-dn),
    			0,                  0,              -1.0f,              0);

            return p;
        }
    }
}

            /*
            float x1 = p.Frustum.x1;
            float x2 = p.Frustum.x2;
            float y1 = p.Frustum.y1;
            float y2 = p.Frustum.y2;
            float dn = p.Frustum.dn;
            float df = p.Frustum.df;

            double fovH = Math.Atan2(x2 - x1, 2) * 2.0 * Rotation.RAD2DEG;
            double fovV = Math.Atan2(y1 - y2, 2) * 2.0 * Rotation.RAD2DEG;

            double frustumOffsetH = (x2 + x1) / (x2 - x1) * window.Height;
            double frustumOffsetV = (y1 + y2) / (y1 - y2) * window.Height;

            Matrix4 projectionPaper = new Matrix4(
                (2*dn)/(x2-x1),     0,              (x2+x1)/(x2-x1),    0,
                0,                  (2*dn)/(y1-y2), (y1+y2)/(y1-y2),    0,
                0,                  0,              -(df+dn)/(df-dn),   -(2*df*dn)/(df-dn),
                0,                  0,              -1,                 0);

            float f = (float)(1.0 / Math.Tan(window.FieldOfView / 2.0));
            float aspect = (float) window.AspectRatio;

            float hOffset = (float)((2 * window.FrustumOffsetH) / window.Width);
            float vOffset = (float)((2 * window.FrustumOffsetV) / window.Height);

            Matrix4 projectionMine = new Matrix4(
                f/aspect,     0,    hOffset,            0,
                0,            f,    vOffset,            0,
                0,            0,    -(df+dn)/(df-dn),   -(2*df*dn)/(df-dn),
                0,            0,    -1,                 0);
            */
