/*************************************************************************
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
using Chimera.Interfaces.Overlay;
using System.IO;
using Chimera.Util;
using System.Xml;
using Chimera.Overlay.Drawables;
using System.Drawing;
using Chimera.Overlay.Triggers;
using System.Windows.Forms;
using Chimera.Overlay.GUI.Plugins;
using Chimera.Config;
using Chimera.Overlay.Transitions;
using Chimera.Interfaces;
using log4net;

namespace Chimera.Overlay {
    public partial class OverlayPlugin : XmlLoader, ISystemPlugin {
        private readonly ILog Logger = LogManager.GetLogger("Overlay");
        /// <summary>
        /// All the states this manager manages.
        /// </summary>
        private readonly Dictionary<string, State> mStates = new Dictionary<string,State>();
        /// <summary>
        /// The coordinator this state manager is tied to.
        /// </summary>
        private Core mCoordinator;
        /// <summary>
        /// The current state the manager is in. Will be null during a transition.
        /// </summary>
        private State mCurrentState;
        /// <summary>
        /// The first state that was set, when reset the overlay will go back to this.
        /// </summary>
        private State mFirstState;
        /// <summary>
        /// The current transition the manager is going through. Will be null if no transition is in progress.
        /// </summary>
        private StateTransition mCurrentTransition;
        /// <summary>
        /// Delegate for listening for transition end events.
        /// </summary>
        private Action<StateTransition> mTransitionComplete;
        /// <summary>
        /// Window managers for each window in the system.
        /// </summary>
        private readonly Dictionary<string, WindowOverlayManager> mWindowManagers = new Dictionary<string, WindowOverlayManager>();
        /// <summary>
        /// The control panel for the overlay.
        /// </summary>
        private OverlayPluginPanel mPanel;

        /// <summary>
        /// Triggered whenever a new state is added.
        /// </summary>
        public event Action<State> StateAdded;

        /// <summary>
        /// Triggered whenever a transition starts.
        /// </summary>
        public event Action<StateTransition> TransitionStarting;

        /// <summary>
        /// Triggered whenever a transition finishes.
        /// </summary>
        public event Action<StateTransition> TransitionFinished;

        /// <summary>
        /// Triggered whenever the current state changes.
        /// </summary>
        public event Action<State> StateChanged;

        /// <summary>
        /// Triggered whenever the overlay windows are launched.
        /// </summary>
        public event Action OverlayLaunched;
        /// <summary>
        /// Triggered whenever the overlay windows are launched.
        /// </summary>
        public event Action OverlayClosed;

        private Form mMasterForm;

        void mCoordinator_FrameAdded(Frame frame, EventArgs args) {
            WindowOverlayManager manager = new WindowOverlayManager(this, frame);
            mWindowManagers.Add(frame.Name, manager);

            if (mMasterForm != null)
                manager.SetForm(mMasterForm);

            if (mConfig.LaunchOverlay)
                manager.Launch();
        }

        public WindowOverlayManager this[string windowName] {
            get { return mWindowManagers[windowName]; }
        }

        public WindowOverlayManager this[int windowIndex] {
            get { return mWindowManagers[mCoordinator.Frames[0].Name]; }
        }

        public WindowOverlayManager[] OverlayManagers {
            get { return mWindowManagers.Values.ToArray(); }
        }

        /// <summary>
        /// All the states this manager manages.
        /// </summary>
        public State[] States {
            get { return mStates.Values.ToArray(); }
        }

        /// <summary>
        /// The coordinator this state manager is tied to.
        /// </summary>
        public Core Coordinator {
            get { return mCoordinator; }
        }

        /// <summary>
        /// The current state the manager is in. Will be null during a transition.
        /// Setting the state directly will immediately skip to the new state without any transition.
        /// </summary>
        public State CurrentState {
            get { return mCurrentState; }
            set {
                if (mCurrentState != null)
                    mCurrentState.Active = false;
                if (mFirstState == null)
                    mFirstState = value;
                if (mCurrentTransition != null)
                    mCurrentTransition.Cancel();
                mCurrentState = value;
                mCurrentState.Active = true;
                foreach (var windowState in mCurrentState.WindowStates)
                    windowState.Manager.CurrentDisplay = windowState;
                if (!mIdleEnabled) {
                    foreach (var trigger in mIdleTriggers)
                        trigger.Active = false;
                }
                if (mRedraw != null)
                    mRedraw();
                Logger.Info("Current state set to " + mCurrentState.Name + ".");
                if (StateChanged != null)
                    StateChanged(value);
            }
        }

        /// <summary>
        /// The current transition the manager is going through. Will be null if no transition is in progress.
        /// </summary>
        public StateTransition CurrentTransition {
            get { return mCurrentTransition; }
        }

        /// <summary>
        /// True if the manager is currently transitioning transition one state to another.
        /// </summary>
        public bool Transitioning {
            get { return mCurrentState == null; }
        }

        public void Reset() {
            CurrentState = mFirstState;
            foreach (var manager in OverlayManagers) {
                manager.Close();
                manager.Launch();
            }
        }

        /// <summary>
        /// Add a state to the manager.
        /// </summary>
        public void AddState(State state) {
            state.Init();

            if (!mStates.ContainsKey(state.Name))
                mStates.Add(state.Name, state);
            if (StateAdded != null)
                StateAdded(state);
        }

        /// <summary>
        /// Start the transition transition one state to another.
        /// </summary>
        /// <param name="transition">The transition to begin.</param>
        public void BeginTransition(StateTransition transition) {
            if (Transitioning)
                throw new InvalidOperationException("Unable to start transition transition " + transition.From.Name
                     + " to " + transition.To.Name + ". There is already a transition in progress.");

            lock (this) {
                mCurrentState = null;
                mCurrentTransition = transition;
                transition.Finished += mTransitionComplete;
            }
            if (TransitionStarting != null)
                TransitionStarting(transition);
            transition.Begin();
        }

        public void RedrawStatic() {
            foreach (var manager in OverlayManagers)
                manager.ForceRedrawStatic();
        }

        private void transition_Finished(StateTransition transition) {
            if (TransitionFinished != null)
                TransitionFinished(transition);
            lock (this) {
                transition.Finished -= mTransitionComplete;
                CurrentState = transition.To;
                mCurrentTransition = null;
            }
        }

        public bool IdleEnabled {
            get { return mIdleEnabled; }
            set {
                mIdleEnabled = value;
                foreach (var trigger in mIdleTriggers)
                    trigger.Active = value;
            }
        }

        public State GetState(string state) {
            return mStates.ContainsKey(state) ? mStates[state] : null;
        }

        public bool IsKnownWindow(string window) {
            return mWindowManagers.ContainsKey(window);
        }
    }
}
