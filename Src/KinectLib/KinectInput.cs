using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiLibDotNet;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.Kinect {
    public class KinectInput : IInput {
        private readonly List<WindowInput> mWindowInputs = new List<WindowInput>();

        private Coordinator mCoordinator;
        private bool mEnabled;
        private bool mPointEnabled;
        private bool mMoveEnabled;
        private bool mPreviouslyEnabled;
        private Vector mPointStart = Vector.Create(0f, 0f, 0f);
        private Vector mPointDir = Vector.Create(0f, 0f, 0f);
        private Vector3 mKinectPosition;
        private Rotation mKinectOrientation;

        public event Action<Vector3> PositionChanged;
        public event Action<Quaternion> OrientationChanged;
        public event Action<Vector, Vector> VectorsAssigned;

        /// <summary>
        /// Get the window input associated with a specific window.
        /// </summary>
        /// <param name="window">The name of the window to get the input for.</param>
        /// <returns>The WindowInput object which is calculating whether the user is pointing at the specified window.</returns>
        public WindowInput this[string window] {
            get { return mWindowInputs.FirstOrDefault(i => i.Window.Name.Equals(window); }
        }
        /// <summary>
        /// All the window inputs this input associates with the windows the system renders to.
        /// </summary>
        public WindowInput[] WindowInputs {
            get { return mWindowInputs.ToArray(); }
        }

        public Vector PointStart {
            get { return mPointStart; }
        }

        public Vector PointDir {
            get { return mPointDir; }
        }

        public Vector3 Position {
            get { return mKinectPosition; }
        }

        public Rotation Orientation {
            get { return mKinectOrientation; }
        }

        #region IInput Members

        public UserControl ControlPanel {
            get { throw new NotImplementedException(); }
        }

        public string Name {
            get { return "Kinect"; }
        }

        public bool Enabled {
            get { return mMoveEnabled; }
            set {
                if (value && !mPreviouslyEnabled) {
                    Nui.Init();
                    Nui.SetAutoPoll(true);
                    Vector pointEnd = Nui.joint(Nui.Hand_Right);
                    mPointStart = Nui.joint(Nui.Shoulder_Right);
                    mPointDir = mPointStart - pointEnd;
                    mPreviouslyEnabled = true;

                    if (VectorsAssigned != null)
                        VectorsAssigned(mPointStart, mPointDir);
                }
                mMoveEnabled = value;
            }
        }

        public string[] ConfigSwitches {
            get { throw new NotImplementedException(); }
        }

        public string State {
            get { return "Kinect State"; }
        }

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;

            foreach(var window in mCoordinator.Windows)
                mWindowInputs.Add(new WindowInput(this, window));
        }

        public void Close() {
            Nui.Close();
        }

        public void Draw(Perspective perspective, System.Drawing.Graphics graphics) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
