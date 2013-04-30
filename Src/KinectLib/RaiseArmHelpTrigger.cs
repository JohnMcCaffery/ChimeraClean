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
using Chimera.Kinect.Interfaces;
using System.Windows.Forms;
using Chimera.Kinect.GUI;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;

namespace Chimera.Kinect {
    public class RaiseArmHelpTrigger : ISystemPlugin, IHelpTrigger {
        private RaiseArmHelpTriggerPanel mPanel;
        private bool mEnabled = true;

        private Vector mArmR;
        private Vector mArmL;
        private Scalar mAngleR;
        private Scalar mAngleL;
        private Condition mTriggerR;
        private Condition mTriggerL;
        private Condition mTrigger;
        private Scalar mAngleThreshold;
        private Scalar mHeightThreshold;

        public Vector ArmR { get { return mArmR; } }
        public Vector ArmL { get { return mArmL; } }
        public Scalar AngleR { get { return mAngleR; } }
        public Scalar AngleL { get { return mAngleL; } }
        public Condition TriggerR { get { return mTriggerR; } }
        public Condition TriggerL { get { return mTriggerL; } }
        public Condition Trigger { get { return mTrigger; } }
        public Scalar AngleThreshold { get { return mAngleThreshold; } }
        public Scalar HeightThreshold { get { return mHeightThreshold; } }

        private Coordinator mCoordinator;

        public event Action<IHelpTrigger> Triggered;

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new RaiseArmHelpTriggerPanel(this);
                return mPanel;
            }
        }

        public string Name { get { return "RaiseArmHelp"; } }

        public bool Enabled {
            get { return mEnabled; }
            set { 
                mEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;

            mHeightThreshold = Scalar.Create(.4f);
            mAngleThreshold = Scalar.Create(.48f);

            Vector up = Vector.Create(0f, 1f, 0f);
            mArmR = Nui.joint(Nui.Hand_Right) - Nui.joint(Nui.Shoulder_Right);
            mArmL = Nui.joint(Nui.Hand_Left) - Nui.joint(Nui.Shoulder_Left);
            mAngleR = Nui.dot(up, mArmR);
            mAngleL = Nui.dot(up, mArmL);

            mTriggerR = C.And(Nui.y(mArmR) > mHeightThreshold, mAngleR > mAngleThreshold);
            mTriggerL = C.And(Nui.y(mArmL) > mHeightThreshold, mAngleL > mAngleThreshold);
            mTrigger = C.Or(mTriggerR, mTriggerL);

            mTrigger.OnChange += new ChangeDelegate(mTrigger_OnChange);
        }

        void mTrigger_OnChange() {
            if (mEnabled && mTrigger.Value) {
                mCoordinator.StateManager.TriggerCustom("Help");
                if (Triggered != null)
                    Triggered(this);
            }

        }

        #region IPlugin Members

        public event Action<IPlugin, bool> EnabledChanged;

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Util.ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
        }

        public void Draw(Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, System.Drawing.Graphics graphics, Action redraw) {
        }

        #endregion
    }
}
