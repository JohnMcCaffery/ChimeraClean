using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Interfaces.Overlay {
    public interface IImageTransition : IDrawable {
        event Action Finished;
        void Init(Bitmap from, Bitmap to);
        void Begin();
    }
}
