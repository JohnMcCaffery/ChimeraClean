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
    public class RaiseArmHelpTrigger : IHelpTrigger {
        private RaiseArmHelpTriggerPanel mPanel;
        private bool mActive = true;

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

        public event Action HelpTriggered;

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new RaiseArmHelpTriggerPanel(this);
                return mPanel;
            }
        }

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public void Init(IKinectController controller) { 
            Vector up = Vector.Create(0f, 1f, 0f);
            mArmR = Nui.joint(Nui.Hand_Right) - Nui.joint(Nui.Shoulder_Right);
            mArmL = Nui.joint(Nui.Hand_Right) - Nui.joint(Nui.Shoulder_Right);
            mAngleR = Nui.dot(up, mArmR);
            mAngleL = Nui.dot(up, mArmL);

            mTriggerR = C.And(Nui.y(mArmR) > mHeightThreshold, mAngleR < mAngleThreshold);
            mTriggerL = C.And(Nui.y(mArmL) > mHeightThreshold, mAngleL < mAngleThreshold);
            mTrigger = C.Or(mTrigger, mTriggerL);

            mTrigger.OnChange += new ChangeDelegate(mTrigger_OnChange);
        }

        void mTrigger_OnChange() {
            if (mActive && HelpTriggered != null && mTrigger.Value)
                HelpTriggered();

        }
    }
}
