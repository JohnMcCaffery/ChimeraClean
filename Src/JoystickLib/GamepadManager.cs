/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.XInput;
using Chimera;

namespace Joystick {
    static class GamepadManager {
        private static Gamepad sGamepad;
        private static Controller sController;
        private static bool mTracking;

        public static bool Initialised {
            get { return sGamepad != null && sController.IsConnected; }
        }

        public static Gamepad Gamepad {
            get { return sGamepad; }
        }


        public static void Init(ITickSource source) {
            if (!mTracking) {
                GetController();
                source.Tick += new Action(source_Tick);
                mTracking = true;
            }
        }

        static void source_Tick() {
            if (sController != null && sController.IsConnected)                sGamepad = sController.GetState().Gamepad;        }        public static Controller GetController() {
            sController = new Controller(UserIndex.One);
            if (sController.IsConnected)
                return sController;

            sController = new Controller(UserIndex.Two);
            if (sController.IsConnected)
                return sController;

            sController = new Controller(UserIndex.Three);
            if (sController.IsConnected)
                return sController;

            sController = new Controller(UserIndex.Four);
            if (sController.IsConnected)
                return sController;

            return null;
        }

        public static Controller GetController(UserIndex index) {
            sController = new Controller(index);
            return sController.IsConnected ? sController : null;
        }

    }
}
