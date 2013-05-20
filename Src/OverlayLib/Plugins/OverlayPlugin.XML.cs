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

namespace Chimera.Overlay {
    public partial class OverlayPlugin : XmlLoader, ISystemPlugin {
        public static readonly string CLICK_MODE = "ClickBased";
        public static readonly string HOVER_MODE = "HoverBased";
        public IEnumerable<IImageTransitionFactory> mImageTransitionFactories;
        public IEnumerable<IDrawableFactory> mDrawableFactories;
        public IEnumerable<ITriggerFactory> mTriggerFactories;
        public IEnumerable<ISelectionRendererFactory> mSelectionRendererFactories;
        public IEnumerable<ITransitionStyleFactory> mTransitionStyleFactories;
        public IEnumerable<IStateFactory> mStateFactories;

        private Dictionary<string, IDrawable> mDrawables = new Dictionary<string, IDrawable>();
        private Dictionary<string, ITrigger> mTriggers = new Dictionary<string, ITrigger>();
        private Dictionary<string, IWindowTransitionFactory> mTransitionStyles = new Dictionary<string, IWindowTransitionFactory>();
        private Dictionary<string, IHoverSelectorRenderer> mRenderers = new Dictionary<string, IHoverSelectorRenderer>();
        private Dictionary<string, IImageTransition> mImageTransitions = new Dictionary<string, IImageTransition>();

        private State mSplashState;
        private IWindowTransitionFactory mIdleSplashTransition = new OpacityFadeInWindowTransitionFactory(2000);
        private IWindowTransitionFactory mSplashIdleTransition = new OpacityFadeOutWindowTransitionFactory(2000);

        private Rectangle mClip = new Rectangle(0, 0, 1920, 1080);
        private bool mClipLoaded;

        private string mMode;

        private bool mIdleEnabled;
        private State mIdleState;
        private List<ITrigger> mIdleTriggers = new List<ITrigger>();

        public OverlayPlugin(
                IMediaPlayer player,
                IEnumerable<IImageTransitionFactory> imageTransitionFactories, 
                IEnumerable<IDrawableFactory> drawableFactories, 
                IEnumerable<ITriggerFactory> triggerFactories, 
                IEnumerable<ISelectionRendererFactory> selectionRendererFactories, 
                IEnumerable<ITransitionStyleFactory> transitionStyleFactories, 
                IEnumerable<IStateFactory> stateFactories) {

            mPlayer = player; 
            mImageTransitionFactories = imageTransitionFactories;
            mDrawableFactories = drawableFactories;
            mTriggerFactories = triggerFactories;
            mSelectionRendererFactories = selectionRendererFactories;
            mTransitionStyleFactories = transitionStyleFactories;
            mStateFactories = stateFactories;

            mConfig = new OverlayConfig();
            mMode = mConfig.InterfaceMode;
        }

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
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            LoadClip(doc);

            LoadComponent(doc, mRenderers, "SelectionRenderers");
            if (mRenderers.Count == 0)
                //TODO add renderer factories
                mRenderers.Add("CursorRenderer", new DialCursorRenderer());

            LoadComponent(doc, mImageTransitions, "ImageTransitions");
            LoadComponent(doc, mDrawables, "Drawables");
            LoadComponent(doc, mTriggers, "Triggers");
            LoadComponent(doc, mTransitionStyles, "TransitionStyles");
            LoadComponent(doc, mStates, "States");


            foreach (var state in mStates.Values)
                AddState(state);

            LoadTransitions(doc);
            LoadSpeciStates();
        }

        private void LoadTransitions(XmlDocument doc) {            Console.WriteLine("Creating Transitions");
            foreach (XmlNode transitionRoot in doc.GetElementsByTagName("Transitions"))
                foreach (XmlNode transitionNode in transitionRoot.ChildNodes)
                    if (transitionNode is XmlElement)
                        LoadTransition(transitionNode);
        }

        private void LoadSpeciStates() {            if (mSplashState != null && mIdleState != null)
                foreach (var state in mStates.Values)
                    if (state != mIdleState)
                        foreach (var trigger in mIdleTriggers)
                            mIdleState.AddTransition(new StateTransition(this, state, mIdleState, trigger, mSplashIdleTransition));

            IdleEnabled = mIdleEnabled;
            if (mSplashState != null)
                CurrentState = mSplashState;
        }

