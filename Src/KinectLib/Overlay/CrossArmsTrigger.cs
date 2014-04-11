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
using System.Windows.Forms;
using Chimera.Kinect.GUI;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using Chimera.Config;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Xml;

namespace Chimera.Kinect.Overlay {
    public class CrossArmsTriggerFactory : ITriggerFactory {
        #region ITriggerFactory Members

        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return OverlayPlugin.HOVER_MODE; }
        }

        #endregion

        #region IFactory<ITrigger> Members

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node) {
            return new CrossArmsTrigger(node);
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "CrossArms"; }
        }

        #endregion
    }


    public class CrossArmsTrigger : OverlayXmlLoader, ITrigger {
        private RaiseArmTriggerPanel mPanel;
        private bool mEnabled = true;

        private Vector mBody;
        private Vector mForeArmR;
        private Vector mForeArmL;
        private Scalar mAngleArms;
        private Scalar mAngleL;
        private Condition mTriggerHeight;
        private Condition mTriggerL;
        private Condition mTrigger;
        private Condition mArmsCrossed;
        private Scalar mDepthThreshold;
        private Scalar mAngleThreshold;
        private Scalar mHeightThreshold;
        private Scalar mWidthThreshold;

        public Vector Body { get { return mBody; } }
        public Vector ForeArmR { get { return mForeArmR; } }
        public Vector ForeArmL { get { return mForeArmL; } }
        public Scalar AngleArms { get { return mAngleArms; } }
        public Scalar AngleL { get { return mAngleL; } }
        public Condition TriggerHeight { get { return mTriggerHeight; } }
        public Condition TriggerL { get { return mTriggerL; } }
        public Condition Trigger { get { return mTrigger; } }
        public Scalar AngleThreshold { get { return mAngleThreshold; } }
        public Scalar HeightThreshold { get { return mHeightThreshold; } }
        public Scalar DepthThreshold { get { return mDepthThreshold; } }
        public Scalar WidthThreshold { get { return mWidthThreshold; } }

        public event Action<ITrigger> Triggered;

        public void ForceTrigger() {
            if (Triggered != null)
                Triggered(this);
        }

        /*public override Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new CrossArmsTriggerPanel(this);
                return mPanel;
            }
        }*/

        public bool Active {
            get { return mEnabled; }
            set { mEnabled = value; }
        }

        public CrossArmsTrigger(XmlNode node) {
            mHeightThreshold = Nui.magnitude(Nui.joint(Nui.Shoulder_Centre) - Nui.joint(Nui.Hip_Centre));
            mAngleThreshold = Scalar.Create(.48f);
            mDepthThreshold = Scalar.Create(GetFloat(node, 3.6f, "DepthThreshold"));
            mWidthThreshold = Scalar.Create(GetFloat(node, 1f, "WidthThreshold"));

            mBody = Nui.joint(Nui.Hip_Centre);

            Condition inWidth = Nui.abs(Nui.x(Nui.joint(Nui.Hip_Centre))) < mWidthThreshold;
            Condition inDepth = Nui.z(Nui.joint(Nui.Hip_Centre)) < mDepthThreshold;
            Condition inRange = C.And(inWidth, inDepth);

            mTriggerHeight = C.And(Nui.y(Nui.joint(Nui.Hand_Right)) > Nui.y(Nui.joint(Nui.Elbow_Right)), Nui.y(Nui.joint(Nui.Hand_Left)) > Nui.y(Nui.joint(Nui.Elbow_Left)));
            mArmsCrossed = Nui.x(Nui.joint(Nui.Hand_Right)) < Nui.x(Nui.joint(Nui.Hand_Left));
            mTrigger = C.And(C.And(mTriggerHeight, mArmsCrossed), inRange);

            mTrigger.OnChange += new ChangeDelegate(mTrigger_OnChange);
        }

        void mTrigger_OnChange() {
            if (mEnabled && mTrigger.Value && Triggered != null)
                Triggered(this);
        }
    }
}
