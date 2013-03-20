using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Util;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Chimera.Interfaces;
using System.IO;
using System.Threading;

namespace Chimera {
    /// <summary>
    /// Which perspective to render.
    /// </summary>
    public enum Perspective { 
        /// <summary>
        /// View down the X axis.
        /// </summary>
        X, 
        /// <summary>
        /// View down the Y axis.
        /// </summary>
        Y, 
        /// <summary>
        /// View down the Z axis.
        /// </summary>
        Z 
    }

    public class CameraUpdateEventArgs {
        /// <summary>
        /// The new position for the camera.
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// The delta that results in the new position.
        /// </summary>
        public Vector3 positionDelta;
        /// <summary>
        /// The new orientation of the camera.
        /// </summary>
        public Rotation rotation;
        /// <summary>
        /// The delta that results in the new orientation.
        /// </summary>
        public Rotation rotationDelta;

        /// <param name="position">The new position for the camera.</param>
        /// <param name="positionDelta">The delta that results in the new position.</param>
        /// <param name="rotation">The new orientation for the camera.</param>
        /// <param name="rotationDelta">The delta that results in the orientation.</param>
        public CameraUpdateEventArgs(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) {
            this.position = position; 
            this.positionDelta = positionDelta; 
            this.rotation = rotation; 
            this.rotationDelta = rotationDelta; 
        }
    }

    public class HeightmapChangedEventArgs : EventArgs {
        public float[,] Heights;
        public int StartX;
        public int StartY;
    }

    public class Coordinator : ICrashable, IInputSource {
        /// <summary>
        /// The authoratitive orientation of the camera in virtual space. This is read-only. To update it use the 'Update' method.
        /// </summary>
        private readonly Rotation mRotation;
        /// <summary>
        /// The authoratitive position of the camera in virtual space.
        /// </summary>
        private Vector3 mPosition;
        /// <summary>
        /// The position of the eye in the real world, in mm.
        /// </summary>
        private Vector3 mEyePosition;
        /// <summary>
        /// Whether to accept updates to the camera position.
        /// </summary>
        private bool mEnableUpdates = true;
        /// <summary>
        /// Whether the tick thread should continue running.
        /// </summary>
        private bool mAlive;
        /// <summary>
        /// The heightmap for the region. Camera positions below this will be ignored.
        /// </summary>
        private readonly float[,] mHeightmap;
        /// <summary>
        /// The default height below which the camera should go. Used if heightmap data is not available.
        /// </summary>
        private readonly float mDefaultHeight;
        /// <summary>
        /// Lock object to ensure only this object can update the rotation.
        /// </summary>
        private readonly object mRotationLock = new object();
        /// <summary>
        /// The inputs which can control the virtual position and lookAt of the view and the real world eye position to calculate views from.
        /// </summary>
        private readonly List<ISystemInput> mInputs = new List<ISystemInput>();
        /// <summary>
        /// The windows which define where, in real space, each 'input' onto the virtual space is located.
        /// </summary>
        private readonly List<Window> mWindows = new List<Window>();
        /// <summary>
        /// The windows which define where, in real space, each 'input' onto the virtual space is located.
        /// </summary>
        private readonly List<IOverlayState> mStates = new List<IOverlayState>();

        /// <summary>
        /// Scales that the various output input is currently set to.
        /// </summary>
        private double[] mScales = new double[3];
        /// <summary>
        /// Point on the various output windows that is to be used as the origin.
        /// </summary>
        private Point[] mOrigins = new Point[3];
        /// <summary>
        /// The sizes of the clip rectangles that bounds the horizontal output input.
        /// </summary>
        private Size[] mSizes = new Size[3];
        /// <summary>
        /// The window the system is currently in.
        /// </summary>
        private IOverlayState mActiveState;

        /// <summary>
        /// File to log information about any crash to.
        /// </summary>
        private string mCrashLogFile;
        /// <summary>
        /// The length of each tick for any input that has an event loop.
        /// </summary>
        private int mTickLength;

        private Chimera.Overlay.MainMenu mMainMenu;

