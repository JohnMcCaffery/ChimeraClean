using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Util;
using System.Drawing;
using System.Windows.Forms;

namespace Chimera {
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

    public class Coordinator {
        /// <summary>
        /// The position of the eye in the real world, in mm.
        /// </summary>
        private Vector3 mEyePosition;
        /// <summary>
        /// The authoratitive position of the camera in virtual space.
        /// </summary>
        private Vector3 mPosition;
        /// <summary>
        /// Lock object to ensure only this object can update the rotation.
        /// </summary>
        private readonly object mRotationLock = new object();
        /// <summary>
        /// The authoratitive orientation of the camera in virtual space. This is read-only. To update it use the 'Update' method.
        /// </summary>
        private readonly Rotation mRotation;
        /// <summary>
        /// The inputs which can control the virtual position and lookAt of the view and the real world eye position to calculate views from.
        /// </summary>
        private readonly List<IInput> mInputs = new List<IInput>();
        /// <summary>
        /// The windows which define where, in real space, each 'window' onto the virtual space is located.
        /// </summary>
        private readonly List<Window> mWindows = new List<Window>();
        /// <summary>
        /// Scale that the horizontal output window is currently set to.
        /// </summary>
        private double mScaleH;
        /// <summary>
        /// Point on the horizontal output window that is to be used as the origin.
        /// </summary>
        private Point mOriginH;
        /// <summary>
        /// The size of the clip rectangle that bounds the horizontal output window.
        /// </summary>
        private Size mClipH;

        /// <summary>
        /// The size of the clip rectangle that bounds the vertical output window.
        /// </summary>
        private Size mClipV;

        /// <summary>
        /// Point on the vertical output window that is to be used as the origin.
        /// </summary>
        private Point mOriginV;

        /// <summary>
        /// Scale that the vertical output window is currently set to.
        /// </summary>
        private double mScaleV;
        /// <summary>
        /// True if the main menu is active.
        /// </summary>
        private bool mMainMenuActive;
        /// <summary>
        /// The currently active main menu item.
        /// </summary>
        private IOutput mCurrentSelection;

        /// <summary>
        /// Triggered whenever a new window is added.
        /// </summary>
        public event Action<Window, EventArgs> WindowAdded;

        /// <summary>
        /// Triggered whenever a window is removed.
        /// </summary>
        public event Action<IOverlayArea, EventArgs> WindowRemoved;

        /// <summary>
        /// Triggered whenever the virtual camera position/orientation is changed.
        /// </summary>
        public event Action<Coordinator, CameraUpdateEventArgs> CameraUpdated;

        /// <summary>
        /// Triggered whenever the location of the eye in real space is updated.
        /// </summary>
        public event Action<Coordinator, EventArgs> EyeUpdated;

        /// <summary>
        /// Triggered whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Coordinator, KeyEventArgs> KeyDown;

        /// <summary>
        /// Triggered whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Coordinator, KeyEventArgs> KeyUp;

        /// <summary>
        /// Triggered when the user selects a specific overlay area.
        /// </summary>
        public event Action<IOverlayArea, EventArgs> OverlayAreaActivated;

        /// <summary>
        /// Triggered whenever the main menu is activated.
        /// </summary>
        public event EventHandler MainMenuActivated;

        /// <summary>
        /// Initialise this coordinator, specifying a collection of inputs to work with.
        /// </summary>
        /// <param name="inputs">The inputs which control the camera through this coordinator.</param>
        public Coordinator(params IInput[] inputs) {
            mInputs = new List<IInput>(inputs);
            mRotation = new Rotation(mRotationLock);
        }

        /// <summary>
        /// Initialise this coordinator, specifying a collection of inputs to work with and a collection of windows coordinated by this coordinator.
        /// </summary>
        /// <param name="windows">The windows which are coordinated by this coordinator.</param>
        /// <param name="inputs">The inputs which control the camera through this coordinator.</param>
        public Coordinator(IEnumerable<Window> windows, params IInput[] inputs)
            : this(inputs) {
            mWindows = new List<Window>(windows);
            foreach (var window in mWindows)
                window.Init(this);
        }

        /// <summary>
        /// The windows which define where, in real space, each 'window' onto the virtual space is located.
        /// </summary>
        public Window[] Windows {
            get { return mWindows.ToArray() ; }
        }

        /// <summary>
        /// The inputs which can control the virtual position and lookAt of the view and the real world eye position to calculate views from.
        /// </summary>
        public IInput[] Inputs {
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
        public Rotation Rotation {
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
        /// True if the main menu is currently active.
        /// </summary>
        public bool MainMenuActive {
            get {
                throw new System.NotImplementedException();
            }
            set {
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
            if (!mMainMenuActive) {
                mPosition = position;
                mRotation.Update(mRotationLock, orientation);
                if (CameraUpdated != null) {
                    CameraUpdateEventArgs args = new CameraUpdateEventArgs(position, postionDelta, orientation, orientationDelta);
                    CameraUpdated(this, args);
                }
            }
        }

        /// <summary>
        /// Register a key press or release.
        /// </summary>
        public void TriggerKeyboard(bool down, KeyEventArgs args) {
            if (down && KeyDown != null)
                KeyDown(this, args);
            else if (!down && KeyUp != null)
                KeyDown(this, args);
        }

        /// <summary>
        /// Add a window to the system.
        /// </summary>
        /// <param name="window">The window to add.</param>
        public void AddWindow(Window window) {
            mWindows.Add(window);
            if (WindowAdded != null)
                WindowAdded(window, null);
        }

        /// <summary>
        /// DrawH any relevant information about this input onto a diagram.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="scale">Scale values for X and Y.</param>
        /// <param name="origin">The origin on the panel to draw from.</param>
        public void Draw(Graphics graphics, System.ResolveEventArgs clipRectangle, Vector2 scale, System.Version origin) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Notifies the system that an overlay area has been activated.
        /// </summary>
        /// <param name="mainMenuArea">The overlay area which was activated</param>
        public void ActivateOverlayArea(IOverlayArea overlayArea) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get a point on the horizontal output window that corresponds to a point in real space.
        /// </summary>
        /// <param name="realPoint">The real point to translate into 2D coordinates.</param>
        public Point GetPointH(Vector3 realPoint) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get a point on the vertical output window that corresponds to a point in real space.
        /// </summary>
        /// <param name="realPoint">The real point to translate into 2D coordinates.</param>
        public Point GetPointV(Vector3 realPoint) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Used to put the system into main menu mode.
        /// </summary>
        public void ActivateMainMenu() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the coordinator is to be disposed of.
        /// </summary>
        public void Close() {
            foreach (var input in mInputs)
                input.Close();
            foreach (var window in mWindows)
                window.Close();
        }
    }
}
