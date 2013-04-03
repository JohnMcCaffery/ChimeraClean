using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using System.Windows.Forms;
using OpenMetaverse;
using System.Drawing;

namespace Chimera.Inputs {
    public class DeltaBasedInput : ISystemInput {
        private IDeltaInput mInput;
        private Coordinator mCoordinator;
        private bool mEnabled;

        public DeltaBasedInput(IDeltaInput input) {
            mInput = input;

            mInput.Change += new Action<IDeltaInput>(mInput_Change);
        }

        void mInput_Change(IDeltaInput input) {
            Vector3 move = mInput.PositionDelta;

            //TODO - handle keyboard rotation
            if (mInput.Enabled && move != Vector3.Zero || mInput.OrientationDelta.Pitch != 0.0 || mInput.OrientationDelta.Yaw != 0.0) {
                float fly = move.Z;
                move.Z = 0f;
                move *= mCoordinator.Orientation.Quaternion;
                move.Z += fly;

                Vector3 pos = mCoordinator.Position + move;
                Rotation orientation = mCoordinator.Orientation + mInput.OrientationDelta;
                mCoordinator.Update(pos, move, orientation, mInput.OrientationDelta);
            }
        }

        #region ISystemInput Members

        public event Action<IInput, bool> EnabledChanged;

        public virtual UserControl ControlPanel {
            get { return mInput.ControlPanel; }
        }

        public virtual string Name {
            get { return mInput.Name; }
        }

        public virtual bool Enabled {
            get { return mInput.Enabled; }
            set { 
                mInput.Enabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public virtual ConfigBase Config {
            get { return mInput.Config; }
        }

        public virtual string State {
            get { return mInput.State; }
        }

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            mInput.Init(coordinator);
        }

        public void Close() {
            mInput.Close();
        }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) {
            mInput.Draw(to2D, graphics, redraw);
        }

        #endregion
    }
}