        /// <summary>
        /// Selected whenever a new input is added.
        /// </summary>
        public event Action<Window, EventArgs> WindowAdded;

        /// <summary>
        /// Selected whenever a input is removed.
        /// </summary>
        public event Action<ISelectable, EventArgs> WindowRemoved;

        /// <summary>
        /// Selected whenever the virtual camera position/orientation is changed.
        /// </summary>
        public event Action<Coordinator, CameraUpdateEventArgs> CameraUpdated;

        /// <summary>
        /// Selected whenever the location of the eye in real space is updated.
        /// </summary>
        public event Action<Coordinator, EventArgs> EyeUpdated;

        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Coordinator, KeyEventArgs> KeyDown;

        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Coordinator, KeyEventArgs> KeyUp;

        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Coordinator, KeyEventArgs> Closed;

        /// <summary>
        /// Selected when the user selects a specific overlay area.
        /// </summary>
        public event Action<IOverlayState, EventArgs> StateActivated;

        /// <summary>
        /// Triggered whenever the heightmap changes.
        /// </summary>
        public event EventHandler<HeightmapChangedEventArgs> HeightmapChanged;

        /// <summary>
        /// Triggered every tick. Listen for this to keep time across the system.
        /// </summary>
        public event Action Tick;

        /// <summary>
        /// Initialise this input, specifying a collection of inputs to work with.
        /// </summary>
        /// <param name="inputs">The inputs which control the camera through this input.</param>
        public Coordinator(params ISystemInput[] inputs) {
            CoordinatorConfig cfg = new CoordinatorConfig();

            mInputs = new List<ISystemInput>(inputs);
            mRotation = new Rotation(mRotationLock, cfg.Pitch, cfg.Yaw);
            mPosition = cfg.Position;
            mEyePosition = cfg.EyePosition;
            mCrashLogFile = cfg.CrashLogFile;
            mTickLength = cfg.TickLength;
            mDefaultHeight = cfg.HeightmapDefault;
            mHeightmap = new float[cfg.XRegions * 256, cfg.YRegions * 256];
            
            for (int i = 0; i < mHeightmap.GetLength(0); i++) {
                for (int j = 0; j < mHeightmap.GetLength(1); j++) {
                    mHeightmap[i, j] = mDefaultHeight;
                }
            }

            mMainMenu = new Chimera.Overlay.MainMenu();

            foreach (var input in mInputs)
                input.Init(this);

            Thread tickThread = new Thread(() => {
                mAlive = true;
                while (mAlive) {
                    if (Tick != null)
                        Tick();
                    Thread.Sleep(mTickLength);
                }
            });
            tickThread.Name = "Tick Thread";
            tickThread.Start();
        }

        /// <summary>
        /// Initialise this input, specifying a collection of inputs to work with and a collection of windows coordinated by this input.
        /// </summary>
        /// <param name="windows">The windows which are coordinated by this input.</param>
        /// <param name="inputs">The inputs which control the camera through this input.</param>
        public Coordinator(IEnumerable<Window> windows, params ISystemInput[] inputs)
            : this(inputs) {

            foreach (var window in windows)
                AddWindow(window);
        }

        /// <summary>
        /// Initialise this input, specifying a collection of inputs to work with and a collection of windows coordinated by this input.
        /// </summary>
        /// <param name="windows">The windows which are coordinated by this input.</param>
        /// <param name="inputs">The inputs which control the camera through this input.</param>
        public Coordinator(IEnumerable<Window> windows, Chimera.Overlay.MainMenu mainMenu, params ISystemInput[] inputs)
            : this(windows, inputs) {

            mMainMenu = mainMenu;
            mMainMenu.Init(this);
        }

        /// <summary>
        /// The heightmap the coordinator is working with. Any attempted input value below the heightmap value will be set to the heightmap value.
        /// Stops the camera going through the floor.
        /// </summary>
        public float[,] Heightmap {
            get { return mHeightmap; }
        }

        /// <summary>
        /// Whether to accept updates to the camera position.
        /// </summary>
        public bool EnableUpdates {
            get { return mEnableUpdates; }
            set { mEnableUpdates = value; }
        }

