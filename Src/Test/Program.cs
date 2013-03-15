using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using Chimera.Util;

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
            mPointStart = Vector.Create("PointStart", 0f, 0f, 0f);
            mPointDir = Vector.Create("PointDir", 0f, 0f, 0f);

            Init();

            GC.Collect();

            ProcessWrangler.BlockingRunForm(new TestForm(mPlaneTopLeft, mPlaneNormal, mTop, mSide, mPointStart, mPointDir, mIntersection, mW, mH, mX, mY), null, false);
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
            //Calculate a vector that represents the orientation of the top of the coordinator.
            mTop = Nui.scale(Nui.cross(vertical, mPlaneNormal), mW);
            //Calculate a vector that represents the orientation of the side of the coordinator.
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

            Nui.Tick += new ChangeDelegate(Nui_Tick);
        }

        static void Nui_Tick() {
            Console.WriteLine("Tick");
        }
    }
}
