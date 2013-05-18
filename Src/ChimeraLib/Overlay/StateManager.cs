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

namespace Chimera.Overlay {
    public class StateManager {
        public static readonly string CLICK_MODE = "ClickBased";
        public static readonly string HOVER_MODE = "HoverBased";

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
                table += "<TABLE BORDER=\"1\">" + Environment.NewLine;
                table += "    <TR>" + Environment.NewLine;
                table += "        <TD>State Name</TD>" + Environment.NewLine;
                table += "        <TD># Visits</TD>" + Environment.NewLine;
                table += "        <TD>Time</TD>" + Environment.NewLine;
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
            foreach (var window in Coordinator.Windows) {
                window.OverlayManager.Close();
                window.OverlayManager.Launch();
            }
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
            foreach (var window in Coordinator.Windows)
                window.OverlayManager.ForceRedrawStatic();
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

        internal void Dump(string reason) {
            ProcessWrangler.Dump(Statistics, reason + ".html");
        }

        IEnumerable<ITriggerFactory> mTriggerFactories;
        Dictionary<string, IDrawable> mDrawables = new Dictionary<string, IDrawable>();
        Dictionary<string, ITrigger> mTriggers = new Dictionary<string, ITrigger>();
        Dictionary<string, IWindowTransitionFactory> mTransitionStyles = new Dictionary<string, IWindowTransitionFactory>();
        Dictionary<string, IHoverSelectorRenderer> mRenderers = new Dictionary<string, IHoverSelectorRenderer>();
        Rectangle mClip = new Rectangle(0, 0, 1920, 1080);
        private string mMode;

        private bool mClipLoaded;

        private void LoadClip(XmlDocument doc) {
            XmlNodeList clipList = doc.GetElementsByTagName("Overlay");
            if (clipList.Count == 0)
                return;

            XmlNode node = clipList[0];
            if (node.Attributes["Width"] != null && node.Attributes["Height"] != null) {
                mClip = new Rectangle(0, 0, XmlLoader.GetInt(node, mClip.Width, "X"), XmlLoader.GetInt(node, mClip.Height, "Y"));
                mClipLoaded = true;
            }
        }

        public void LoadXML(
                string xml, 
                string mode,
                IEnumerable<IDrawableFactory> drawableFactories, 
                IEnumerable<ITriggerFactory> triggerFactories, 
                IEnumerable<ISelectionRendererFactory> selectionRendererFactories, 
                IEnumerable<ITransitionStyleFactory> transitionStyleFactories, 
                IEnumerable<IStateFactory> stateFactories) {

            mTriggerFactories = triggerFactories;
            mMode = mode;

            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            LoadClip(doc);

            LoadComponent(doc, mode, drawableFactories, mDrawables, "Drawables");
            LoadComponent(doc, mode, triggerFactories, mTriggers, "Triggers");
            LoadComponent(doc, mode, transitionStyleFactories, mTransitionStyles, "TransitionStyles");
            LoadComponent(doc, mode, selectionRendererFactories, mRenderers, "SelectionRenderers");
            LoadComponent(doc, mode, stateFactories, mStates, "States");


            if (mRenderers.Count == 0)
                mRenderers.Add("CircleRenderer", new CircleRenderer());

            foreach (var state in mStates.Values)
                AddState(state);


            foreach (XmlNode transitionRoot in doc.GetElementsByTagName("Transitions"))
                foreach (XmlNode transitionNode in transitionRoot.ChildNodes)
                    LoadTransition(mode, transitionNode);
        }

        private void LoadComponent<T>(XmlDocument doc, string mMode, IEnumerable<IFactory<T>> factories, Dictionary<string, T> map, string nodeID) {
            foreach (XmlNode root in doc.GetElementsByTagName("Overlay")[0].ChildNodes)
                if (root.Name == "Any" || root.Name == mMode) {
                    XmlNode specificRoot = root.SelectSingleNode("child::" + nodeID);
                    if (specificRoot != null) {
                    foreach (XmlNode node in specificRoot.ChildNodes)
                        LoadFactory(node, factories, map);
                    }
                }
        }

