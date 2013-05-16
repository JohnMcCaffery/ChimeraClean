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
using Chimera.Util;
using System.IO;
using OpenMetaverse;
using Chimera.Plugins;

namespace Chimera.Config {
    public class ProjectorConfig : ConfigFolderBase {
        public string RoomFile;
        public Vector3 RoomAnchor;

        public Vector3 ProjectorPosition;
        public double ProjectorPitch;
        public double ProjectorYaw;
        public float ThrowRatio;
        public float VOffset;
        public AspectRatio AspectRatio = AspectRatio.SixteenNine;
        public AspectRatio NativeAspectRatio = AspectRatio.SixteenNine;
        public bool Draw;
        public bool DrawRoom;
        public bool DrawLabels;
        public bool AutoUpdate;
        public bool UpsideDown;
        public float WallDistance;
        public bool ConfigureWindow;

        private string mWindow = "MainWindow";

        public ProjectorConfig() : base ("Windows") { }

        public ProjectorConfig(string window, params string[] args)
            : base(window, "Windows", args) {
            mWindow = window;
        }

        public override string Group {
            get { return "Window"; }
        }

        protected override void InitConfig() {
            RoomFile = Get(true, "RoomFile", null, "The file containing the layout for the room the projector is in.");
            RoomAnchor = GetV(true, "RoomAnchor", Vector3.Zero, "The anchor point for the room. All room position values will be offset by this in relation to the eye position.");

            string projectorSection = Section + "Projector";

            ProjectorPosition = GetV(projectorSection, "Position", new Vector3(0f, 1000f, -30f), "Where the projector is, relative to the Room Anchor.");
            ProjectorPitch = Get(projectorSection, "Pitch", 5.0, "The pitch the projector is set at.");
            ProjectorYaw = Get(projectorSection, "Yaw", 0.0, "The yaw the projector is set at.");
            ThrowRatio = Get(projectorSection, "ThrowRatio", 1.7f, "The throw ratio of the projector. Throw ratio is the screen distance/screenWidth");
            WallDistance = Get(projectorSection, "WallDistance", 2000f, "How far away from the projector the wall is.");
            VOffset = Get(projectorSection, "VOffset", .09f, "How for that image is shifted up above the level of the projector.");
            Draw = Get(projectorSection, "Draw", true, "Whether to draw the projector on the window diagrams.");
            DrawRoom = Get(projectorSection, "DrawRoom", true, "Whether to draw the room on the window diagrams.");
            DrawLabels = Get(projectorSection, "DrawLabels", true, "Whether to draw labels on the window diagrams.");
            AutoUpdate = Get(projectorSection, "AutoUpdate", false, "Whether to automatically update the screen size based on the projector position.");
            UpsideDown = Get(projectorSection, "UpsideDown", true, "Whether the projector is mounted upside down.");
            ConfigureWindow = Get(projectorSection, "ConfigureWindow", true, "If true then changing the projector will update the window. False then changing the window will update the projector.");

            string aspectRatioStr = Get(mWindow + "Projector", "AspectRatio", "9:16", "Aspect ratio the projector is set to.");
            string nativeAspectRatioStr = Get(mWindow + "Projector", "AspectRatio", "9:16", "Native aspect ratio the projector supports.");
            Enum.TryParse(aspectRatioStr, out AspectRatio);
            Enum.TryParse(nativeAspectRatioStr, out NativeAspectRatio);
        }
    }
}
