using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class DialCursorRenderer : CursorRenderer {
        private static readonly int DEFAULT_R = 40;
        private static readonly Color DEFAULT_COLOUR = Color.Red;

        public DialCursorRenderer()
            : base(new DialRenderer(DEFAULT_R, DEFAULT_COLOUR).DrawHover, new Size(DEFAULT_R * 2, DEFAULT_R * 2)) {
        }
        public DialCursorRenderer(int r)
            : base(new DialRenderer(r, DEFAULT_COLOUR).DrawHover, new Size(r * 2, r * 2)) {
        }
        public DialCursorRenderer(Color colour)
            : base(new DialRenderer(DEFAULT_R, colour).DrawHover, new Size(DEFAULT_R * 2, DEFAULT_R * 2)) {
        }
        public DialCursorRenderer(int r, Color colour)
            : base(new DialRenderer(DEFAULT_R, colour).DrawHover, new Size(r * 2, r * 2)) {
        }
        public DialCursorRenderer(DialRenderer r)
            : base(r.DrawHover, new Size(r.R * 2, r.R * 2)) {
        }


        public DialCursorRenderer(WindowOverlayManager manager)
            : base(new DialRenderer(DEFAULT_R, DEFAULT_COLOUR).DrawHover, new Size(DEFAULT_R * 2, DEFAULT_R * 2), manager) {
        }
        public DialCursorRenderer(int r, WindowOverlayManager manager)
            : base(new DialRenderer(r, DEFAULT_COLOUR).DrawHover, new Size(r * 2, r * 2), manager) {
        }
        public DialCursorRenderer(Color colour, WindowOverlayManager manager)
            : base(new DialRenderer(DEFAULT_R, colour).DrawHover, new Size(DEFAULT_R * 2, DEFAULT_R * 2), manager) {
        }
        public DialCursorRenderer(int r, Color colour, WindowOverlayManager manager)
            : base(new DialRenderer(DEFAULT_R, colour).DrawHover, new Size(r * 2, r * 2), manager) {
        }
        public DialCursorRenderer(DialRenderer r, WindowOverlayManager manager)
            : base(r.DrawHover, new Size(r.R * 2, r.R * 2), manager) {
        }
    }
}
