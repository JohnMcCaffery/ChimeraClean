using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Features {
    public class ColourFeatureFactory : IFeatureFactory {
        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new ColourFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return new ColourFeature(manager, node, clip);
        }

        public string Name {
            get { return "Colour"; }
        }
    }


    public class ColourFeature : XmlLoader, IFeature {
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private Rectangle mClip;
        private Color mColour = Color.Black;
        private string mFrame;
        private bool mActive;

        public ColourFeature() { }

        public ColourFeature(Color colour) {
            mColour = colour;
        }

        public ColourFeature(RectangleF bounds) {
            mBounds = bounds;
        }

        public ColourFeature(Color colour, RectangleF bounds) {
            mColour = colour;
            mBounds = bounds;
        }

        public ColourFeature(OverlayPlugin manager, XmlNode node) {
            mColour = GetColour(node, "colour feature", mColour);
            mBounds = GetBounds(node, "colour feature");
            mFrame = GetManager(manager, node, "colour feature.").Name;
        }

        public ColourFeature(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            mColour = GetColour(node, "colour feature", mColour);
            mBounds = GetBounds(node, "colour feature", clip);
            mFrame = GetManager(manager, node, "colour feature.").Name;
        }

        public Rectangle Clip {
            get { return mClip;  }
            set { mClip = value; }
        }

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public bool NeedsRedrawn {
            get { return false; }
        }

        public string Frame {
            get { return mFrame; }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) {
            using (Brush b = new SolidBrush(mColour))
                graphics.FillRectangle(b, new RectangleF(mBounds.X * mClip.Width, mBounds.Y * mClip.Height, mBounds.Width * mClip.Width, mBounds.Height * mClip.Height));
        }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }
    }
}
