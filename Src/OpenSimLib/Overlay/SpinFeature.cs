using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Xml;
using System.Drawing;
using Chimera.Overlay.Triggers;
using Chimera.OpenSim;
using log4net;
using Chimera.Overlay;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera.OpenSim.Overlay
{
    public class SpinFeatureFactory : IFeatureFactory
    {
        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new SpinFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "Spin"; }
        }
    }

    public class SpinFeature : OverlayXmlLoader, IFeature
    {
        private bool mActive;
        private Core mCore;
        private string mFrame;
        private Rotation mDelta;

        public SpinFeature(OverlayPlugin manager, XmlNode node) {
            mCore = manager.Core;
            mDelta = new Rotation(0, 3);
            mFrame = GetManager(manager, node, "Spin Feature").Name;
        }

        public Rectangle Clip {
            get { return Rectangle.Empty; }
            set { }
        }

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value) {
                        mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation + mDelta, mDelta);
                    } else {
                        mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation + mDelta, Rotation.Zero);
                    }
                }
            }
        }

        public bool NeedsRedrawn {
            get { return false; }
        }

        public string Frame {
            get { return mFrame; }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }
    }
}
