using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Overlay.Drawables;
using System.Drawing;

namespace Chimera.Overlay {
    public class XmlLoader {
        public static float GetFloat(XmlNode node, float defalt, params string[] attributes) {
            float t = defalt;
            if (attributes.Length == 0)
                return defalt;
            int attr = 0;
            string attribute = attributes[attr++];
            while ((node == null || node.Attributes[attribute] == null || !float.TryParse(node.Attributes[attribute].Value, out t)) && attr < attributes.Length)
                attribute = attributes[attr++];
            return t;
        }

        public static int GetInt(XmlNode node, int defalt, params string[] attributes) {
            int t = defalt;
            if (attributes.Length == 0)
                return defalt;
            int attr = 0;
            string attribute = attributes[attr++];
            while ((node == null || node.Attributes[attribute] == null || !int.TryParse(node.Attributes[attribute].Value, out t)) && attr < attributes.Length)
                attribute = attributes[attr++];
            return t;
        }

        protected WindowOverlayManager GetManager(Coordinator coordinator, XmlNode node) {
            WindowOverlayManager mManager;
            XmlAttribute windowAttr = node.Attributes["Window"];
            if (windowAttr == null) {
                mManager = coordinator.Windows[0].OverlayManager;
                Console.WriteLine("No window specified. Using default window. Using default window " + mManager.Window.Name + ".");
            } else {
                Window window = coordinator.Windows.FirstOrDefault(w => w.Name == windowAttr.Value);
                if (windowAttr == null) {
                    mManager = coordinator.Windows[0].OverlayManager;
                    Console.WriteLine(windowAttr.Value + " is not a known window. Using default window " + mManager.Window.Name + ".");
                } else
                    mManager = coordinator[windowAttr.Value].OverlayManager;
            }
            return mManager;
        }

        private const string DEFAULT_FONT = "Verdana";
        private const float DEFAULT_FONT_SIZE = 12f;
        private const FontStyle DEFAULT_FONT_STYLE = FontStyle.Regular;
        protected static readonly Color DEFAULT_FONT_COLOUR = Color.Black;

        protected Font GetFont(XmlNode node) {            FontStyle style = DEFAULT_FONT_STYLE;
            FontStyle styleT;

            string fontName = node.Attributes["Font"] != null ? node.Attributes["Name"].Value : DEFAULT_FONT;
            float size = GetFloat(node, DEFAULT_FONT_SIZE, "Size");
            if (node.Attributes["Style"] != null && Enum.TryParse<FontStyle>(node.Attributes["Style"].Value, true, out styleT))
                style = styleT;
            return new Font(fontName, size, style);
        }

        protected Color GetColour(XmlNode node, Color defalt) {
            Color colour = defalt;
            if (node.Attributes["Colour"] != null)
                return Color.FromName(node.Attributes["Colour"].Value);
            Console.WriteLine("Unable to get colour for " + node.Name + ". No Colour attribute specified.");
            return defalt;
        }
    }
}
