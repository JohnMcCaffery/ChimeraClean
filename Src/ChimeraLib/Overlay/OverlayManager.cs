using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public class StateManager {
        /// <summary>
        /// All the states this manager manages.
        /// </summary>
        private readonly Dictionary<string, State> mStates = new Dictionary<string,State>();
        /// <summary>
        /// The coordinator this state manager is tied to.
        /// </summary>
        private Coordinator mCoordinator;
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
        /// Generic mechanism for triggering events.
        /// </summary>
        public event Action<string> CustomTrigger;

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
        /// CreateWindowState the manager. Linking it with a coordinator.
        /// </summary>
        /// <param name="coordinator">The coordinator which this state form manages state for.</param>
        public StateManager(Coordinator coordinator) {
            mCoordinator = coordinator;
            mTransitionComplete = new Action<StateTransition>(transition_Finished);
        }

        public string Statistics {
            get {
                string table = "";
                table += "<TABLE>" + Environment.NewLine;
                table += "    <TR>" + Environment.NewLine;
                table += "        <TD>State Name</TD>" + Environment.NewLine;
                table += "        <TD># Visits</TD>" + Environment.NewLine;
                table += "        <TD>Shortest Visit (m)</TD>" + Environment.NewLine;
                table += "        <TD>Longest Visit (m)</TD>" + Environment.NewLine;
                table += "        <TD>Mean Visit Length (m)</TD>" + Environment.NewLine;
                table += "    </TR>" + Environment.NewLine;

                foreach (var state in mStates.Values)
                    table += state.StatisticsRow;

                table += "</TABLE>" + Environment.NewLine;

                return table;
            }
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
        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        /// <summary>
        /// The current state the manager is in. Will be null during a transition.
        /// Setting the state directly will immediately skip to the new state without any transition.
        /// </summary>
        public State CurrentState {
            get { return mCurrentState; }
            set {
                if (mFirstState == null)
                    mFirstState = value;
                if (mCurrentTransition != null)
                    mCurrentTransition.Cancel();
                mCurrentState = value;
                mCurrentState.Active = true;
                foreach (var windowState in mCurrentState.WindowStates)
                    windowState.Manager.CurrentDisplay = windowState;
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
        }

        /// <summary>
        /// Trigger a custom event.
        /// </summary>
        /// <param name="custom">The string tied to the custom event.</param>
        public void TriggerCustom(string custom) {
            if (CustomTrigger != null)
                CustomTrigger(custom);
        }

        /// <summary>
        /// Add a state to the manager.
        /// </summary>
        public void AddState(State state) {
            foreach (var window in mCoordinator.Windows)
                state.Init();

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

        private void transition_Finished(StateTransition transition) {
            if (TransitionFinished != null)
                TransitionFinished(transition);
            lock (this) {
                transition.Finished -= mTransitionComplete;
                mCurrentTransition = null;
                CurrentState = transition.To;
            }
        }
    }
}
