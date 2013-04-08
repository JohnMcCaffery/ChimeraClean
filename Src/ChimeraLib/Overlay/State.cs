using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public abstract class State {
        /// <summary>
        /// The window states, mapped to the names of the windows.
        /// </summary>
        private readonly Dictionary<string, IWindowState> mWindowStates = new Dictionary<string,IWindowState>();
        /// <summary>
        /// Transitions transition this state to other states, mapped to the name of the other state.
        /// </summary>
        private readonly Dictionary<string, StateTransition> mTransitions = new Dictionary<string, StateTransition>();
        /// <summary>
        /// The form which will coordinate the state.
        /// </summary>
        private readonly StateManager mManager;
        /// <summary>
        /// The name for the state.
        /// </summary>
        private string mName;
        /// <summary>
        /// Whether this state is currently active.
        /// </summary>
        private bool mActive;

        /// <summary>
        /// CreateWindowState the state, specifying the name, form and the window factory for creating window states.
        /// </summary>
        /// <param name="name">The name of the state. All state names should be unique.</param>
        /// <param name="form">The form which will control this state.</param>
        public State(string name, StateManager manager) {
            mName = name;
            mManager = manager;

            mManager.Coordinator.WindowAdded += new Action<Window,EventArgs>(Coordinator_WindowAdded);
        }

        /// <summary>
        /// //TODO - is this right? 
        /// Relies on state being added to coordinator to add windows, i.e. quite late on during startup.
        /// </summary>
        public void Init() {
            foreach (var window in mManager.Coordinator.Windows)
                Coordinator_WindowAdded(window, null);
        }

        protected virtual void Coordinator_WindowAdded(Window window, EventArgs args) {
            if (!mWindowStates.ContainsKey(window.Name))
                mWindowStates.Add(window.Name, CreateWindowState(window));
        }

        public IWindowState this[string window] {
            get {
                if (!mWindowStates.ContainsKey(window))
                    Coordinator_WindowAdded(mManager.Coordinator[window], null);
                return mWindowStates[window];
            }
        }
    
        public IWindowState[] WindowStates {
            get { return mWindowStates.Values.ToArray(); }
        }

        /// <summary>
        /// All the transitions for the state.
        /// </summary>
        public StateTransition[] Transitions {
            get { return mTransitions.Values.ToArray(); }
        }

        /// <summary>
        /// The unique name for the state.
        /// </summary>
        public string Name {
            get { return mName; }
        }

        /// <summary>
        /// Whether the state is currently active.
        /// </summary>
        public bool Active {
            get { return mActive; }
            set { 
                mActive = value;
                foreach (var transition in mTransitions.Values)
                    transition.Active = value;
                foreach (var window in mWindowStates.Values)
                    window.Active = false;
                if (value)
                    TransitionToFinish();
                else
                    TransitionFromStart();
            }
        }

        /// <summary>
        /// The manager which will coordiante the state.
        /// </summary>
        public StateManager Manager {
            get { return mManager; }
        }

        /// <summary>
        /// Add a new transition to another state.
        /// </summary>
        /// <param name="stateTransition">The new transition to add.</param>
        public void AddTransition(StateTransition stateTransition) {
            mTransitions.Add(stateTransition.To.Name, stateTransition);
            if (stateTransition is IDrawable)
                AddFeature(stateTransition as IDrawable);
        }
        
        /// <summary>
        /// Add a feature to the be drawn on one of the windows for the state.
        /// </summary>
        /// <param name="window">The window to draw the feature on.</param>
        /// <param name="feature">The feature to draw.</param>
        public void AddFeature(IDrawable feature) {
            this[feature.Window].AddFeature(feature);
        }

        /// <summary>
        /// CreateWindowState a window state for drawing this state to the specified window.
        /// </summary>
        /// <param name="window">The window the new window state is to draw on.</param>
        public abstract IWindowState CreateWindowState(Window window);

        /// <summary>
        /// Called before a transition to this state begins, set up any graphics that need to be in place before the transition begins.
        /// Will only be called if a tranisition was used to get to the state.
        /// </summary>
        public abstract void TransitionToStart();

        /// <summary>
        /// Do any actions that need to be set as soon as the state is activated.
        /// Use this to make sure the overlay is set up as expected, e.g. set whether the camera should be controlled.
        /// </summary>
        protected abstract void TransitionToFinish();

        /// <summary>
        /// Do any actions that need to be when the state is de-activated.
        /// Use this to make sure the overlay is set up as expected, e.g. set whether the camera should be controlled.
        /// </summary>
        protected abstract void TransitionFromStart();

        /// <summary>
        /// Called after the transition away from this state has been finished. Finalize any graphics that needed to stay in place whilst the transition was going.
        /// Will only be called if a tranisition was used to get from the state.
        /// </summary>
        public abstract void TransitionFromFinish();

        public override string ToString() {
            return mName;
        }
    }
}
