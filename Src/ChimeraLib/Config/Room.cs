using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Drawing;
using System.Xml;

namespace Chimera.Config {
    public class Room {
        private static readonly List<Vector3> sCorners = new List<Vector3>();
        private static bool sInitialised;
        private static Vector3 sAnchor;
        private static Vector3 sBig = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        private static Vector3 sSmall = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        private static void Init() {
            if (sInitialised)
                return;

            sInitialised = true;
            WindowConfig config = new WindowConfig();
            string roomFile = config.RoomFile;
            if (roomFile != null) {
                XmlDocument doc = new XmlDocument();
                doc.Load(roomFile);

                foreach (XmlElement cornerNode in doc.GetElementsByTagName("Room")[0].ChildNodes) {
                    XmlAttribute xAttr = cornerNode.Attributes["X"];
                    XmlAttribute yAttr = cornerNode.Attributes["Y"];
                    XmlAttribute zAttr = cornerNode.Attributes["Z"];

                    float x, y, z;
                    x = y = z = 0f;

                    if (xAttr != null)
                        float.TryParse(xAttr.Value, out x);
                    if (yAttr != null)
                        float.TryParse(yAttr.Value, out y);
                    if (yAttr != null)
                        float.TryParse(zAttr.Value, out z);

                    sCorners.Add(new Vector3(x, y, z));

                    if (sBig.X < x) sBig.X = x;
                    if (sBig.Y < y) sBig.Y = y;
                    if (sBig.Z < z) sBig.Z = z;

                    if (sSmall.X > x) sSmall.X = x;
                    if (sSmall.Y > y) sSmall.Y = y;
                    if (sSmall.Z > z) sSmall.Z = z;
                }

                sAnchor = config.RoomAnchor;
            }

        }

        public static Vector3 Anchor {
            get { 
                Init();
                return sAnchor; 
            }
            set { 
                Init();
                sAnchor = value;
            }
        }


        public static Vector3 Big {
            get { 
                 Init();
                return sBig; 
            }
        }

        public static Vector3 Small {
            get { 
                 Init();
                return sSmall; 
            }
        }

        public static IEnumerable<Vector3> Corners {
            get { 
                Init();
                return sCorners;
            }
        }

        public Vector3 InternalAnchor {
            get { return sAnchor; }
            set { sAnchor = value; }
        }

        public IEnumerable<Vector3> InternalCorners {
            get { return sCorners; }
        }

        public static void Draw(Graphics g, Func<Vector3, Point> to2D) {
            g.DrawPolygon(Pens.Black, sCorners.Select(v => to2D(v + sAnchor)).ToArray());
        }
    }
}
