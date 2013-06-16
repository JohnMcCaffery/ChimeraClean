using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Overlay.Features;
using System.Drawing;
using System.IO;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Interfaces;
using System.Windows.Forms;
using log4net;

namespace Chimera.Overlay {
    public class XmlLoader : IControllable {
        private static readonly ILog Logger = LogManager.GetLogger("Overlay");

        private Panel mPanel = new Panel();
        private string mName = null;

        public virtual Control ControlPanel {
            get { return mPanel; }
        }

        public virtual string Name {
            get { return mName == null ? GetType().Name : mName; }
            set { mName = value; }
        }

        public const string DEFAULT_FONT = "Verdana";
        public const float DEFAULT_FONT_SIZE = 12f;
        public const FontStyle DEFAULT_FONT_STYLE = FontStyle.Regular;
        public static readonly Color DEFAULT_FONT_COLOUR = Color.Black;

        public static string GetName(XmlNode node, string reason) {
            //if (node.Attributes["Name"] == null)
                //throw new ArgumentException("Unable to load " + reason + " from " + node.Name + ". No name attribute specified.");
            return node.Attributes["Name"] == null ? node.Name : node.Attributes["Name"].Value;
        }

        public static RectangleF GetBounds(XmlNode node, string request) {
            if (node == null) {
                Logger.Debug("No node specified when looking up bounds for " + request + " from. Using defaults (full screen).");
                return new RectangleF(0f, 0f, 1f, 1f);
            }

            if (node == null)
                return new RectangleF(0f, 0f, 1f, 1f);

            if (node.Attributes["L"] == null)
                return new RectangleF(GetFloat(node, 0f, "X"), GetFloat(node, 0f, "Y"), GetFloat(node, 1f, "W", "Width"), GetFloat(node, 1f, "H", "Height"));

            float l = GetFloat(node, 0, "L", "Left");
            float r = GetFloat(node, 1f, "R", "Right");
            float t = GetFloat(node, 0, "T", "Top");
            float b = GetFloat(node, 1f, "B", "Bottom");
            return new RectangleF(l, t, (r - l), (b - t));
        }

        public static RectangleF GetBounds(XmlNode node, string request, Rectangle clip) {
            if (node == null)
                return GetBounds(node, request);
            RectangleF bounds = GetBounds(node, request);
            if (bounds.X > 0 || bounds.Y > 0 || bounds.Width > 1f || bounds.Height > 1f)
                return new RectangleF(bounds.X / clip.Width, bounds.Y / clip.Height,  bounds.Width/ clip.Width, bounds.Height / clip.Height);
            return bounds;
        }

        public static Bitmap GetImage(XmlNode node, string request) {
            if (node == null) {
                Logger.Debug("Unable to load image for " + request + ". No node specified.");
                return null;
            }
            if (node.InnerText == null) {
                Logger.Debug("Unable to load image for " + request + " from " + node.Name + ". No file specified.");
                return null;
            }

            try {
                string file = Path.GetFullPath(node.InnerText);
                if (!File.Exists(file)) {
                    Logger.Debug("Unable to load image for " + request + " from " + node.Name + ". #" + file + "' does not exist.");
                    return null;
                }
                return new Bitmap(file);    
            } catch (ArgumentException e) {
                Logger.Debug("Unable to load image for " + request + " from " + node.Name + ". '" + node.InnerText + "' is not a valid file name.");
                return null;
            }

        }

        public static string GetString(XmlNode node, string defalt, params string[] attributes) {
            if (attributes.Length == 0)
                return defalt;
            int attr = 0;
            string attribute = attributes[attr++];
            while ((node == null || node.Attributes[attribute] == null) && attr < attributes.Length)
                attribute = attributes[attr++];
            if (node != null && node.Attributes[attribute] != null)
                return node.Attributes[attribute].Value;
            return defalt;
        }

        public static double GetDouble(XmlNode node, double defalt, params string[] attributes) {
            double t = defalt;
            if (attributes.Length == 0)
                return defalt;
            int attr = 0;
            string attribute = attributes[attr++];
            while ((node == null || node.Attributes[attribute] == null || !double.TryParse(node.Attributes[attribute].Value, out t)) && attr < attributes.Length)
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

        public static WindowOverlayManager GetManager(OverlayPlugin manager, XmlNode node, string request) {
            WindowOverlayManager mManager;
            if (node == null) {
                mManager = manager[0];
                Logger.Debug("No node specified when looking up frame for " + request + ". Using " + mManager.Frame.Name + " as default.");
                return mManager;
            }
            XmlAttribute frameAttr = node.Attributes["Frame"];
            if (frameAttr == null) {
                mManager = manager[0];
                Logger.Debug("No window specified whilst resolving " + node.Name + " from " + node.Name + ". Using " + mManager.Frame.Name + " as default.");
            } else {
                if (!manager.IsKnownWindow(frameAttr.Value)) {
                    mManager = manager[0];
                    Logger.Debug(frameAttr.Value + " is not a known frame. Using " + mManager.Frame.Name + " as default.");
                } else
                    mManager = manager[frameAttr.Value];
            }
            return mManager;
        }

        public static Font GetFont(XmlNode node, string request) {
            if (node == null) {
                Logger.Debug("No node specified when looking up font for " + request + ". Using defaults.");
                return new Font(DEFAULT_FONT, DEFAULT_FONT_SIZE, DEFAULT_FONT_STYLE);
            }
            FontStyle style = DEFAULT_FONT_STYLE;
            FontStyle styleT;

            string fontName = node != null && node.Attributes["Font"] != null ? node.Attributes["Font"].Value : DEFAULT_FONT;
            float size = GetFloat(node, DEFAULT_FONT_SIZE, "Size");
            if (node != null && node.Attributes["Style"] != null && Enum.TryParse<FontStyle>(node.Attributes["Style"].Value, true, out styleT))
                style = styleT;
            return new Font(fontName, size, style);
        }

        public static Color GetColour(XmlNode node, string request, Color defalt) {
            if (node == null) {
                Logger.Debug("No node specified when looking up colour for " + request + ". Using defaults.");
                return DEFAULT_FONT_COLOUR;
            }
            Color colour = defalt;
            if (node.Attributes["Colour"] != null)
                return Color.FromName(node.Attributes["Colour"].Value);
            Logger.Debug("Unable to get colour for " + node.Name + ". No Colour attribute specified.");
            return defalt;
        }

        public static IEnumerable<XmlElement> GetChildrenOfChild(XmlNode root, string childName) {
            XmlNode childParent = root.SelectSingleNode("child::" + childName);
            if (childParent == null)
                return new XmlElement[0];
            return childParent.ChildNodes.OfType<XmlElement>();
        } 
    }
}
