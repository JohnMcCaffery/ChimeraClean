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
            //Type = PacketType.SetCameraProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 427;
            Header.Reliable = true;
            CameraProperty = new CameraPropertyBlock();
        }

        public SetCameraPropertiesPacket(bool enable) : this() {
            if (!enable) {
                CameraProperty.FrustumOffsetH = 0f;
                CameraProperty.FrustumOffsetV = 0f;
                CameraProperty.CameraAngle = 1f;
                CameraProperty.AspectSet = false;
                CameraProperty.SetNear = false;
            }
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

    /// <exclude/>
    public sealed class SetFrustumPacket: Packet {
        /// <exclude/>
        public sealed class FrustumBlock : PacketBlock {
            public float x1;
            public float x2;
            public float y1;
            public float y2;
            public float dn;
            public float df;
            public bool ControlFrustum = false;

            public override int Length {
                get {
                    return 25;
                }
            }

            public FrustumBlock() { }
            public FrustumBlock(byte[] bytes, ref int i) {
                FromBytes(bytes, ref i);
            }

            public override void FromBytes(byte[] bytes, ref int i) {
                try {
                    x1 = Utils.BytesToFloat(bytes, i); i += 4;
                    x2 = Utils.BytesToFloat(bytes, i); i += 4;
                    y1 = Utils.BytesToFloat(bytes, i); i += 4;
                    y2 = Utils.BytesToFloat(bytes, i); i += 4;
                    dn = Utils.BytesToFloat(bytes, i); i += 4;
                    df = Utils.BytesToFloat(bytes, i); i += 4;
                    ControlFrustum = bytes[i++] == 0 ? false : true;
                } catch (Exception) {
                    throw new MalformedDataException();
                }
            }

            public override void ToBytes(byte[] bytes, ref int i) {
                Utils.FloatToBytes(x1, bytes, i); i += 4;
                Utils.FloatToBytes(x2, bytes, i); i += 4;
                Utils.FloatToBytes(y1, bytes, i); i += 4;
                Utils.FloatToBytes(y2, bytes, i); i += 4;
                Utils.FloatToBytes(dn, bytes, i); i += 4;
                Utils.FloatToBytes(df, bytes, i); i += 4;
                bytes[i++] = (byte)(ControlFrustum ? 1 : 0);
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

        public SetFrustumPacket () {
            HasVariableBlocks = false;
            //Type = PacketType.SetFollowCamProperties;
            Header = new Header();
            Header.Frequency = PacketFrequency.Low;
            Header.ID = 428;
            Header.Reliable = true;
            Frustum = new FrustumBlock();
        }

        public SetFrustumPacket(bool enable)
            : this() {

            Frustum.ControlFrustum = enable;
        }

        public SetFrustumPacket(byte[] bytes, ref int i)
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
            Vector3 lookAt = position + finalRot.LookAtVector;
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
            cameraPacket.CameraProperty[17].Value = lookAt.X;
            cameraPacket.CameraProperty[18].Value = lookAt.Y;
            cameraPacket.CameraProperty[19].Value = lookAt.Z;
            cameraPacket.CameraProperty[20].Value = 1f;
            cameraPacket.CameraProperty[21].Value = 1f;
            return cameraPacket;
        }

        public static SetFrustumPacket CreateFrustumPacket(this Window window, float vanishingPoint) {
            Vector3 upperRight = new Vector3(0f, (float)(window.Width / 2.0), (float)(window.Height / 2.0));
            Vector3 lowerLeft = new Vector3(0f, (float)(window.Width / -2.0), (float)(window.Height / -2.0));
            Vector3 diff = window.ScreenPosition - window.EyePosition;

            diff *= -window.RotationOffset.Quaternion;

            upperRight += diff;
            lowerLeft += diff;

            //upperRight /= (Math.Abs(diff.X) - .01f);
            //lowerLeft /= (Math.Abs(diff.X) - .01f);
            upperRight /= (float) (diff.X * 10.0);
            lowerLeft /= (float) (diff.X * 10.0);

            SetFrustumPacket p = new SetFrustumPacket();
            p.Frustum.x1 = Math.Min(upperRight.Y, lowerLeft.Y);
            p.Frustum.x2 = Math.Max(upperRight.Y, lowerLeft.Y);
            p.Frustum.y1 = Math.Max(upperRight.Z, lowerLeft.Z);
            p.Frustum.y2 = Math.Min(upperRight.Z, lowerLeft.Z);
            p.Frustum.dn = (diff.Length() / diff.X) * .1f;
            p.Frustum.df = (vanishingPoint * 100f) * p.Frustum.dn;
            p.Frustum.ControlFrustum = true;

            return p;
        }

        public static SetCameraPropertiesPacket CreateCameraPacket(this Window window, bool updateFrustum = true, bool updateFoV = true, bool updateAspectRatio = true) {
            SetCameraPropertiesPacket packet = new SetCameraPropertiesPacket();
            packet.CameraProperty = new SetCameraPropertiesPacket.CameraPropertyBlock();
            packet.CameraProperty.FrustumOffsetH = 0f;
            packet.CameraProperty.FrustumOffsetV = 0f;
            packet.CameraProperty.CameraAngle = 1f;

            packet.CameraProperty.AspectRatio = (float)(window.Width / window.Height);
            packet.CameraProperty.AspectSet = updateAspectRatio;

            packet.CameraProperty.FrustumNear = .01f;
            packet.CameraProperty.SetNear = false;

            if (updateFrustum) {
                packet.CameraProperty.FrustumOffsetH = (float)((2 * window.FrustumOffsetH) / window.Width);
                packet.CameraProperty.FrustumOffsetV = (float)((2 * window.FrustumOffsetV) / window.Height);
            }
            if (updateFoV)
                packet.CameraProperty.CameraAngle = (float)window.FieldOfView;

            return packet;
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