        private void LoadComponent<T>(XmlDocument doc, Dictionary<string, T> map, string nodeID) {
            foreach (XmlNode root in doc.GetElementsByTagName("Overlay")[0].ChildNodes)
                if (root is XmlElement && (root.Name == "Any" || root.Name == mMode)) {
                    XmlNode specificRoot = root.SelectSingleNode("child::" + nodeID);
                    if (specificRoot != null) {
                    foreach (XmlNode node in specificRoot.ChildNodes)
                        if (node is XmlElement)
                            LoadFactory(node, map);
                    }
                }
        }

        private void LoadTransition(XmlNode node) {
            State from = GetInstance(node, mStates, "state", "transition start", null, "From");
            State to = GetInstance(node, mStates, "state", "transition start", null, "From");
            ITrigger trigger = GetTrigger(node, "transition");
            IWindowTransitionFactory style = GetTransition(node, "state transition", new BitmapWindowTransitionFactory(new BitmapFadeFactory(), 2000));

            if (from == null || to == null || trigger == null)
                return;

            StateTransition transition = new StateTransition(this, from, to, trigger, style);
            transition.From.AddTransition(transition);
        }

        private T GetInstance<T>(XmlNode node, Dictionary<string, T> map, string target, string reason, T defalt, params string[] attribute) {
            string ifDefault = defalt != null ? "Using default " + defalt.GetType().Name + "." : "";
            string unable = "Unable to get " + target + " for " + reason + ". ";
            if (node == null) {
                Console.WriteLine(unable + "No node." + ifDefault);
                return defalt;
            }
            XmlAttribute nameAttr = attribute.Where(a => node.Attributes[a] != null).Select(a => node.Attributes[a]).FirstOrDefault();
            XmlAttribute factoryAttr = node.Attributes["Factory"];
            if (nameAttr == null && factoryAttr == null) {
                Console.WriteLine(unable + "No '" + attribute + "' or 'Factory' attributes specified." + ifDefault);
                return defalt;
            }
            if (nameAttr != null) {
                if (!map.ContainsKey(nameAttr.Value)) {
                    Console.WriteLine(unable + nameAttr.Value + " is not a known " + target + "." + ifDefault);
                    return defalt;
                }
                return map[nameAttr.Value];
            }
            IEnumerable<IFactory<T>> factories = GetFactories<T>();
            if (factories == null) {
                Console.WriteLine(unable + "No factories bound for " + typeof(T).Name + "." + ifDefault);
                return defalt;
            }
            IFactory<T> factory = GetFactory<T>(factoryAttr.Value, factories);
            if (factory == null) {
                Console.WriteLine(unable + factoryAttr.Value + " is not a known " + target + " factory." + ifDefault);
                return defalt;
            }
            return Create(factory, node);
        }

        public IEnumerable<IFactory<T>> GetFactories<T>() {
            if (typeof(T).FullName == typeof(IDrawable).FullName)
                return (IEnumerable<IFactory<T>>)mDrawableFactories;
            else if (typeof(T).FullName == typeof(IWindowTransitionFactory).FullName)
                return (IEnumerable<IFactory<T>>)mDrawableFactories;
            else if (typeof(T).FullName == typeof(IImageTransition).FullName)
                return (IEnumerable<IFactory<T>>)mDrawableFactories;
            else if (typeof(T).FullName == typeof(IHoverSelectorRenderer).FullName)
                return (IEnumerable<IFactory<T>>)mDrawableFactories;
            else if (typeof(T).FullName == typeof(State).FullName)
                return (IEnumerable<IFactory<T>>)mDrawableFactories;
            else if (typeof(T).FullName == typeof(ITrigger).FullName)
                return (IEnumerable<IFactory<T>>)mDrawableFactories;
            else {
                return null;
            }
        }
        public IFactory<T> GetFactory<T>(string name, IEnumerable<IFactory<T>> factories ) {
            return factories.FirstOrDefault(f => f.Name == name);
        }
        private T Create<T>(IFactory<T> factory, XmlNode node) {
            return mClipLoaded ? factory.Create(this, node, mClip) : factory.Create(this, node);
        }

