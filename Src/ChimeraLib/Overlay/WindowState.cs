using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay {
    public abstract class WindowState : IWindowState {
        /// <summary>
        /// The features which will be drawn on this window state.
        /// </summary>
        private readonly List<IDrawable> mFeatures = new List<IDrawable>();
        /// <summary>
        /// The overlay form for the window this window state is linked to.
        /// </summary>
        private readonly WindowOverlayManager mManager;
        /// <summary>
        /// True if the state needs to be redrawn.
        /// </summary>
        private bool mNeedsRedrawn;
        /// <summary>
        /// Whether the window state is currently active and should be drawn.
        /// </summary>
        private bool mActive;

        /// <param name="manager">The manager which controls this window state.</param>
        public WindowState(WindowOverlayManager manager) {
            mManager = manager;
        }

        /// <summary>
        /// Features which need to be drawn in the window.
        /// </summary>
        public IDrawable[] Features {
            get { return mFeatures.ToArray(); }
        }

        /// <summary>
        /// The manager which controls this window state.
        /// </summary>
        public WindowOverlayManager Manager {
            get { return mManager; }
        }

        /// <summary>
        /// Whether the dynmic part of the drawable needs to be redrawn.
        /// </summary>
        public virtual bool NeedsRedrawn {
            get { return mNeedsRedrawn || mFeatures.Aggregate(false, (s, d) => s || d.NeedsRedrawn); }
        }

        /// <summary>
        /// Whether or not the window state is currently enabled.
        /// </summary>
        public virtual bool Enabled {
            get { return mActive; }
            set { mActive = value; }
        }

        /// <summary>
        /// Add a drawable feature to the state. Any features added will be drawn on top of content drawn as part of the state itself.
        /// </summary>
        /// <param name="feature">The feature to add.</param>
        public virtual void AddFeature(IDrawable feature) {
            mFeatures.Add(feature);
        }

        /// <summary>
        /// Notify the drawable that the area on which it is to draw has changed. Should draw any elements which only change when the area is resized to the supplied graphics object.
        /// Call this transition a sub class to draw all features for this window state.
        /// </summary>
        /// <param name="clip">The area in which this drawable will be drawn.</param>
        /// <param name="graphics">The object with which to to draw any elements which only change when the area is resized.</param>
        public virtual void RedrawStatic(Rectangle clip, Graphics graphics) {
            foreach (var feature in mFeatures)
                feature.RedrawStatic(clip, graphics);
        }

        /// <summary>
        /// Draw the elements of the drawable that change more frequently than when the drawing area is resized.
        /// Call this transition a sub class to draw all features for this window state.
        /// </summary>
        /// <param name="graphics">The object with which to draw the elements.</param>
        public virtual void DrawDynamic(Graphics graphics) {
            foreach (var feature in mFeatures)
                feature.DrawDynamic(graphics);
        }

        /// <summary>
        /// Set whether or not this window state needs redrawn.
        /// </summary>
        /// <param name="value"></param>
        protected void SetNeedsRedrawn(bool value) {
            mNeedsRedrawn = value;
        }
    }
}
