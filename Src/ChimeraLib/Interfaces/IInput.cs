using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Chimera.Util;

namespace Chimera {
    public interface ISystemInput : IInput {

        /// <summary>
        /// The input this input is to control.
        /// </summary>
        Coordinator Coordinator {
            get;
        }

        /// <summary>
        /// Initialise the input, giving it a reference to the input it is to control.
        /// </summary>
        /// <param name="input">The input object the input can control.</param>
        void Init(Coordinator coordinator);
    }
}
