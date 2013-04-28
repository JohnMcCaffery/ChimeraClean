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
