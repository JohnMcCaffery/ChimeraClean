﻿/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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
using Chimera.Config;
using log4net;
using System.Diagnostics;

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
        Z,
        /// <summary>
        /// The perspective is top down and drawn on the heightmap.
        /// </summary>
        Map
    }
    public class DeltaUpdateEventArgs : EventArgs {
        /// <summary>
        /// The delta that results in the new position.
        /// </summary>
        public Vector3 positionDelta;
        /// <summary>
        /// The delta that results in the new orientation.
        /// </summary>
        public Rotation rotationDelta;

        /// <param name="positionDelta">The delta that results in the new position.</param>
        /// <param name="rotationDelta">The delta that results in the orientation.</param>
        public DeltaUpdateEventArgs(Vector3 positionDelta, Rotation rotationDelta) {
            this.positionDelta = positionDelta;
            this.rotationDelta = rotationDelta;
        }
    }
    public class CameraUpdateEventArgs : DeltaUpdateEventArgs {
        /// <summary>
        /// The new position for the camera.
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// The new orientation of the camera.
        /// </summary>
        public Rotation rotation;

        /// <param name="position">The new position for the camera.</param>
        /// <param name="positionDelta">The delta that results in the new position.</param>
        /// <param name="rotation">The new orientation for the camera.</param>
        /// <param name="rotationDelta">The delta that results in the orientation.</param>
        public CameraUpdateEventArgs(Vector3 position, Vector3 positionDelta, Rotation rotation, Rotation rotationDelta) :
            base(positionDelta, rotationDelta) {
            this.position = position;
            this.rotation = rotation;
        }
    }

    /// <summary>
    /// How the camera for the system as a whole is controlled.
    /// </summary>
    public enum ControlMode {
        /// <summary>
        /// Control the system by specifying the precise location and orientation of the camera.
        /// </summary>
        Absolute,
        /// <summary>
        /// Control the system by sending delta events to be applied to camera/avatar position.
        /// </summary>
        Delta
    }

    public class HeightmapChangedEventArgs : EventArgs {
        public float[,] Heights;
        public int StartX;
        public int StartY;
    }

    public class Core : ICrashable, ITickSource {
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
        /// The plugins currently attached to the system.
        /// Plugins can can control the virtual position and lookAt of the view and the real world eye position to calculate views transition.
        /// They can also control the mouse or set up the heightmap.
        /// </summary>
        private readonly List<ISystemPlugin> mPlugins = new List<ISystemPlugin>();
        /// <summary>
        /// The windows which define where, in real space, each 'view' onto the virtual space is located.
        /// </summary>
        private readonly Dictionary<string, Frame> mFrames = new Dictionary<string, Frame>();
        /// <summary>
        /// Whether the core has been successfully initialised.
        /// </summary>
        private readonly bool mInitialised;
        /// <summary>
        /// The object containing the configuration for the system.
        /// </summary>
        private readonly CoreConfig mConfig;
        /// <summary>
        /// Logger for recording details of how the core is operating.
        /// </summary>
        private readonly ILog Logger = LogManager.GetLogger("Core");
        /// <summary>
        /// The authoratitive orientation of the camera in virtual space. This is read-only. To update it use the 'Update' method.
        /// </summary>
        private readonly Rotation mOrientation;
        /// <summary>
        /// Current orientation delta.
        /// </summary>
        private readonly Rotation mOrientationDelta;
        /// <summary>
        /// When the system was started.
        /// </summary>
        private readonly DateTime mStart = DateTime.UtcNow;

#if DEBUG
        /// <summary>
        /// Statistics object which monitors how long ticks are taking.
        /// </summary>
        private readonly TickStatistics mTickStats = new TickStatistics();
        /// <summary>
        /// Statistics object which monitors how long update operations are taking.
        /// </summary>
        private readonly TickStatistics mUpdateStats = new TickStatistics();
        /// <summary>
        /// Statistics object which monitors how camera updates are taking.
        /// </summary>
        private readonly TickStatistics mCameraStats = new TickStatistics();
        /// <summary>
        /// Statistics object which monitors how delta updates are taking.
        /// </summary>
        private readonly TickStatistics mDeltaStats = new TickStatistics();
#endif

        /// <summary>
        /// The authoratitive position of the camera in virtual space.
        /// </summary>
        private Vector3 mPosition;
        /// <summary>
        /// Current position delta.
        /// </summary>
        private Vector3 mPositionDelta;
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
        /// File to log information about any crash to.
        /// </summary>
        private string mCrashLogFile;
        /// <summary>
        /// The length of each tick for any plugin that has an event loop.
        /// </summary>
        private int mTickLength;
        /// <summary>
        /// How the camera is controlled.
        /// </summary>
        private ControlMode mControlMode = ControlMode.Absolute;
        /// <summary>
        /// When the view was last updated.
        /// </summary>
        private DateTime mLastUpdate;
        /// <summary>
        /// What exit code to exit with.
        /// </summary>
        private int mExitCode;

        private bool mEnableInputUpdates = false;

        /// <summary>
        /// Triggered whenever the ability to control the view is switched on or off.
        /// </summary>
        public event Action EnableUpdatesChanged;
        /// <summary>
        /// Triggered when the camera control mode changes.
        /// </summary>
        public event Action<Core, ControlMode> ControlModeChanged;
        /// <summary>
        /// Triggered whenever a window is added.
        /// </summary>
        public event Action<Frame, EventArgs> FrameAdded;
        /// <summary>
        /// Triggered whenever a window is removed.
        /// </summary>
        public event Action<Frame, EventArgs> WindowRemoved;
        /// <summary>
        /// Selected whenever the delta controlling where the camera is is updated.
        /// </summary>
        public event Action<Core, DeltaUpdateEventArgs> DeltaUpdated;
        /// <summary>
        /// Selected whenever the virtual camera position/orientation is changed.
        /// </summary>
        public event Action<Core, CameraUpdateEventArgs> CameraUpdated;
        /// <summary>
        /// Selected whenever the location of the eye in real space is updated.
        /// </summary>
        public event Action<Core, EventArgs> EyeUpdated;
        /// <summary>
        /// Triggered whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Core, KeyEventArgs> KeyDown;
        /// <summary>
        /// Triggered whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Core, KeyEventArgs> KeyUp;
        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        public event Action<Core, KeyEventArgs> Closed;
        /// <summary>
        /// Triggered whenever the heightmap changes.
        /// </summary>
        public event EventHandler<HeightmapChangedEventArgs> HeightmapChanged;
        /// <summary>
        /// Triggered when all frames, output modules and plugins have been initialised.
        /// </summary>
        public event Action InitialisationComplete;
        /// <summary>
        /// Triggered every tick. Listen for this to keep time across the system.
        /// </summary>
        public event Action Tick;

        /// <summary>
        /// Initialise this coordinator, specifying a collection of plugins to work with.
        /// </summary>
        /// <param name="plugins">The plugins which control this coordinator.</param>
        public Core(IOutputFactory outputFactory, params ISystemPlugin[] plugins) {
            try {
                mConfig = new CoreConfig();

                mPlugins = new List<ISystemPlugin>(plugins);
                mOrientation = new Rotation(mRotationLock, mConfig.Pitch, mConfig.Yaw);
                mOrientationDelta = new Rotation(mRotationLock);
                mPosition = mConfig.Position;
                mEyePosition = mConfig.EyePosition;
                mCrashLogFile = mConfig.CrashLogFile;
                mTickLength = mConfig.TickLength;
                mDefaultHeight = mConfig.HeightmapDefault;
                mHeightmap = new float[mConfig.XRegions * 256, mConfig.YRegions * 256];
                mEnableInputUpdates = mConfig.EnableInputUpdates;

                for (int i = 0; i < mHeightmap.GetLength(0); i++) {
                    for (int j = 0; j < mHeightmap.GetLength(1); j++) {
                        mHeightmap[i, j] = mDefaultHeight;
                    }
                }

                foreach (string frame in mConfig.Frames)
                    if (outputFactory != null)
                        AddFrame(new Frame(frame, outputFactory.Create(frame)));
                    else
                        AddFrame(new Frame(frame));

                foreach (var plugin in mPlugins) {
                    plugin.Init(this);
                    plugin.Enabled = mConfig.PluginEnabled(plugin);
                    Logger.Info("Loaded " + plugin.Name + ". " + (plugin.Enabled ? "Enabled" : "Disabled") + ".");
                }

                Thread tickThread = new Thread(TickMethod);

                tickThread.Name = "Tick Thread";
                //tickThread.Priority = ThreadPriority.Highest;
                tickThread.Start();
                mInitialised = true;
                if (InitialisationComplete != null)
                    InitialisationComplete();
            } catch (Exception e) {
                Logger.Warn("Unable to instantiate core. " + e.Message);
                Logger.Debug("Unable to instantiate core.", e);
            } finally {
                if (!mInitialised)
                    Close();
            }
        }

        private void TickMethod() {
            mAlive = true;
            while (mAlive) {
                DateTime tickStart = DateTime.UtcNow;
#if DEBUG
                mTickStats.Begin();
#endif
                var tick = Tick;
#if DEBUG
                if (tick != null) {
                    Delegate[] tickHandlers = tick.GetInvocationList();
                    foreach (Delegate currentHandler in tickHandlers)
                    {
                        
                        Action tickAction = (Action)currentHandler;
                        var watch = Stopwatch.StartNew();
                        tickAction();
                        watch.Stop();
                        if(watch.ElapsedMilliseconds > 0)
                            Logger.InfoFormat("{0}.{2} {1}", currentHandler.Method.DeclaringType.FullName, watch.ElapsedMilliseconds, currentHandler.Method.Name);
                    }
                }
#else
                if (tick != null)
                    tick();
#endif

#if DEBUG
                mTickStats.End();
#endif
                int time = (int)(mTickLength - DateTime.UtcNow.Subtract(tickStart).TotalMilliseconds);
                if (mAlive && time > 0)
                    Thread.Sleep(time);
            }
            Logger.Info("Tick Thread shut down.");
        }

        public bool Initialised {
            get { return mInitialised; }
        }

        /// <summary>
        /// What exit code the application should exit with.
        /// </summary>
        public int ExitCode {
            get { return mExitCode; }
            set { mExitCode = value; }
        }

        /// <summary>
        /// Look up a window by it's name.
        /// </summary>
        /// <param name="frameName">The name of the window to look up.</param>
        /// <returns>The window called 'windowName'</returns>
        /// <exception cref="InvalidOperationException">Thrown if there is no window with the given name.</exception>
        public Frame this[string frameName] {
            get { return mFrames[frameName]; }
        }

        public bool HasFrame(string window) {
            return mFrames.ContainsKey(window);
        }

        /// <summary>
        /// The heightmap the coordinator is working with. Any attempted camera position z value below the heightmap value will be set to the heightmap value.
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
            set {
                mEnableUpdates = value;
                if (EnableUpdatesChanged != null)
                    EnableUpdatesChanged();
            }
        }

        public bool EnableInputUpdates
        {
            get { return mEnableInputUpdates; }
            set
            {
                mEnableInputUpdates = value;
                if (EnableUpdatesChanged != null)
                    EnableUpdatesChanged();
            }
        }

        /// <summary>
        /// The windows which define where, in real space, each 'view' onto the virtual space is located.
        /// </summary>
        public Frame[] Frames {
            get { return mFrames.Values.ToArray() ; }
        }

        /// <summary>
        /// The plugins which can control the virtual position and lookAt of the view and the real world eye position to calculate views transition.
        /// </summary>
        public ISystemPlugin[] Plugins {
            get { return mPlugins.ToArray() ; }
        }

        /// <summary>
        /// The authoratitive position of the camera in virtual space.
        /// </summary>
        public Vector3 Position {
            get { return mPosition ; }
        }

        /// <summary>
        /// The current delta for the camera in virtual space.
        /// </summary>
        public Vector3 PositionDelta {
            get { return mPositionDelta ; }
        }

        /// <summary>
        /// The authoratitive orientation of the camera position in virtual space. This is read-only. To update it use the 'Update' method.
        /// </summary>
        public Rotation Orientation {
            get { return mOrientation ; }
        } 

        /// <summary>
        /// The current delta for the camera orientation in virtual space.
        /// </summary>
        public Rotation OrientationDelta {
            get { return mOrientationDelta ; }
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
        /// How long every tick should be, in ms, for any plugin that has a refresh loop.
        /// </summary>
        public int TickLength {
            get { return mTickLength; }
            set { mTickLength = value; }
        }

        /// <summary>
        /// How the camera is controlled.
        /// </summary>
        public ControlMode ControlMode {
            get { return mControlMode; }
            set {
                if (value != mControlMode) {
                    mControlMode = value;
                    //TODO - add code here so that when the value is changed the camera updates
                    if (ControlModeChanged != null)
                        ControlModeChanged(this, value);
                    Logger.Debug("Control mode changed to " + value + ".");
                }
            }
        }

#if DEBUG
        /// <summary>
        /// Object which will supply information about how long ticks are taking.
        /// </summary>
        public TickStatistics TickStatistics {
            get { return mTickStats; }
        }

        /// <summary>
        /// Object which will supply information about how long camera updates are taking.
        /// </summary>
        public TickStatistics UpdateStatistics {
            get { return mUpdateStats; }
        }

        /// <summary>
        /// Object which will supply information about how long camera updates are taking.
        /// </summary>
        public TickStatistics CameraStatistics {
            get { return mCameraStats; }
        }

        /// <summary>
        /// Object which will supply information about how delta updates ticks are taking.
        /// </summary>
        public TickStatistics DeltaStatistics {
            get { return mDeltaStats; }
        }
#endif

        /// <summary>
        /// Update a section of the heightmap.
        /// </summary>
        public void SetHeightmapSection(float[,] section, int startX, int startY, bool regionCompleted) {
            for (int i = 0; i < section.GetLength(0); i++) {
                for (int j = 0; j < section.GetLength(1); j++)
                    mHeightmap[startX + i, startY + j] = section[i, j];
            }
            //if (regionCompleted && HeightmapChanged != null) {
            if (HeightmapChanged != null) {
                HeightmapChangedEventArgs args = new HeightmapChangedEventArgs();
                args.Heights = section;
                args.StartX = startX;
                args.StartY = startY;
                HeightmapChanged(this, args);
            }
        }

        public void InputUpdate(Vector3 position, Vector3 postionDelta, Rotation orientation, Rotation orientationDelta)
        {
            if(mEnableInputUpdates)
                Update(position, postionDelta, orientation, orientationDelta);
        }

        /// <summary>
        /// Update the position of the camera.
        /// </summary>
        /// <param name="position">The new position for the camera.</param>
        /// <param name="postionDelta">The delta to be applied to the position for interpolation.</param>
        /// <param name="orientation">The look at vector for the camera orientation.</param>
        /// <param name="orientationDelta">The delta to be applied to the look at vector for interpolation.</param>
        public void Update(Vector3 position, Vector3 postionDelta, Rotation orientation, Rotation orientationDelta) {
            Update(position, postionDelta, orientation, orientationDelta, mControlMode);
        }
        /// <summary>
        /// Update the position of the camera.
        /// </summary>
        /// <param name="position">The new position for the camera.</param>
        /// <param name="postionDelta">The delta to be applied to the position for interpolation.</param>
        /// <param name="orientation">The look at vector for the camera orientation.</param>
        /// <param name="orientationDelta">The delta to be applied to the look at vector for interpolation.</param>
        /// <param name="mode">How the camera is to be updated. Can override the current global ControlMode setting.</param>
        public void Update(Vector3 position, Vector3 postionDelta, Rotation orientation, Rotation orientationDelta, ControlMode mode) {
            if (mEnableUpdates) {
#if DEBUG
                mUpdateStats.Begin();
#endif
                mPositionDelta = postionDelta;
                mOrientationDelta.Update(mRotationLock, orientationDelta);
                if (mode == Chimera.ControlMode.Absolute) {
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
                    mOrientation.Update(mRotationLock, orientation);
                    if (CameraUpdated != null && mAlive) {
                        CameraUpdateEventArgs args = new CameraUpdateEventArgs(position, postionDelta, orientation, orientationDelta);
#if DEBUG
                        mCameraStats.Begin();
#endif
                        CameraUpdated(this, args);
#if DEBUG
                        mCameraStats.End();
#endif
                        //Console.WriteLine("TickFrequency - time since update: " + (mTickLength -  DateTime.Now.Subtract(mLastUpdate).TotalMilliseconds));
                        mLastUpdate = DateTime.UtcNow;
                    }
                } else if (DeltaUpdated != null && mAlive) {
                    DeltaUpdateEventArgs args = new DeltaUpdateEventArgs(postionDelta, orientationDelta);
#if DEBUG
                    mDeltaStats.Begin();
#endif
                    DeltaUpdated(this, args);
#if DEBUG
                    mDeltaStats.End();
#endif
                }
#if DEBUG
                mUpdateStats.End();
#endif
            }
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
        /// Add a view to the system.
        /// </summary>
        /// <param name="frame">The window to add.</param>
        public void AddFrame(Frame frame) {
            mFrames.Add(frame.Name, frame);
            frame.Init(this);
            if (FrameAdded != null)
                FrameAdded(frame, null);
        }

        /// <summary>
        /// Draw any relevant information about this window onto a diagram.
        /// Which diagram is specified in the perspective parameter.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clip">The bounds of the area being drawn on.</param>
        /// <param name="scale">The value to control how large or small the diagram is rendered.</param>
        /// <param name="origin">The origin on the panel to draw transition.</param>
        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Rectangle clip, Action redraw, Perspective perspective) {
            foreach (var plugin in mPlugins)
                plugin.Draw(graphics, to2D, redraw, perspective);

            foreach (var window in Frames)
                window.Draw(to2D, graphics, clip, redraw, perspective);
        }

        /// <summary>
        /// Get a point on the 2D output diagram that corresponds to a point in real space.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="realPoint">The real point to translate into 2D coordinates.</param>
        public Point GetPoint(Func<Vector3, Point> to2D, Vector3 realPoint) {
            throw new System.NotImplementedException();
        }

        public void Close() {
            Close("-Shutdown");
        }

        /// <summary>
        /// Called when the coordinator is to be disposed of.
        /// </summary>
        public void Close(string reason) {
            Logger.Debug("Shutting down tick thread.");
            mAlive = false;
            Thread.Sleep(mTickLength * 2);

            foreach (var plugin in mPlugins) {
                plugin.Enabled = false;
                plugin.Close();
            }
            foreach (var frame in Frames)
                frame.Close();

            if (Closed != null)
                Closed(this, null);

            Logger.Info("System shutdown for " + reason + ".");
        }

        /// <summary>
        /// Handle a crash event,
        /// </summary>
        public void OnCrash(Exception e) {
            string dump = "Crash: " + DateTime.UtcNow.ToString("u") + Environment.NewLine;
            dump += "Uptime: " + DateTime.UtcNow.Subtract(mStart) + Environment.NewLine;
            dump += String.Format("{1}{0}{2}{0}{0}", Environment.NewLine, e.Message, e.StackTrace);
            dump += String.Format("-----------Core-----------{0}", Environment.NewLine);
            dump += "Virtual Position: " + mPosition + Environment.NewLine;
            dump += "Virtual Orientation | Yaw: " + mOrientation.Yaw + ", Pitch: " + mOrientation.Pitch + Environment.NewLine;
            dump += "Eye Position: " + mEyePosition + Environment.NewLine;

            /*
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
            */

            if (mFrames.Count > 0) {
                dump += String.Format("{0}{0}--------Frames--------{0}", Environment.NewLine);
                foreach (var window in Frames) {
                    try {
                        dump += Environment.NewLine + window.State;
                    } catch (Exception ex) {
                        dump += "Unable to get stats for window " + window.Name + ". " + ex.Message + Environment.NewLine;
                        dump += ex.StackTrace;
                    }
                }
                dump += Environment.NewLine;
            }

            if (mPlugins.Count > 0) {
                dump += String.Format("{0}{0}--------Plugins--------{0}", Environment.NewLine);
                foreach (var plugin in mPlugins) {
                    if (plugin.Enabled)
                        try {
                            dump += Environment.NewLine + plugin.State;
                        } catch (Exception ex) {
                            dump += "Unable to get stats for plugin " + plugin.Name + ". " + ex.Message + Environment.NewLine;
                            dump += ex.StackTrace;
                        } else
                        dump += Environment.NewLine + "--------" + plugin.Name + "--------" + Environment.NewLine + "Disabled";
                }
                dump += Environment.NewLine;
            }

            dump += String.Format("{0}{0}------------------------End of Crash Report------------------------{0}{0}", Environment.NewLine);

            ProcessWrangler.Dump(dump, "-Crash.log");

            Close("-Crash");
        }

        /// <summary>
        /// Get the plugin instance of the specified type. Throws an ArgumentException if no such plugin is found.
        /// </summary>
        public T GetPlugin<T> () where T : ISystemPlugin {
            Type t = typeof(T);
            ISystemPlugin ret = mPlugins.FirstOrDefault(plugin => plugin.GetType() == t);
            if (ret == null)
                throw new ArgumentException("Unable to get plugin. No plugin of the specified type (" + t.FullName + ") found.");
            return (T)ret;
        }

        public bool HasPlugin<PluginT>() {
            return mPlugins.FirstOrDefault(p => p is PluginT) != null;
        }
    }
}