        public IWindowTransitionFactory GetTransition(XmlNode node, string reason, IWindowTransitionFactory defalt) {
            return GetInstance(node, mTransitionStyles, "transition style", reason, defalt, "Transition");
        }

        public IImageTransition GetImageTransition(XmlNode node, string reason, IImageTransition defalt) {
            return GetInstance(node, mImageTransitions, "image transition", reason, defalt, "Transition");
        }
        public IHoverSelectorRenderer GetRenderer(XmlNode node, string reason, IHoverSelectorRenderer defalt) {
            return GetInstance(node, mRenderers, "hover selection renderer", reason, defalt, "Renderer");
        }

        public IDrawable GetFeature(XmlNode node, string reason, IDrawable defalt) {
            return GetInstance(node, mDrawables, "feature", reason, defalt, "Feature");
        }

        public ITrigger GetTrigger(XmlNode node, string reason) {
            if (node.Name.Contains("Invis"))
                    return GetSpecialTrigger(SpecialTrigger.Invisible, node);
            if (node.Name.Contains("Image"))
                    return GetSpecialTrigger(SpecialTrigger.Image, node);
            if (node.Name.Contains("Text"))
                    return GetSpecialTrigger(SpecialTrigger.Text, node);
            return GetInstance(node, mTriggers, "trigger", reason, null, "Trigger");
        }

        private ITrigger GetSpecialTrigger(SpecialTrigger type, XmlNode node) {
            ITriggerFactory factory = mTriggerFactories.FirstOrDefault(f => f.Special == type && f.Mode == mMode);
            if (factory == null) {
                Console.WriteLine("Unable to load " + type + " trigger. No trigger factory mapped for " + mMode + ". Check the ninject configuration file.");
                return null;
            }

            return mClipLoaded ? factory.Create(this, node, mClip) : factory.Create(this, node);
        }

        private void LoadFactory<T>(XmlNode node, Dictionary<string, T> instances) {
            XmlAttribute nameAttr = node.Attributes["Name"];
            if (nameAttr == null) {
                Console.WriteLine("Unable to load " + node.Name + ". No Name attribute specified.");
                return;
            }

            IFactory<T> factory = GetFactory<T>(node, nameAttr.Value);
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

        public IFactory<T> GetFactory<T>(XmlNode node, string reason) {
            string unable = "Unable to get " + typeof(T).Name + " factory for " + reason + ". ";
            if (node == null) {
                Console.WriteLine(unable + " No node.");
                return null;
            }
            IEnumerable<IFactory<T>> factories = GetFactories<T>();
            if (factories == null) {
                Console.WriteLine(unable + ". No factories found for that type.");
                return null;
            }
            XmlAttribute factoryAttr = node.Attributes["Factory"];
            if (factoryAttr == null) {
                Console.WriteLine(unable + " No Factory attribute specified.");
                return null;
            }
            IFactory<T> factory = GetFactory<T>(factoryAttr.Value, factories);
            if (factory == null) {
                Console.WriteLine(unable + "There is no mapping for that name and factory. Check Ninject config to make sure the binding is correct.");
            }
            return factory;
        }

        private void LoadIdle(XmlNode node, State state) {
            mIdleState = state;
            foreach (XmlNode child in node.ChildNodes) {
                if (child is XmlElement) {
                    switch (child.Name) {
                        case "IdleTransition": mSplashIdleTransition = GetTransition(child, "to idle transition", new OpacityFadeOutWindowTransitionFactory(5000)); return;
                        case "SplashTransition": mIdleSplashTransition = GetTransition(child, "from idle transition", new OpacityFadeInWindowTransitionFactory(5000)); return;
                        default: LoadIdleTrigger(child, GetTrigger(child, "idle transition")); return;
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

        public RectangleF GetBounds(XmlNode node, string reason) {
            return mClipLoaded ? GetBounds(node, reason, mClip) : GetBounds(node, reason);
        }
    }
}
