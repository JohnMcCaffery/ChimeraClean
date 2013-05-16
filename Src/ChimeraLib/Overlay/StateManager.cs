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
using Chimera.Interfaces.Overlay;
using System.IO;
using Chimera.Util;
using System.Xml;
using Chimera.Overlay.Drawables;
using System.Drawing;
using Chimera.Overlay.Triggers;

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

        private static readonly bool USE_CLICKS_DEFAULT = false;

        private bool GetUseClicks(XmlDocument doc) {
            XmlNodeList overlayNodes = doc.GetElementsByTagName("Overlay");
            if (overlayNodes.Count == 0)
                return USE_CLICKS_DEFAULT;
            XmlNode overlayNode = overlayNodes[0];
            if (overlayNode == null)
                return USE_CLICKS_DEFAULT;
            return GetUseClicks(overlayNode);
        }

        private bool GetUseClicks(XmlNode node) {
            XmlAttribute useClicksAttr = node.Attributes["UseClicks"];
            if (useClicksAttr == null)
                return USE_CLICKS_DEFAULT;

            return bool.Parse(useClicksAttr.Value);
        }

        Dictionary<string, ITrigger> mTriggers = new Dictionary<string, ITrigger>();
        Dictionary<string, IWindowTransitionFactory> mTransitionStyles = new Dictionary<string, IWindowTransitionFactory>();
        Dictionary<string, IHoverSelectorRenderer> mRenderers = new Dictionary<string, IHoverSelectorRenderer>();
        Rectangle mClip = new Rectangle(0, 0, 1920, 1080);

        private void LoadClip(XmlDocument doc) {
            XmlNodeList clipList = doc.GetElementsByTagName("Overlay");
            if (clipList.Count == 0)
                return;

            XmlNode node = clipList[0];
            if (node.Attributes["Width"] != null && node.Attributes["Height"] != null)
                mClip = new Rectangle(0, 0, GetInt(node, "X", mClip.Width), GetInt(node, "Y", mClip.Height));
        }

        public void LoadXML(
                string xml, 
                IEnumerable<ITriggerFactory> triggerFactories, 
                IEnumerable<ISelectionRendererFactory> selectionRendererFactories, 
                IEnumerable<ITransitionStyleFactory> transitionStyleFactories, 
                IEnumerable<IStateFactory> stateFactories) {

            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            bool useClicks = GetUseClicks(doc);
            LoadClip(doc);

            LoadComponent(doc, triggerFactories, mTriggers, "Triggers", useClicks);
            LoadComponent(doc, transitionStyleFactories, mTransitionStyles, "States", useClicks);
            LoadComponent(doc, selectionRendererFactories, mRenderers, "SelectionRenderers", useClicks);
            LoadComponent(doc, stateFactories, mStates, "States", useClicks);


            if (mRenderers.Count == 0)
                mRenderers.Add("CircleRenderer", new CircleRenderer());

            foreach (var state in mStates.Values)
                AddState(state);


            foreach (XmlNode transitionRoot in doc.GetElementsByTagName("Transitions"))
                foreach (XmlNode transitionNode in transitionRoot.ChildNodes)
                    LoadTransition(useClicks, transitionNode);
        }

        private void LoadComponent<T>(XmlDocument doc, IEnumerable<IFactory<T>> factories, Dictionary<string, T> map, string nodeID, bool useClicks) {
            foreach (XmlNode root in doc.GetElementsByTagName(nodeID))
                foreach (XmlNode node in root.ChildNodes)
                    if (useClicks == GetUseClicks(node))
                        LoadFactory(node, factories, map);
        }

        private void LoadTransition(
                bool useClicks,
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
            if (!mStates.ContainsKey(transitionAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + transitionAttr.Value + " is not a known transition style.");
                return;
            }

            ITrigger trigger = GetTrigger(useClicks, node);
            if (trigger != null) {
                StateTransition transition = new StateTransition(this, mStates[fromAttr.Value], mStates[toAttr.Value], trigger, mTransitionStyles[transitionAttr.Value]);
                transition.From.AddTransition(transition);
            }
        }

        private ITrigger GetTrigger(bool useClicks, XmlNode node) {
            switch (node.Name) {
                case "InvibleTransition": return MakeInvisibleTrigger(useClicks, node);
                case "ImageTransition": return MakeImageTrigger(useClicks, node);
                case "TextTransition": return MakeTextTrigger(useClicks, node);
            }

            XmlAttribute triggerAttr = node.Attributes["Trigger"];
            if (triggerAttr == null) {
                Console.WriteLine("Unable to load transition. No trigger attribute specified."); 
                return null;
            }            if (!mTriggers.ContainsKey(triggerAttr.Value)) {
                Console.WriteLine("Unable to load transition. " + triggerAttr.Value + " is not a known trigger.");
                return null;
            }

            return mTriggers[triggerAttr.Value];
        }

        private static readonly string DEFAULT_FONT = "Verdana";
        private static readonly float DEFAULT_FONT_SIZE = 12f;
        private static readonly FontStyle DEFAULT_FONT_STYLE = FontStyle.Regular;
        private static readonly Color DEFAULT_FONT_COLOUR = Color.Black;

        private float GetFloat(XmlNode node, string attribute, float defalt) {
            float t = defalt;
            if (node == null || node.Attributes[attribute] != null && float.TryParse(node.Attributes[attribute].Value, out t))
                return defalt;
            return t;
        }

        private int GetInt(XmlNode node, string attribute, int defalt) {
            int t = defalt;
            if (node == null || node.Attributes[attribute] != null && int.TryParse(node.Attributes[attribute].Value, out t))
                return defalt;
            return t;
        }

        private WindowOverlayManager GetWindow(XmlNode node) {
            XmlAttribute windowAttr = node.Attributes["Window"];
            if (windowAttr == null)
                return Coordinator.Windows.FirstOrDefault().OverlayManager;

            return Coordinator.Windows.FirstOrDefault(w => w.Name == windowAttr.Value).OverlayManager;
        }

        private IHoverSelectorRenderer GetRenderer(XmlNode node) {
            XmlAttribute rendererNode = node.Attributes["Renderer"];
            if (rendererNode == null || !mRenderers.ContainsKey(rendererNode.Value))
                return mRenderers.FirstOrDefault().Value;

            return mRenderers[rendererNode.Value];
        }

        private ITrigger MakeTextTrigger(bool useClicks, XmlNode node) {
            string text = node.InnerText;
            string window = node.Attributes["Window"].Value;

            FontStyle style = DEFAULT_FONT_STYLE;
            Color colour = DEFAULT_FONT_COLOUR;
            FontStyle styleT;

            string fontName = node.Attributes["Font"] != null ? node.Attributes["Name"].Value : DEFAULT_FONT;
            float size = GetFloat(node, "Size", DEFAULT_FONT_SIZE);
            if (node.Attributes["Style"] != null && Enum.TryParse<FontStyle>(node.Attributes["Style"].Value, true, out styleT))
                style = styleT;
            if (node.Attributes["Colour"] != null)
                colour = Color.FromName(node.Attributes["Colour"].Value);

            float x = GetFloat(node, "X", 0f);
            float y = GetFloat(node, "Y", 0f);

            Font f = new Font(fontName, size, style);
            Text txt = new StaticText(text, window, f, colour, new PointF(x, y));
            WindowOverlayManager manager = GetWindow(node);
                return useClicks ?
                    (ITrigger)new TextClickTrigger(manager, txt, mClip) :
                    (ITrigger)new TextHoverTrigger(manager, GetRenderer(node), txt, mClip);
        }

        private ITrigger MakeImageTrigger(bool useClicks, XmlNode node) {
            float x = GetFloat(node, "X", 0f);
            float y = GetFloat(node, "Y", 0f);
            float w = GetFloat(node, "W", .1f);
            if (node.Attributes["File"] == null) {
                Console.WriteLine("Unable to load image trigger. No file specified.");
                return null;
            }
            string file = Path.GetFullPath(node.Attributes["File"].Value);
            if (!File.Exists(file)) {
                Console.WriteLine("Unable to load image trigger. " + file + " does not exist.");
                return null;
            }
            WindowOverlayManager manager = GetWindow(node);
            OverlayImage img = new OverlayImage(new Bitmap(file), x, y, w, manager.Window.Name);
            return useClicks ? 
                (ITrigger) new ImageClickTrigger(manager, img) :
                (ITrigger) new ImageHoverTrigger(manager, GetRenderer(node), img);
        }

        private ITrigger MakeInvisibleTrigger(bool useClicks, XmlNode node) {            int left = GetInt(node, "Left", 0);
            int right = GetInt(node, "Right", 0);
            int top = GetInt(node, "Top", mClip.Width / 10);
            int bottom = GetInt(node, "Bottom", mClip.Height / 10);

            WindowOverlayManager manager = GetWindow(node);
            return useClicks ?
                (ITrigger) new ClickTrigger(manager, left, top, right - left, bottom - top, mClip) :
                (ITrigger) new InvisibleHoverTrigger(
                manager, GetRenderer(node), left, top, right - left, bottom - top, mClip);
        }

        private void LoadFactory<T>(XmlNode node, IEnumerable<IFactory<T>> factories, Dictionary<string, T> triggers) {
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
                Console.WriteLine("Unable to load " + node.Name + " " + nameAttr.Value + ". Type " + typeAttr.Value + " is not mapped to a trigger factory. Check Ninject config to make sure the binding is correct.");
                return;
            }

            T t = factory.Create(node, Coordinator);
            triggers.Add(nameAttr.Value, t);
        }
    }
}
