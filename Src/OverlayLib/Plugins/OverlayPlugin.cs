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
    public class OverlayPlugin : XmlLoader, ISystemPlugin {
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
        /// Window managers for each window in the system.
        /// </summary>
        private readonly Dictionary<string, WindowOverlayManager> mWindowManagers = new Dictionary<string, WindowOverlayManager>();

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

        void mCoordinator_WindowAdded(Window window, EventArgs args) {
            mWindowManagers.Add(window.Name, new WindowOverlayManager(this, window));
        }

        public WindowOverlayManager this[string windowName] {
            get { return mWindowManagers[windowName]; }
        }

        public WindowOverlayManager this[int windowIndex] {
            get { return mWindowManagers[mCoordinator.Windows[0].Name]; }
        }

        public WindowOverlayManager[] OverlayManagers {
            get { return mWindowManagers.Values.ToArray(); }
        }

        public string Statistics {
            get {
                string table = "";
                table += "<TABLE BORDER=\"1\">" + Environment.NewLine;
                table += "    <TR>" + Environment.NewLine;
                table += "        <TD>State Name</TD>" + Environment.NewLine;
                table += "        <TD># Visits</TD>" + Environment.NewLine;
                table += "        <TD>Time</TD>" + Environment.NewLine;
                table += "        <TD>Longest Visit (m)</TD>" + Environment.NewLine;
                table += "        <TD>Shortest Visit (m)</TD>" + Environment.NewLine;
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
                if (!mIdleEnabled) {
                    foreach (var trigger in mIdleTriggers)
                        trigger.Active = false;
                }
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
            foreach (var manager in OverlayManagers)
                manager.ForceRedrawStatic();
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

        public bool IdleEnabled {
            get { return mIdleEnabled; }
            set {
                mIdleEnabled = value;
                foreach (var trigger in mIdleTriggers)
                    trigger.Active = value;
            }
        }

        public IWindowTransitionFactory DefaultTransition {
            get { return mTransitionStyles.First().Value; }
        }

        public IImageTransitionFactory DefaultImageTransition {
            get { return mImageTransitions.First().Value; }
        }

        IEnumerable<IImageTransitionFactory> mImageTransitionFactories;
        IEnumerable<IDrawableFactory> mDrawableFactories;
        IEnumerable<ITriggerFactory> mTriggerFactories;
        IEnumerable<ISelectionRendererFactory> mSelectionRendererFactories;
        IEnumerable<ITransitionStyleFactory> mTransitionStyleFactories;
        IEnumerable<IStateFactory> mStateFactories;

        private OverlayConfig mConfig;

        public OverlayPlugin(
                IEnumerable<IImageTransitionFactory> imageTransitionFactories, 
                IEnumerable<IDrawableFactory> drawableFactories, 
                IEnumerable<ITriggerFactory> triggerFactories, 
                IEnumerable<ISelectionRendererFactory> selectionRendererFactories, 
                IEnumerable<ITransitionStyleFactory> transitionStyleFactories, 
                IEnumerable<IStateFactory> stateFactories) {

            this.mImageTransitionFactories = imageTransitionFactories;
            this.mDrawableFactories = drawableFactories;
            this.mTriggerFactories = triggerFactories;
            this.mSelectionRendererFactories = selectionRendererFactories;
            this.mTransitionStyleFactories = transitionStyleFactories;
            this.mStateFactories = stateFactories;

            mConfig = new OverlayConfig();
            mMode = mConfig.InterfaceMode;
        }

        private Dictionary<string, IDrawable> mDrawables = new Dictionary<string, IDrawable>();
        private Dictionary<string, ITrigger> mTriggers = new Dictionary<string, ITrigger>();
        private Dictionary<string, IWindowTransitionFactory> mTransitionStyles = new Dictionary<string, IWindowTransitionFactory>();
        private Dictionary<string, IHoverSelectorRenderer> mRenderers = new Dictionary<string, IHoverSelectorRenderer>();
        private Dictionary<string, IImageTransitionFactory> mImageTransitions = new Dictionary<string, IImageTransitionFactory>();

        private State mSplashState;
        private IWindowTransitionFactory mIdleSplashTransition;
        private IWindowTransitionFactory mSplashIdleTransition;

        private Rectangle mClip = new Rectangle(0, 0, 1920, 1080);
        private bool mClipLoaded;

        private string mMode;

        private bool mIdleEnabled;
        private State mIdleState;
        private List<ITrigger> mIdleTriggers = new List<ITrigger>();

        private void LoadClip(XmlDocument doc) {
            XmlNodeList clipList = doc.GetElementsByTagName("Overlay");
            if (clipList.Count == 0)
                return;

            XmlNode node = clipList[0];
            if (node.Attributes["Width"] != null && node.Attributes["Height"] != null) {
                mClip = new Rectangle(0, 0, GetInt(node, mClip.Width, "X"), GetInt(node, mClip.Height, "Y"));
                mClipLoaded = true;
            }
        }

        public void LoadXML(string xml) {

            foreach (var imageTrans in mImageTransitionFactories)
                mImageTransitions.Add(imageTrans.Name, imageTrans);

            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            LoadClip(doc);

            LoadComponent(doc, mSelectionRendererFactories, mRenderers, "SelectionRenderers");
            if (mRenderers.Count == 0)
                //TODO add renderer factories
                mRenderers.Add("CursorRenderer", new DialCursorRenderer());

            LoadComponent(doc, mDrawableFactories, mDrawables, "Drawables");
            LoadComponent(doc, mTriggerFactories, mTriggers, "Triggers");
            LoadComponent(doc, mTransitionStyleFactories, mTransitionStyles, "TransitionStyles");
            LoadComponent(doc, mStateFactories, mStates, "States");


            foreach (var state in mStates.Values)
                AddState(state);


            Console.WriteLine("Creating Transitions");
            foreach (XmlNode transitionRoot in doc.GetElementsByTagName("Transitions"))
                foreach (XmlNode transitionNode in transitionRoot.ChildNodes)
                    if (transitionNode is XmlElement)
                        LoadTransition(transitionNode);

            if (mSplashState != null && mIdleState != null)
                foreach (var state in mStates.Values)
                    if (state != mIdleState)
                        foreach (var trigger in mIdleTriggers)
                            mIdleState.AddTransition(new StateTransition(this, state, mIdleState, trigger, mSplashIdleTransition));

            IdleEnabled = mIdleEnabled;
        }

        private void LoadComponent<T>(XmlDocument doc, IEnumerable<IFactory<T>> factories, Dictionary<string, T> map, string nodeID) {
            foreach (XmlNode root in doc.GetElementsByTagName("Overlay")[0].ChildNodes)
                if (root is XmlElement && (root.Name == "Any" || root.Name == mMode)) {
                    XmlNode specificRoot = root.SelectSingleNode("child::" + nodeID);
                    if (specificRoot != null) {
                    foreach (XmlNode node in specificRoot.ChildNodes)
                        if (node is XmlElement)
                            LoadFactory(node, factories, map);
                    }
                }
        }

        private void LoadTransition(XmlNode node) {
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
            if (!mStates.ContainsKey(fromAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + fromAttr.Value + " is not a known state.");
                return;
            }
            if (!mStates.ContainsKey(toAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + toAttr.Value + " is not a known state.");
                return;
            }

            IWindowTransitionFactory style = GetTransition(node);
            if (style == null)
                return;

            ITrigger trigger = GetTrigger(node);
            if (trigger != null) {
                StateTransition transition = new StateTransition(this, mStates[fromAttr.Value], mStates[toAttr.Value], trigger, style);
                transition.From.AddTransition(transition);
            }
        }

        public IWindowTransitionFactory GetTransition(XmlNode node) {
            XmlAttribute transitionAttr = node.Attributes["Transition"];
            if (transitionAttr == null) {
                Console.WriteLine("Unable to load transition. No Transition attribute specified.");
                return null;
            }
            if (!mTransitionStyles.ContainsKey(transitionAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + transitionAttr.Value + " is not a known transition style.");
                return null;
            }
            return mTransitionStyles[transitionAttr.Value];
        }

        public IImageTransitionFactory GetImageTransition(XmlNode node, string reason) {
            if (node == null) {
                Console.WriteLine("Unable to get image transition for " + reason + ". No node.");
                return null;
            }
            XmlAttribute transitionAttr = node.Attributes["Transition"];
            if (transitionAttr == null) {
                Console.WriteLine("Unable to get image transition from " + node.Name + " for " + reason + ". No Transition attribute.");
                return null;
            }
            if (!mImageTransitions.ContainsKey(transitionAttr.Value)) {
                Console.WriteLine("Unable to get image transition from " + node.Name + " for " + reason + ". " + transitionAttr.Value + " is not a known transition.");
                return null;
            }
            return mImageTransitions[transitionAttr.Value];
        }

        //TODO Get<T>(XmlNode node, Dictionary<string,T> map, string reason, params string[] keys)

        private ITrigger GetSpecialTrigger(SpecialTrigger type, XmlNode node) {
            ITriggerFactory factory = mTriggerFactories.FirstOrDefault(f => f.Special == type && f.Mode == mMode);
            if (factory == null) {
                Console.WriteLine("Unable to load " + type + " trigger. No trigger factory mapped for " + mMode + ". Check the ninject configuration file.");
                return null;
            }

            return mClipLoaded ? factory.Create(this, node, mClip) : factory.Create(this, node);
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
            XmlAttribute typeAttr = node.Attributes["Type"];
            if (triggerAttr == null && typeAttr == null) {
                Console.WriteLine("Unable to load trigger for " + node.Name + ". No trigger or type attribute specified."); 
                return null;
            }
            if (typeAttr != null) {
                IFactory<ITrigger> factory = GetFactory("Trigger", node, mTriggerFactories);
                if (factory == null)
                    return null;
                return Create(factory, node);
            }
            if (!mTriggers.ContainsKey(triggerAttr.Value)) {
                Console.WriteLine("Unable to load trigger for " + node.Name + ". " + triggerAttr.Value + " is not a known trigger.");
                return null;
            }
            return mTriggers[triggerAttr.Value];
        }

        private T Create<T>(IFactory<T> factory, XmlNode node) {
            return mClipLoaded ? factory.Create(this, node, mClip) : factory.Create(this, node);
        }

        public IHoverSelectorRenderer GetRenderer(XmlNode node) {
            XmlAttribute rendererNode = node.Attributes["Renderer"];
            if (rendererNode == null || !mRenderers.ContainsKey(rendererNode.Value))
                return mRenderers.First().Value;

            return mRenderers[rendererNode.Value];
        }

        public IDrawable GetDrawable(XmlNode node) {
            XmlAttribute drawableNode = node.Attributes["Drawalbe"];
            if (drawableNode == null || !mRenderers.ContainsKey(drawableNode.Value))
                return mDrawables.FirstOrDefault().Value;

            return mDrawables[drawableNode.Value];
        }

        private void LoadFactory<T>(XmlNode node, IEnumerable<IFactory<T>> factories, Dictionary<string, T> instances) {
            XmlAttribute nameAttr = node.Attributes["Name"];
            if (nameAttr == null) {
                Console.WriteLine("Unable to load " + node.Name + ". No Name attribute specified.");
                return;
            }

            IFactory<T> factory = GetFactory(nameAttr.Value, node, factories);
            if (factory == null)
                return;
            T t = Create(factory, node);
            instances.Add(nameAttr.Value, t);

            if (typeof(T) == typeof(State)) {
                if (GetBool(node, false, "Idle"))
                    LoadIdle(node, t as State);
                else if (GetBool(node, false, "Splash"))
                    mSplashState = t as State;
            }
        }

        private IFactory<T> GetFactory<T>(string name, XmlNode node, IEnumerable<IFactory<T>> factories) {
            XmlAttribute typeAttr = node.Attributes["Type"];
            if (typeAttr == null) {
                Console.WriteLine("Unable to load " + node.Name + " " + name + ". No Type attribute specified.");
                return null;
            }
            IFactory<T> factory = factories.FirstOrDefault(f => f.Name == typeAttr.Value);
            if (factory == null) {
                Console.WriteLine("Unable to load " + node.Name + " " + name + ". Type " + typeAttr.Value + " is not mapped to a " + node.Name + " factory. Check Ninject config to make sure the binding is correct.");
            }
            return factory;
        }

        private void LoadIdle(XmlNode node, State state) {
            mIdleState = state;
            foreach (XmlNode child in node.ChildNodes) {
                if (child is XmlElement) {
                    switch (child.Name) {
                        case "IdleTransition": mSplashIdleTransition = GetTransition(child); return;
                        case "SplashTransition": mIdleSplashTransition = GetTransition(child); return;
                        default: LoadIdleTrigger(child, GetTrigger(child)); return;
                    }
                }
            }
        }

        private void LoadIdleTrigger(XmlNode node, ITrigger trigger) {
            if (trigger != null)
                mIdleTriggers.Add(trigger);
        }

        public OverlayImage MakeImage(XmlNode node) {
            return mClipLoaded ?  new OverlayImage(this, node, mClip) : new OverlayImage(this, node);
        }

        public Text MakeText(XmlNode node) {
            return mClipLoaded ?  new StaticText(this, node, mClip) : new StaticText(this, node);
        }

        public State GetState(string state) {
            return mStates.ContainsKey(state) ? mStates[state] : null;
        }

        public RectangleF GetBounds(XmlNode node, string reason) {
            return mClipLoaded ? GetBounds(node, reason, mClip) : GetBounds(node, reason);
        }

        private bool mEnabled;

        #region ISystemInput members

        public event Action<IPlugin, bool> EnabledChanged;

        public System.Windows.Forms.UserControl ControlPanel {
            get { throw new NotImplementedException(); }
        }

        public bool Enabled {
            get {
                return mEnabled;
            }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "Overlay"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { return mConfig; }
        }
        
        /// <summary>
        /// CreateWindowState the manager. Linking it with a coordinator.
        /// </summary>
        /// <param name="coordinator">The coordinator which this state form manages state for.</param>
        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            mTransitionComplete = new Action<StateTransition>(transition_Finished);
            mCoordinator.WindowAdded += new Action<Window,EventArgs>(mCoordinator_WindowAdded);

            if (mConfig.OverlayFile != null)
                LoadXML(mConfig.OverlayFile);
        }

        public void Close() { }

        public void Draw(Func<OpenMetaverse.Vector3, Point> to2D, Graphics graphics, Action redraw, Perspective perspective) { }

        #endregion
    }
}
