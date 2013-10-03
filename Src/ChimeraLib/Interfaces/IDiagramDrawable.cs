using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenMetaverse;

namespace Chimera.Interfaces {
    public interface IDiagramDrawable {
        void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective);
    }
}
