using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Overlay.Drawables;
using System.Drawing;
using System.IO;

namespace Chimera.Overlay {
    public class XmlLoader {        public const string DEFAULT_FONT = "Verdana";
        public const float DEFAULT_FONT_SIZE = 12f;
        public const FontStyle DEFAULT_FONT_STYLE = FontStyle.Regular;
        public static readonly Color DEFAULT_FONT_COLOUR = Color.Black;

        public static string GetName(XmlNode node) {
            if (node.Attributes["Name"] == null)
                throw new ArgumentException("Unable to load " + node.Name + ". No name attribute specified.");
            return node.Attributes["Name"].Value;
        }

        public RectangleF GetBounds(XmlNode node) {
            if (node == null) {
                Console.WriteLine("Unable to look up bounds. No node specified. Using defaults");
                return new RectangleF(0f, 0f, 0f, 0f);
            }
            return new RectangleF(GetFloat(node, 0f, "X"), GetFloat(node, 0f, "Y"), GetFloat(node, .1f, "W", "Width"), GetFloat(node, .1f, "H", "Height"));
        }

        public RectangleF GetBounds(XmlNode node, Rectangle clip) {
            if (node == null)
                return new RectangleF(0f, 0f, 0f, 0f);

            if (node.Attributes["L"] == null)
                return GetBounds(node);

            float l = GetFloat(node, 0, "L", "Left");
            float r = GetFloat(node, clip.Width / 10, "R", "Right");
            float t = GetFloat(node, 0, "T", "Top");
            float b = GetFloat(node, clip.Height / 10, "B", "Bottom");
            return new RectangleF(l / clip.Width, t / clip.Height, (r - l) / clip.Width, (b - t) / clip.Height);
        }

        public static Bitmap GetImage(XmlNode node) {
            if (node == null) {
                Console.WriteLine("Unable to load image. No node specified.");
                return null;
            }
            if (node.Attributes["File"] == null) {
                Console.WriteLine("Unable to load image. No file specified.");
                return null;
            }

            string file = Path.GetFullPath(node.Attributes["File"].Value);
            if (!File.Exists(file)) {
                Console.WriteLine("Unable to load image. " + file + " does not exist.");
                return null;
            }

            return new Bitmap(file);    
        }

        public static string GetString(XmlNode node, string defalt, params string[] attributes) {
            string t = defalt;
            if (attributes.Length == 0)
                return defalt;
            int attr = 0;
            string attribute = attributes[attr++];
            while ((node == null || node.Attributes[attribute] == null) && attr < attributes.Length)
                attribute = attributes[attr++];
            return t;
        }

        public static bool GetBool(XmlNode node, bool defalt, params string[] attributes) {
            bool t = defalt;
            if (attributes.Length == 0)
                return defalt;
            int attr = 0;
            string attribute = attributes[attr++];
            while ((node == null || node.Attributes[attribute] == null || !bool.TryParse(node.Attributes[attribute].Value, out t)) && attr < attributes.Length)
                attribute = attributes[attr++];
            return t;
        }

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

        public static WindowOverlayManager GetManager(StateManager manager, XmlNode node) {
            WindowOverlayManager mManager;
            if (node == null) {
                mManager = manager.Coordinator.Windows[0].OverlayManager;
                Console.WriteLine("No node specified when looking up window. Using default window " + mManager.Window.Name + ".");
            }
            XmlAttribute windowAttr = node.Attributes["Window"];
            if (windowAttr == null) {
                mManager = manager.Coordinator.Windows[0].OverlayManager;
                Console.WriteLine("No window specified whilst resolving window for " + node.Name + ". Using default window. Using default window " + mManager.Window.Name + ".");
            } else {
                Window window = manager.Coordinator.Windows.FirstOrDefault(w => w.Name == windowAttr.Value);
                if (windowAttr == null) {
                    mManager = manager.Coordinator.Windows[0].OverlayManager;
                    Console.WriteLine(windowAttr.Value + " is not a known window. Using default window " + mManager.Window.Name + ".");
                } else
                    mManager = manager.Coordinator[windowAttr.Value].OverlayManager;
            }
            return mManager;
        }

        public static Font GetFont(XmlNode node) {
            if (node == null) {
                Console.WriteLine("No node specified when looking up font. Using defaults.");
                return new Font(DEFAULT_FONT, DEFAULT_FONT_SIZE, DEFAULT_FONT_STYLE);
            }
            FontStyle style = DEFAULT_FONT_STYLE;
            FontStyle styleT;

            string fontName = node != null && node.Attributes["Font"] != null ? node.Attributes["Name"].Value : DEFAULT_FONT;
            float size = GetFloat(node, DEFAULT_FONT_SIZE, "Size");
            if (node != null && node.Attributes["Style"] != null && Enum.TryParse<FontStyle>(node.Attributes["Style"].Value, true, out styleT))
                style = styleT;
            return new Font(fontName, size, style);
        }

        public static Color GetColour(XmlNode node, Color defalt) {
            if (node == null) {
                Console.WriteLine("No node specified when looking up colour. Using defaults.");
                return DEFAULT_FONT_COLOUR;
            }
            Color colour = defalt;
            if (node.Attributes["Colour"] != null)
                return Color.FromName(node.Attributes["Colour"].Value);
            Console.WriteLine("Unable to get colour for " + node.Name + ". No Colour attribute specified.");
            return defalt;
        }
    }
}
