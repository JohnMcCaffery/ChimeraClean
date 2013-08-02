using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace Chimera.Util {
    public static class Algorithms {
        private static float Cross(Vector2 v, Vector2 w) {
            return (v.X * w.Y) - (v.Y * w.X);
        }

        private static bool VerticalIntersect(Vector2 pva, Vector2 pvb, Vector2 pla, Vector2 plb) {
                float m2 = (plb.Y - pla.Y) / (plb.X - pla.X);
                float c2 = pla.Y - (m2 * pla.X);
                float y = (pva.X * m2) + c2;

                return
                    Math.Min(pla.X, plb.X) < pva.X && Math.Max(pla.X, plb.X) > pva.X &&
                    Math.Min(pva.X, pvb.X) < y && Math.Max(pva.X, pvb.X) > y;
        }

        public static bool LineIntersects(Vector2 p1a, Vector2 p1b, Vector2 p2a, Vector2 p2b) {
            //Both vertical
            if (p1a.X == p1b.X && p2a.X == p2b.X)
                return p1a.X == p2a.X;
            //Line 1 vertical
            else if (p1a.X == p1b.X)
                return VerticalIntersect(p1a, p1b, p2a, p2b);
            //Line 2 vertical
            else if (p2a.X == p2b.X)
                return VerticalIntersect(p2a, p2b, p1a, p1b);

            //Neither line vertical
            float m1 = (p1b.Y - p1a.Y) / (p1b.X - p1a.X);
            float c1 = (m1 * p1a.X) + p1a.Y;

            float m2 = (p2b.Y - p2a.Y) / (p2b.X - p2a.X);
            float c2 = p2a.Y - (m2 * p2a.X);

            //Lines parallel
            if (m1 == m2)
                return c1 == c2;

            float x = (c2 - c1) / (m1 - m2);
            float y = (m1 * x) + c1;

            return
                Math.Min(p1a.X, p1b.X) < x && Math.Max(p1a.X, p1b.X) > x &&
                Math.Min(p1a.Y, p1b.Y) < y && Math.Max(p1a.Y, p1b.Y) > y &&
                Math.Min(p2a.X, p2b.X) < x && Math.Max(p2a.X, p2b.X) > x &&
                Math.Min(p2a.Y, p2b.Y) < y && Math.Max(p2a.Y, p2b.Y) > y;
        }

        public static bool PolygonContains(Vector2 p, params Vector2[] points) {
            // http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
            Vector2 r = new Vector2(0f, 100f) - p;

            Vector2 q = points[points.Length - 1];
            int c = 0;
            foreach (Vector2 sAbs in points) {
                Vector2 s = sAbs - q;
                //Parallel
                if (Cross(r, s) != 0f) {
                    float t = Cross(q - p, s / Cross(r, s));
                    float u = Cross(q - p, r / Cross(r, s));

                    if (t >= 0f && t <= 1f &&
                        u >= 0f && u <= 1f)
                        c++;
                }
                q = sAbs;
            }

            return c % 2 != 0;
        }
    }
}
