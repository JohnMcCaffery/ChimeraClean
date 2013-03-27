using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chimera.Interfaces {
    public interface IOverlayWindowFactory {
        IOverlayWindow Make(OverlayController controller);
    }
    public interface IOverlayWindow {
        event FormClosedEventHandler FormClosed;

        /// <summary>
        /// Whether to go full screen.
        /// </summary>
        bool Fullscreen {
            get;
            set;
        }

        /// <summary>
        /// Redraw the input.
        /// </summary>
        void Redraw();

        /// <summary>
        /// Link this form with a logical input.
        /// </summary>
        /// <param name="input">The input to link this form with.</param>
        void Init(OverlayController controller);

        void Show();

        void Foreground();

        void Close();
    }
}
