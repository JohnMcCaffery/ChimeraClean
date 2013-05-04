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
using NuiLibDotNet;
using Chimera.Util;
using System.Windows.Forms;
using Chimera;
using OpenMetaverse;
using Chimera.Kinect;
using Nini.Config;
using System.Configuration;

namespace Test {
    public class Program {
        static Vector mPointStart;
        static Vector mPointDir;
        static Vector mPlaneTopLeft;
        static Vector mPlaneNormal;
        static Scalar mW;
        static Scalar mH;
        static Vector mIntersection;
        static Vector mTop;
        static Vector mSide;
        static Scalar mX;
        static Scalar mY;

        [STAThread]
        public static void Main(string[] args) {
            DotNetConfigSource dotnet = new DotNetConfigSource();
            ArgvConfigSource arg = new ArgvConfigSource(args);

            dotnet.Merge(arg);

            arg.AddSwitch("Test", "Test", "t");

            string test = arg.Configs["Test"].Get("Test");
            string test2 = dotnet.Configs["appSettings"].Get("Test");

            Console.WriteLine(test);

            /*
            mPointStart = Vector.Create("PointStart", 0f, 0f, 0f);
            mPointDir = Vector.Create("PointDir", 0f, 0f, 0f);

            //Init();
            Nui.Init();
            Nui.SetAutoPoll(true);

            GC.Collect();


            Window window = new Window("Test Window");
            window.Width = 2000;
            window.Height = 1500;
            window.TopLeft = new Vector3(0f, -1000f, 750f);

            IKinectCursor cursor = new PointCursor();
            //IKinectCursor cursor = new SimpleCursor();

            //Form = new TestForm(mPlaneTopLeft, mPlaneNormal, mTop, mSide, mPointStart, mPointDir, mIntersection, mW, mH, mX, mY);
            //Form form = new KinectMovementForm();
            //form.Begin();
            Form form = new KinectCursorForm(cursor, window);
            ProcessWrangler.BlockingRunForm(form, null);
            */
        }

        private static void Init() {
			Nui.Init();
            Nui.SetAutoPoll(true);
            Vector pointEnd = Nui.joint(Nui.Hand_Right);
            mPointStart = Nui.joint(Nui.Shoulder_Right);
            mPointDir = mPointStart - pointEnd;

            mPlaneTopLeft = Vector.Create("PlanePoint", 1f, 1f, 0f);
            mPlaneNormal = Vector.Create("PlaneNormal", 0f, 0f, -1f);
            mW = Scalar.Create("W", 2f);
            mH = Scalar.Create("H", 2f);

            Vector vertical = Vector.Create(0f, 1f, 0f); // Vertical
            //Calculate the intersection of the plane defined by the point mPlaneTopLeft and the normal mPlaneNormal and the line defined by the point mPointStart and the direction mPointDir.
            mIntersection = Nui.intersect(mPlaneTopLeft, Nui.normalize(mPlaneNormal), mPointStart, Nui.normalize(mPointDir));
            //Calculate a vector that represents the orientation of the top of the input.
            mTop = Nui.scale(Nui.cross(vertical, mPlaneNormal), mW);
            //Calculate a vector that represents the orientation of the side of the input.
            mSide = Nui.scale(Nui.cross(mPlaneNormal, mTop), mH);

            //Calculate the vector (running along the plane) between the top left corner and the point of intersection.
            Vector diff = mIntersection - mPlaneTopLeft;

            //Project the diff line onto the top and side vectors to get x and y values.
            mX = Nui.project(diff, mTop);
            mY = Nui.project(diff, mSide);

            mX.OnChange += () => {
                if (mX.Value > 0 && mX.Value < mW.Value)
                    Console.WriteLine(mX.Value);
            };

            mPointStart.Set(1f, 0f, 0f);
        }
    }
}