        /// <summary>
        /// The windows which define where, in real space, each 'input' onto the virtual space is located.
        /// </summary>
        public Window[] Windows {
            get { return mWindows.ToArray() ; }
        }

        /// <summary>
        /// The inputs which can control the virtual position and lookAt of the view and the real world eye position to calculate views from.
        /// </summary>
        public ISystemInput[] Inputs {
            get { return mInputs.ToArray() ; }
        }

        /// <summary>
        /// The authoratitive position of the camera in virtual space.
        /// </summary>
        public Vector3 Position {
            get { return mPosition ; }
        }
        /// <summary>
        /// The authoratitive orientation of the camera in virtual space. This is read-only. To update it use the 'Update' method.
        /// </summary>
        public Rotation Orientation {
            get { return mRotation ; }
        } 
        /// <summary>
        /// The position of the eye in the real world, in mm.
        /// </summary>
        public Vector3 EyePosition {
            get { return mEyePosition ; }
            set { 
                mEyePosition  = value;
                if (EyeUpdated != null)
                    EyeUpdated(this, null);
            }
        }

        /// <summary>
        /// How long every tick should be, in ms, for any input that has a refresh loop.
        /// </summary>
        public int TickLength {
            get { return mTickLength; }
        }

        /// <summary>
        /// The overlay which is controlling the system.
        /// </summary>
        public IOverlay Overlay {
            get { return mMainMenu; }
        }

        public void SetHeightmapSection(float[,] section, int startX, int startY, bool regionCompleted) {
            for (int i = 0; i < section.GetLength(0); i++) {
                for (int j = 0; j < section.GetLength(1); j++)
                    mHeightmap[startX + i, startY + j] = section[i, j];
            }
            if (regionCompleted && HeightmapChanged != null) {
            //if (HeightmapChanged != null) {
                HeightmapChangedEventArgs args = new HeightmapChangedEventArgs();
                args.Heights = section;
                args.StartX = startX;
                args.StartY = startY;
                HeightmapChanged(this, args);
            }
        }

        /// <summary>
        /// Update the position of the camera.
        /// </summary>
        /// <param name="position">The new position for the camera.</param>
        /// <param name="postionDelta">The delta to be applied to the position for interpolation.</param>
        /// <param name="orientation">The look at vector for the camera orientation.</param>
        /// <param name="orientationDelta">The delta to be applied to the look at vector for interpolation.</param>
        public void Update(Vector3 position, Vector3 postionDelta, Rotation orientation, Rotation orientationDelta) {
            //TODO put this back in when menus are set up
            //if (!mMainMenuActive) {
            if (mEnableUpdates) {
                int x = (int)position.X;
                int y = (int)position.Y;
                float height = 
                    x >= 0 && x < mHeightmap.GetLength(0) && 
                    y >= 0 && y < mHeightmap.GetLength(1) ? 
                        mHeightmap[x, y] : 
                        mDefaultHeight;
                height += .5f;
                if (position.Z < height) {
                    position.Z = height;
                    postionDelta.Z = 0f;
                }
                mPosition = position;
                mRotation.Update(mRotationLock, orientation);
                if (CameraUpdated != null && mAlive) {
                    CameraUpdateEventArgs args = new CameraUpdateEventArgs(position, postionDelta, orientation, orientationDelta);
                    CameraUpdated(this, args);
                }
            }
            //}
        }

        /// <summary>
        /// Register a key press or release.
        /// </summary>
        public void TriggerKeyboard(bool down, KeyEventArgs args) {
            if (down && KeyDown != null)
                KeyDown(this, args);
            else if (!down && KeyUp != null)
                KeyUp(this, args);
        }

        /// <summary>
        /// Add a input to the system.
        /// </summary>
        /// <param name="input">The input to add.</param>
        public void AddWindow(Window window) {
            mWindows.Add(window);
            window.Init(this);
            if (WindowAdded != null)
                WindowAdded(window, null);
        }