        private void LoadTransition(
                string mMode,
                XmlNode node) {
            XmlAttribute fromAttr = node.Attributes["From"];
            XmlAttribute toAttr = node.Attributes["To"];
            XmlAttribute transitionAttr = node.Attributes["Transition"];

            if (fromAttr == null) {
                Console.WriteLine("Unable to load transition. No From attribute specified.");
                return;
            }
            if (toAttr == null) {
                Console.WriteLine("Unable to load transition. No To attribute specified.");
                return;
            }
            if (transitionAttr == null) {
                Console.WriteLine("Unable to load transition. No Transition attribute specified.");
                return;
            }
            if (!mStates.ContainsKey(fromAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + fromAttr.Value + " is not a known state.");
                return;
            }
            if (!mStates.ContainsKey(toAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + toAttr.Value + " is not a known state.");
                return;
            }
            if (!mTransitionStyles.ContainsKey(transitionAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + transitionAttr.Value + " is not a known transition style.");
                return;
            }

            ITrigger trigger = GetTrigger(node);
            if (trigger != null) {
                StateTransition transition = new StateTransition(this, mStates[fromAttr.Value], mStates[toAttr.Value], trigger, mTransitionStyles[transitionAttr.Value]);
                transition.From.AddTransition(transition);
            }
        }

        private ITrigger GetSpecialTrigger(SpecialTrigger type, XmlNode node) {
            ITriggerFactory factory = mTriggerFactories.FirstOrDefault(f => f.Special == type && f.Mode == mMode);
            if (factory == null) {
                Console.WriteLine("Unable to load " + type + " trigger. No trigger factory mapped for " + mMode + ". Check the ninject configuration file.");
                return null;
            }

            return mClipLoaded ? factory.Create(node, this, mClip) : factory.Create(node, this);
        }

        public ITrigger GetTrigger(XmlNode node) {
            switch (node.Name) {
                case "InvisibleTransition": 
                case "InvisibleTrigger": 
                    return GetSpecialTrigger(SpecialTrigger.Invisible, node);
                case "ImageTransition": 
                case "ImageTrigger": 
                    return GetSpecialTrigger(SpecialTrigger.Image, node);
                case "TextTransition": 
                case "TextTrigger": 
                    return GetSpecialTrigger(SpecialTrigger.Text, node);
            }

            XmlAttribute triggerAttr = node.Attributes["Trigger"];
            if (triggerAttr == null) {
                Console.WriteLine("Unable to load transition. No trigger attribute specified."); 
                return null;
            }
            if (!mTriggers.ContainsKey(triggerAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + triggerAttr.Value + " is not a known trigger.");
                return null;
            }

            return mTriggers[triggerAttr.Value];
        }

        public IHoverSelectorRenderer GetRenderer(XmlNode node) {
            XmlAttribute rendererNode = node.Attributes["Renderer"];
            if (rendererNode == null || !mRenderers.ContainsKey(rendererNode.Value))
                return mRenderers.FirstOrDefault().Value;

            return mRenderers[rendererNode.Value];
        }

        public IDrawable GetDrawable(XmlNode node) {
            XmlAttribute drawableNode = node.Attributes["Drawalbe"];
            if (drawableNode == null || !mRenderers.ContainsKey(drawableNode.Value))
                return mDrawables.FirstOrDefault().Value;

            return mDrawables[drawableNode.Value];
        }

        private void LoadFactory<T>(XmlNode node, IEnumerable<IFactory<T>> factories, Dictionary<string, T> instances) {
            XmlAttribute typeAttr = node.Attributes["Type"];
            XmlAttribute nameAttr = node.Attributes["Name"];
            if (nameAttr == null) {
                Console.WriteLine("Unable to load " + node.Name + ". No Name attribute specified.");
                return;
            }
            if (typeAttr == null) {
                Console.WriteLine("Unable to load " + node.Name + " " + nameAttr.Value + ". No Type attribute specified.");
                return;
            }
            IFactory<T> factory = factories.FirstOrDefault(f => f.Name == typeAttr.Value);
            if (factory == null) {
                Console.WriteLine("Unable to load " + node.Name + " " + nameAttr.Value + ". Type " + typeAttr.Value + " is not mapped to a " + node.Name + " factory. Check Ninject config to make sure the binding is correct.");
                return;
            }

            T t = mClipLoaded ? factory.Create(node, this, mClip) : factory.Create(node, this);
            instances.Add(nameAttr.Value, t);
        }

        public OverlayImage MakeImage(XmlNode node) {
            return mClipLoaded ?  new OverlayImage(this, node, mClip) : new OverlayImage(this, node);
        }

        public Text MakeText(XmlNode node) {
            return mClipLoaded ?  new StaticText(this, node, mClip) : new StaticText(this, node);
        }
    }
}
