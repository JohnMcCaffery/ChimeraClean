using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.Inputs {
    class InputConfig : ConfigBase{
        public bool KeyboardEnabled;
        public bool MouseEnabled;

        public override string Group {
            get { return "Inputs"; }
        }

        protected override void InitConfig() {
            KeyboardEnabled = Get(true, "KeyboardEnabled", true, "Whether to take input from the keyboard at the beginning.");
            MouseEnabled = Get(true, "MouseEnabled", true, "Whether to use the mouse to control the cursor from the beginning.");
        }
    }
}
