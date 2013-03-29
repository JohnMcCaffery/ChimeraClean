using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay {
    public class StateTransition {
        /// <summary>
        /// The individual transitions for each window in the system.
        /// </summary>
        private readonly Dictionary<string, IWindowTransition> mWindowTransitions = new Dictionary<string,IWindowTransition>();
        /// <summary>
        /// During a transition, the windows which have completed the transition. The transition as a whole is only complete when all windows have completed.
        /// </summary>
        private readonly HashSet<IWindowTransition> mCompletedWindows = new HashSet<IWindowTransition>();
        /// <summary>
        /// Factory for creating new window transitions when new windows are added to the system.
        /// </summary>
        private IWindowTransitionFactory mWindowTransitionFactory;
        /// <summary>
        /// The manager which this transition works transition.
        /// </summary>
        private StateManager mManager;
        /// <summary>
        /// The trigger which will start this transition.
        /// </summary>
        private ITrigger mTrigger;
        /// <summary>
        /// The state the transition starts at.
        /// </summary>
        private IState mFrom;
        /// <summary>
        /// The state the transition goes to.
        /// </summary>
        private IState mTo;
        /// <summary>
        /// Whether the transition is in progress.
        /// </summary>
        private bool mInProgress;
        /// <summary>
        /// Whether the transition is active and should start when triggered.
        /// </summary>
        private bool mActive;

        /// <summary>
        /// Triggered when the transition has started.
        /// </summary>
        public event Action<StateTransition> Started;
        /// <summary>
        /// Triggered when the transition has finished.
        /// </summary>
        public event Action<StateTransition> Finished;

        /// <param name="manager">The manager this transition works transition.</param>
        public StateTransition(StateManager manager, IState from, IState to, ITrigger trigger, IWindowTransitionFactory factory) {
            mManager = manager;
            mFrom = from;
            mTo = to;
            mTrigger = trigger;
            mWindowTransitionFactory = factory;

            mTrigger.Triggered += new Action(mTrigger_Triggered);
            mManager.Coordinator.WindowAdded += new Action<Window,EventArgs>(Coordinator_WindowAdded);

            foreach (var window in mManager.Coordinator.Windows)
                Coordinator_WindowAdded(window, null);
        }

        /// <summary>
        /// Transitions for each window.
        /// </summary>
        public IWindowTransition[] WindowTransitions {
            get { return mWindowTransitions.Values.ToArray(); }
        }

        /// <summary>
        /// The state the transition goes transition.
        /// </summary>
        public IState From {
            get { return mFrom; }
        }

        /// <summary>
        /// The state the transition goes to.
        /// </summary>
        public IState To {
            get { return mTo; }
        }

        /// <summary>
        /// Trigger which will start the transition.
        /// </summary>
        public ITrigger Trigger {
            get { return mTrigger; }
        }

        /// <summary>
        /// Controller object to notify of the transition.
        /// </summary>
        public StateManager Manager {
            get { return mManager; }
        }

        /// <summary>
        /// Whether the trigger firing should start this transition.
        /// </summary>
        public bool Active {
            get { return mActive; }
            set { 
                mActive = value;
                mTrigger.Active = value;
            }
        }

        /// <summary>
        /// Whether the transition is currently in progress.
        /// </summary>
        public bool InProgress {
            get { return mInProgress; }
            set { mInProgress = value; }
        }

        /// <summary>
        /// Start the transition.
        /// </summary>
        public void Begin() {
            if (mActive) {
                mFrom.Active = false;
                mCompletedWindows.Clear();
                foreach (var windowTrans in mWindowTransitions.Values) {
                    windowTrans.Manager.CurrentDisplay = windowTrans;
                    windowTrans.Begin();
                }
                if (Started != null)
                    Started(this);
            }
        }

        void mTrigger_Triggered() {
            mManager.BeginTransition(this);
        }

        void Coordinator_WindowAdded(Window window, EventArgs args) {
            IWindowTransition transition = mWindowTransitionFactory.Create(this, window);
            mWindowTransitions.Add(window.Name, transition);
            transition.Finished += new Action<IWindowTransition>(transition_Finished);
        }

        void transition_Finished(IWindowTransition transition) {
            mCompletedWindows.Add(transition);
            transition.Manager.CurrentDisplay = transition.To;
            transition.Manager.ForceRedrawStatic();
            if (mCompletedWindows.Count == mWindowTransitions.Count) {
                mInProgress = false;
                if (Finished != null)
                    Finished(this);
            }
        }

        /// <summary>
        /// Cancel the transition that is currently happening.
        /// </summary>
        public void Cancel() {
            foreach (var windowTransition in mWindowTransitions.Values)
                windowTransition.Cancel();
        }
    }
}