        /// <summary>
        /// DrawSelected any relevant information about this input onto a diagram.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="scale">The value to control how large or small the diagram is rendered.</param>
        /// <param name="origin">The origin on the panel to draw from.</param>
        public void Draw(Perspective perspective, Graphics graphics, Size clipRectangle, double scale, Point origin) {
            mScales[(int) perspective] = scale;
            mSizes[(int) perspective] = clipRectangle;
            mOrigins[(int) perspective] = origin;


            //DrawSelected stuff

            foreach (var input in mInputs)
                input.Draw(perspective, graphics);

            foreach (var window in mWindows)
                window.Draw(perspective, graphics);
        }

        /// <summary>
        /// Get a point on the horizontal output input that corresponds to a point in real space.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="realPoint">The real point to translate into 2D coordinates.</param>
        public Point GetPoint(Perspective perspective, Vector3 realPoint) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Notifies the system that an overlay area has been activated.
        /// </summary>
        /// <param name="input">The overlay area which was activated</param>
        public void ActivateState(IOverlayState overlayArea) {
            mActiveState = overlayArea;
            if (StateActivated != null)
                StateActivated(overlayArea, null);
        }

        /// <summary>
        /// Called when the input is to be disposed of.
        /// </summary>
        public void Close() {
            foreach (var input in mInputs)
                input.Close();
            foreach (var window in mWindows)
                window.Close();
            mAlive = false;
            if (Closed != null)
                Closed(this, null);
        }

        public void OnCrash(Exception e) {
            string dump = "Crash: " + DateTime.Now.ToString("u") + Environment.NewLine;
            dump += String.Format("{1}{0}{2}{0}{0}", Environment.NewLine, e.Message, e.StackTrace);
            dump += String.Format("-----------Coordinator-----------{0}", Environment.NewLine);
            dump += "Virtual Position: " + mPosition + Environment.NewLine;
            dump += "Virtual Orientation | Yaw: " + mRotation.Yaw + ", Pitch: " + mRotation.Pitch + Environment.NewLine;
            dump += "Eye Position: " + mEyePosition + Environment.NewLine;

            if (mActiveState != null) {
                dump += Environment.NewLine + "--------------" + mActiveState.Type + " Active-------------------" + Environment.NewLine;
                dump += "Instance: " + mActiveState.Name + Environment.NewLine;
                try {
                    dump += mActiveState.State;
                } catch (Exception ex) {
                    dump += "Unable to get stats for the active menu item. " + ex.Message + Environment.NewLine;
                    dump += ex.StackTrace;
                }
            }

            if (mWindows.Count > 0) {
                dump += String.Format("{0}{0}--------Windows--------{0}", Environment.NewLine);
                foreach (var window in mWindows)
                    try {
                        dump += Environment.NewLine + window.State;
                    } catch (Exception ex) {
                        dump += "Unable to get stats for input " + window.Name + ". " + ex.Message + Environment.NewLine;
                        dump += ex.StackTrace;
                    }
            }

            if (mInputs.Count > 0) {
                dump += String.Format("{0}{0}--------Inputs--------{0}", Environment.NewLine);
                foreach (var input in mInputs)
                    if (input.Enabled)
                        try {
                            dump += Environment.NewLine + input.State;
                        } catch (Exception ex) {
                            dump += "Unable to get stats for input " + input.Name + ". " + ex.Message + Environment.NewLine;
                            dump += ex.StackTrace;
                        } else
                        dump += Environment.NewLine + "--------" + input.Name + "--------" + Environment.NewLine + "Disabled";
            }

            dump += String.Format("{0}{0}------------------------End of Crash Report------------------------{0}{0}", Environment.NewLine);

            File.AppendAllText(mCrashLogFile, dump);

            Close();
        }

        /// <summary>
        /// Get the input instance of the specified type. Throws an ArgumentException if no such input found.
        /// </summary>
        public T GetInput<T> () where T : ISystemInput {
            Type t = typeof(T);
            ISystemInput ret = mInputs.FirstOrDefault(input => input.GetType() == t);
            if (ret == null)
                throw new ArgumentException("Unable to get input. No input of the specified type (" + t.FullName + ") found.");
            return (T)ret;
        }
    }
}
