using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public abstract class State : IState {
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

            foreach (var window in mManager.Coordinator.Windows)
                Coordinator_WindowAdded(window, null);

            mManager.Coordinator.WindowAdded += new Action<Window,EventArgs>(Coordinator_WindowAdded);
            mManager.AddState(this);
        }

        protected virtual void Coordinator_WindowAdded(Window window, EventArgs args) {
            mWindowStates.Add(window.Name, CreateWindowState(window));
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
        public virtual bool Active {
            get { return mActive; }
            set { 
                mActive = value;
                foreach (var transition in mTransitions.Values)
                    transition.Active = value;
                foreach (var window in mWindowStates.Values)
                    window.Active = false;
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
        }

        /// <summary>
        /// CreateWindowState a window state for drawing this state to the specified window.
        /// </summary>
        /// <param name="window">The window the new window state is to draw on.</param>
        public abstract IWindowState CreateWindowState(Window window);
    }
}
