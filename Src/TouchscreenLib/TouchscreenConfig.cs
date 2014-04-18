using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using Chimera.Plugins;
using log4net;

namespace Touchscreen {
    public class TouchscreenConfig : AxisConfig {
        public float LeftW;
        public float LeftH;
        public float LeftPaddingH;
        public float LeftPaddingV;
        public float RightW;
        public float RightH;
        public float RightPaddingH;
        public float RightPaddingV;
        public float SingleW;
        public float SingleH;
        public float SinglePaddingH;
        public float SinglePaddingV;
        public string Frame;
        public SinglePos SinglePos;
        public double Opacity;

        public override string Group {
            get { return "Touchscreen"; }
        }

        public TouchscreenConfig()
            : base("Touchscreen") {
        }

        protected override void InitConfig() {
            LeftW = Get("Left", "W", .35f, "The width of the left control circle.");
            LeftH = Get("Left", "H", .8f, "The height of the left control circle.");
            LeftPaddingH = Get("Left", "PaddingH", .05f, "The padding to the left of the left control circle.");
            LeftPaddingV = Get("Left", "PaddingV", .1f, "The padding above the left control circle.");
            RightW = Get("Right", "W", .35f, "The width of the right control circle.");
            RightH = Get("Right", "H", .8f, "The height of the right control circle.");
            RightPaddingH = Get("Right", "PaddingH", .05f, "The padding to the left of the right control circle.");
            RightPaddingV = Get("Right", "PaddingV", .1f, "The padding above the right control circle.");
            SingleW = Get("Single", "W", .15f, "The width of the single axis control section.");
            SingleH = Get("Single", "H", .8f, "The height of the single axis control section.");
            SinglePaddingH = Get("Single", "PaddingH", .05f, "The padding to the left of the single axis control section.");
            SinglePaddingV = Get("Single", "PaddingV", .1f, "The padding above the single axis control section.");
            Frame = GetStr("Window", null, "The window which is a touch screen.");
            Opacity = Get("Opacity", .01f, "How opaque the input window should be.");

            //SinglePos = (SinglePos) Enum.Parse(typeof(SinglePos), Get("Single", "Position", "Right", "Where the single axis is positioned (Left, Right or Centre)."));
            SinglePos = GetEnum<SinglePos>("Single", "Position", SinglePos.Right, "Where the single axis is positioned (Left, Right or Centre).", LogManager.GetLogger("Touchscreen"));
        }
    }
}
