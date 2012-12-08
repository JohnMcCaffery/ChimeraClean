using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using UtilLib;

namespace ChimeraLib {
    public class Window {
        private Rotation rotation = new Rotation(0f, 0f);
        private Vector3 screenPosition = Vector3.UnitX * 400;
        private Vector3 eyePosition = Vector3.Zero;
        private double aspectRatio = 9f / 16f;
        private double mmDiagonal = 19.0 * 25.4;
        private bool lockFrustum = true;

        public static readonly double TOLERANCE = 0.0001;
        //private float height = 720f;

        public Window() {
            rotation.OnChange += RotationChanged;
        }
        private void Changed() {
            if (OnChange != null)
                OnChange(this, null);
        }

        private void RotationChanged(object source, EventArgs args) {
            Changed();
        }

        /// <summary>
        /// Triggered whenever any of the properties of the window changes.
        /// </summary>
        public event EventHandler OnChange;

        /// <summary>
        /// Triggered whenever any the eye offset property changes.
        /// Useful for having other screens automatically adjust to this screens positioning.
        /// Supplies the difference between the old eye position and the new one that can be added to frustum position to keep it in one place.
        /// </summary>
        public event Action<Vector3> OnEyeChange;

        /// <summary>
        /// When true changing the eye offset will not affect the location of the screen.
        /// </summary>
        public bool LockScreenPosition {
            get { return lockFrustum; }
            set {
                lockFrustum = value;
                Changed();
            }
        }

        /// <summary>
        /// The position of the centre screen in real space (mm).
        /// </summary>
        public Vector3 ScreenPosition {
            get { return screenPosition; }
            set {
                if (value == screenPosition)
                    return;
                screenPosition = value;
                Changed();
            }
        }

        /// <summary>
        /// The offset of the origin/eye position for this screen from the centre of the real space (mm).
        /// </summary>
        public Vector3 EyePosition {
            get { return eyePosition; }
            set {
                if (value == eyePosition)
                    return;
                Vector3 diff = value - eyePosition;
                if (!lockFrustum) {
                    screenPosition += diff;
                }
                if (OnEyeChange != null)
                    OnEyeChange(diff);
                eyePosition = value;
                Changed();
            }
        }

        /// <summary>
        /// The rotation of the screen from forward in real space.
        /// </summary>
        public Rotation RotationOffset {
            get { return rotation; }
            set {
                if (rotation != null)
                    rotation.OnChange -= RotationChanged;
                rotation = value;
                rotation.OnChange += RotationChanged;
                if (OnChange != null)
                    OnChange(this, null);
            }
        }

        /// <summary>
        /// How wide the screen is in real space (mm). 
        /// Changing this will also change the aspect ratio and the diagonal.
        /// Width is a function of height and aspect ration. w = h / ar
        /// </summary>
        public double Width {
            get { return (Math.Cos(Math.Atan(aspectRatio)) * mmDiagonal); }
            set {
                if (Math.Abs(Width - value) < TOLERANCE || value <= 0.0)
                    return;
                aspectRatio = Height / value;
                mmDiagonal = value / Math.Cos(Math.Atan(aspectRatio));
                Changed();
            }
        }

        /// <summary>
        /// The height of the screen in real space (mm).
        /// Changing this will also change the aspect ration and the diagonal.
        /// </summary>
        public double Height {
            get { return (Math.Sin(Math.Atan(aspectRatio)) * mmDiagonal); }
            set {
                if (Math.Abs(Width - value) < TOLERANCE || value <= 0.0)
                    return;
                aspectRatio = value / Width;
                mmDiagonal = value / Math.Sin(Math.Atan(aspectRatio));
                Changed();
            }
        }

        /// <summary>
        /// The diagonal size of the screen. Specified in inches.
        /// This is included for convenience. Most screens are rated in diagonal inches.
        /// Changing this will change the width and height according to the aspect ratio.
        /// </summary>
        public double Diagonal {
            get { return mmDiagonal; }
            set {
                if (mmDiagonal == value || value <= 0.0)
                    return;
                mmDiagonal = value;
                Changed();
            }
        }

        /// <summary>
        /// The aspect ratio between the height and width of the screen. (h/w).
        /// Changing this will change the width of the screen.
        /// Calculated as height / width.
        /// </summary>
        public double AspectRatio {
            get { return aspectRatio; }
            set {
                if (aspectRatio == value || value <= 0.0)
                    return;
                aspectRatio = value;
                Changed();
            }
        }

        /// <summary>
        /// The field of view the screen shows, in radians. 
        /// Changing this will change the height and width of the screen according to the aspect ratio.
        /// Calculated as the tangent of <code>height / (2 * d)</code> where d is distance to the screen.
        /// </summary>
        public double FieldOfView {
            get {
                Vector3 top = new Vector3(0f, 0f, (float) (Height / 2.0));
                Vector3 bottom = top * -1;
                Quaternion q = Quaternion.CreateFromEulers(0f, (float) (rotation.Pitch * Rotation.DEG2RAD), 0f);
                top *= q;
                bottom *= q;

                top += screenPosition;
                bottom += screenPosition;

                float dot = Vector3.Dot(Vector3.Normalize(top - eyePosition), Vector3.Normalize(bottom - eyePosition));
                return Math.Acos(dot);
            }
            set {
                double fov = FieldOfView;
                if (Math.Abs(fov) < TOLERANCE || value <= 0.0)
                    return;
                double height = 2 * ScreenDistance * Math.Sin(value / 2.0);
                double a = Math.Cos(value / 2);
                if (a != 0.0)
                    height /= a;
                mmDiagonal = Math.Sqrt(Math.Pow(height, 2) + Math.Pow(height / aspectRatio, 2));
                Changed();
            }
        }

        public double FrustumOffsetH {
            get { return CalculateFrustumOffset(v => new Vector2(v.X, v.Y)); } 
        }

        public double FrustumOffsetV {
            //get { return CalculateFrustumOffset(v => new Vector2(v.X, v.Z)); }
            get { return CalculateFrustumOffset(v => new Vector2((float) Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2)), v.Z)); }
        }

        private double CalculateFrustumOffset(Func<Vector3, Vector2> to2D) {
            Vector2 h = to2D(screenPosition - eyePosition);
            Vector2 hNormal = Vector2.Normalize(h);
            Vector2 a = to2D(Vector3.Normalize(rotation.LookAtVector));
            float dot = Vector2.Dot(a, Vector2.Normalize(h));
            double angle = Math.Acos(dot) * Rotation.RAD2DEG;
            float length = h.Length();
            //return h.Length() * Math.Sqrt(1 - Math.Pow(Vector2.Dot(a, Vector2.Normalize(h)), 2));
            Vector2 rot = to2D((screenPosition - eyePosition) * Quaternion.Inverse(rotation.Quaternion));
            double component = Math.Sqrt(1 - Math.Pow(Vector2.Dot(a, Vector2.Normalize(h)), 2));
            return h.Length() * component * (rot.Y > 0 ? 1.0 : -1.0);
        }

        /// <summary>
        /// How far away the screen is from the origin along the direction the screen is rotated.
        /// </summary>
        public double ScreenDistance {
            //get { return Vector3.Dot(screenPosition - eyePosition, Vector3.Normalize(rotation.LookAtVector)); }
            get { 
                return (double) (screenPosition - eyePosition).Length();
            }
        }
    }
}