using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using System.Drawing;
using Touchscreen.GUI;

namespace Touchscreen.Overlay {
    public class TouchscreenWindow : WindowState {
        private TouchscreenPlugin mPlugin;

        public TouchscreenWindow(WindowOverlayManager manager, TouchscreenPlugin plugin)
            : base(manager) {
            mPlugin = plugin;
            mPlugin.Left.SizeChanged += () => Manager.ForceRedrawStatic();
            mPlugin.Right.SizeChanged += () => Manager.ForceRedrawStatic();
            mPlugin.Single.SizeChanged += () => Manager.ForceRedrawStatic();
        }

        public override void DrawStatic(Graphics graphics) {
            base.DrawStatic(graphics);
            TouchscreenForm.TouchscreenForm_Paint(graphics, Clip, mPlugin);
        }
    }
}
