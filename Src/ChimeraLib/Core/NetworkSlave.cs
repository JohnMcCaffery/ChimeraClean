﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public class NetworkWindow : Window {
        public NetworkWindow(string name, params IOverlayArea[] overlayAreas)
            : base(name, overlayAreas) {
        }
    }
}