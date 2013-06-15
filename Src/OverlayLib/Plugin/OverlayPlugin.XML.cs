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
using Chimera.Overlay.Features;
using System.Drawing;
using Chimera.Overlay.Triggers;
using System.Windows.Forms;
using Chimera.Overlay.GUI.Plugins;
using Chimera.Config;
using Chimera.Overlay.Transitions;
using Chimera.Interfaces;
using Chimera.Overlay.Interfaces;
using Chimera.Overlay.SelectionRenderers;

namespace Chimera.Overlay {
    public partial class OverlayPlugin : XmlLoader, ISystemPlugin {
        public static readonly string CLICK_MODE = "ClickBased";
        public static readonly string HOVER_MODE = "HoverBased";

        public IEnumerable<IImageTransitionFactory> mImageTransitionFactories;
        public IEnumerable<IFeatureFactory> mDrawableFactories;
        public IEnumerable<ITriggerFactory> mTriggerFactories;
        public IEnumerable<ISelectionRendererFactory> mSelectionRendererFactories;
        public IEnumerable<ITransitionStyleFactory> mTransitionStyleFactories;
        public IEnumerable<IStateFactory> mStateFactories;

        private Dictionary<string, IFeature> mFeatures = new Dictionary<string, IFeature>();
        private Dictionary<string, ITrigger> mTriggers = new Dictionary<string, ITrigger>();
        private Dictionary<string, ITransitionStyle> mTransitionStyles = new Dictionary<string, ITransitionStyle>();
        private Dictionary<string, ISelectionRenderer> mSelectionRenderers = new Dictionary<string, ISelectionRenderer>();
        private Dictionary<string, IImageTransition> mImageTransitions = new Dictionary<string, IImageTransition>();

        private State mStartState;
        private ITransitionStyle mSplashIdleTransition = new OpacityFadeOutWindowTransitionFactory(2000);

        private Rectangle mClip = new Rectangle(0, 0, 1920, 1080);
        private bool mClipLoaded;

        private string mMode;

        private bool mIdleEnabled;
        private State mIdleState;
        private List<ITrigger> mIdleTriggers = new List<ITrigger>();

        public Rectangle Clip {
            get { return mClip; }
        }

        public OverlayPlugin(
                IMediaPlayer player,
                IEnumerable<IImageTransitionFactory> imageTransitionFactories, 
                IEnumerable<IFeatureFactory> drawableFactories, 
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
            mIdleEnabled = mConfig.IdleEnabled;
            mMode = mConfig.InterfaceMode;
        }

        #region Loading

        public void LoadXML(string xml) {
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            LoadClip(doc);

            LoadComponent(doc, mSelectionRenderers, "SelectionRenderers");
            if (mSelectionRenderers.Count == 0)
                //TODO add renderer factories
                mSelectionRenderers.Add("CursorRenderer", new DialCursorRenderer(this));

            LoadComponent(doc, mImageTransitions, "ImageTransitions");
            LoadComponent(doc, mFeatures, "Features");
            LoadComponent(doc, mTriggers, "Triggers");
            LoadComponent(doc, mTransitionStyles, "TransitionStyles");
            LoadComponent(doc, mStates, "States");

            LoadTransitions(doc);
            LoadSpecialStates();
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

        private void LoadTransitions(XmlDocument doc) {
            Logger.Info("Creating Transitions");
            foreach (XmlNode transitionRoot in doc.GetElementsByTagName("Transitions"))
                foreach (XmlNode transitionNode in transitionRoot.ChildNodes.OfType<XmlElement>())
                    LoadTransition(transitionNode);
        }

        private void LoadSpecialStates() {
            if (mIdleState != null)
                foreach (var state in mStates.Values.Where(s => s != mIdleState))
                    foreach (var trigger in mIdleTriggers)
                        state.AddTransition(new StateTransition(this, state, mIdleState, trigger, mSplashIdleTransition));

            IdleEnabled = mIdleEnabled;
            if (mStartState != null)
                CurrentState = mStartState;
        }

        private void LoadComponent<T>(XmlDocument doc, Dictionary<string, T> map, string nodeID) where T : IControllable {
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
            State to = GetInstance(node, mStates, "state", "transition target", null, "To");
            ITrigger trigger = GetTrigger(node, "transition", null, "Trigger");
            ITransitionStyle style = GetTransition(node, "state transition", new BitmapWindowTransitionFactory(new BitmapFadeFactory(), 2000), "Transition");

            if (from == null || to == null || trigger == null) {
                Logger.Debug("Unable to create transition.");
                return;
            }

            StateTransition transition = new StateTransition(this, from, to, trigger, style);
            transition.From.AddTransition(transition);
        }

        private void LoadFactory<T>(XmlNode node, Dictionary<string, T> instances) where T : IControllable {
            XmlAttribute nameAttr = node.Attributes["Name"];
            if (nameAttr == null) {
                Logger.Debug("Unable to load " + node.Name + ". No Name attribute specified.");
                return;
            }

            IFactory<T> factory = GetFactory<T>(node, nameAttr.Value);
            if (factory == null)
                return;
            T t = Create(factory, node);
            t.Name = nameAttr.Value;
            if (typeof(T) != typeof(State)) {
                if (instances.ContainsKey(nameAttr.Value))
                    Logger.Debug(String.Format("Unable to add {0} {1} another {0} has been bound with that name.", typeof(T).Name.TrimStart('I'), nameAttr.Value));
                else
                    instances.Add(nameAttr.Value, t);
            } else
                LoadState(t as State, node);
        }

        private void LoadState(State state, XmlNode node) {
            AddState(state);
            if (GetBool(node, false, "Idle"))
                LoadIdle(node, state);
            if (GetBool(node, false, "Start"))
                mStartState = state;
            foreach (var child in GetChildrenOfChild(node, "Features")) {
                IFeature f = GetFeature(child, "state feature", null);
                if (f != null)
                    state.AddFeature(f);
            }
            Logger.Info("Created " + state.GetType().Name + " state " + state.Name + ".");
        }

        private void LoadIdle(XmlNode node, State state) {
            mIdleState = state;
            foreach (XmlNode child in node.ChildNodes.OfType<XmlElement>()) {
                switch (child.Name) {
                    case "IdleTransition": mSplashIdleTransition = GetTransition(child, "to idle transition", new OpacityFadeOutWindowTransitionFactory(5000)); break;
                }
            }
            foreach (var child in GetChildrenOfChild(node, "IdleTriggers")) {
                ITrigger trigger = GetTrigger(child, "idle state trigger", null);
                if (trigger != null)
                    mIdleTriggers.Add(trigger);
            }
        }

        #endregion

        #region Specific Getters

        public ITransitionStyle[] Transitions {
            get { return mTransitionStyles.Values.ToArray(); }
        }

        public IImageTransition[] ImageTransitions {
            get { return mImageTransitions.Values.ToArray(); }
        }

        public ISelectionRenderer[] Renderers {
            get { return mSelectionRenderers.Values.ToArray(); }
        }

        public IFeature[] Features {
            get { return mFeatures.Values.ToArray(); }
        }

        public ITrigger[] Triggers {
            get { return mTriggers.Values.ToArray(); }
        }

        public RectangleF GetBounds(XmlNode node, string reason) {
            return mClipLoaded ? GetBounds(node, reason, mClip) : GetBounds(node, reason);
        }

        public OverlayImage MakeImage(XmlNode node, string reason) {
            return mClipLoaded ?  new OverlayImage(this, node, mClip, reason) : new OverlayImage(this, node, reason);
        }

        public Text MakeText(XmlNode node) {
            return mClipLoaded ?  new StaticText(this, node, mClip) : new StaticText(this, node);
        }

        public ITransitionStyle GetTransition(XmlNode node, string reason, ITransitionStyle defalt, params string[] attributes) {
            return GetInstance(node, mTransitionStyles, "transition style", reason, defalt, attributes);
        }

        public IImageTransition GetImageTransition(XmlNode node, string reason, IImageTransition defalt, params string[] attributes) {
            return GetInstance(node, mImageTransitions, "image transition", reason, defalt, attributes);
        }

        public ISelectionRenderer GetRenderer(XmlNode node, string reason, ISelectionRenderer defalt, params string[] attributes) {
            return GetInstance(node, mSelectionRenderers, "hover selection renderer", reason, defalt, attributes);
        }

        public IFeature GetFeature(XmlNode node, string reason, IFeature defalt, params string[] attributes) {
            return GetInstance(node, mFeatures, "feature", reason, defalt, attributes);
        }

        public ITrigger GetTrigger(XmlNode node, string reason, ITrigger defalt, params string[] attributes) {
            if (node.Name.Contains("Invis"))
                    return GetSpecialTrigger(SpecialTrigger.Invisible, node);
            if (node.Name.Contains("Image"))
                    return GetSpecialTrigger(SpecialTrigger.Image, node);
            if (node.Name.Contains("Text"))
                    return GetSpecialTrigger(SpecialTrigger.Text, node);
            return GetInstance(node, mTriggers, "trigger", reason, defalt, attributes);
        }

        private ITrigger GetSpecialTrigger(SpecialTrigger type, XmlNode node) {
            ITriggerFactory factory = mTriggerFactories.FirstOrDefault(f => f.Special == type && f.Mode == mMode);
            if (factory == null) {
                Logger.Debug("Unable to load " + type + " trigger from " + node.Name + ". No trigger factory mapped for " + mMode + ". Check the ninject configuration file.");
                return null;
            }

            return mClipLoaded ? factory.Create(this, node, mClip) : factory.Create(this, node);
        }

        public IImageTransitionFactory GetImageTransitionFactory(XmlNode node, string reason, params string[] attributes) {
            return (IImageTransitionFactory) GetFactory<IImageTransition>(node, reason, attributes);
        }

        public ITriggerFactory GetTriggerFactory(XmlNode node, string reason, params string[] attributes) {
            return (ITriggerFactory) GetFactory<ITrigger>(node, reason, attributes);
        }

        public ISelectionRendererFactory GetSelectionRendererFactory(XmlNode node, string reason, params string[] attributes) {
            return (ISelectionRendererFactory) GetFactory<ISelectionRenderer>(node, reason, attributes);
        }

        public IFeatureFactory GetDrawableFactory(XmlNode node, string reason, params string[] attributes) {
            return (IFeatureFactory) GetFactory<IFeature>(node, reason, attributes);
        }

        #endregion

        #region Generic Getters

        private T GetInstance<T>(XmlNode node, Dictionary<string, T> map, string target, string reason, T defalt, params string[] attributes) {
            string ifDefault = defalt != null ? " Using default: " + defalt.GetType().Name + "." : "";
            string unable = "Unable to get " + target + " for " + reason + " from " + node.Name + ". ";
            if (node == null) {
                Logger.Debug(unable + "No node." + ifDefault);
                return defalt;
            }

            if (attributes.Length > 0)
                return GetInstanceFromAttributes(node, map, defalt, reason, unable, ifDefault, attributes);
            return GetInstanceFromDefaults(node, map, defalt, reason, unable, ifDefault);
        }

        private T GetInstanceFromAttributes<T>(XmlNode node, Dictionary<string, T> map, T defalt, string reason, string unable, string ifDefault, string[] attributes) {
            var factories = GetFactories<T>();
            IEnumerable<XmlAttribute> attrs = attributes.Where(a => node.Attributes[a] != null).Select(a => node.Attributes[a]);
            if (attrs.Count() == 0) {
                string attributesL = attributes.Aggregate((sum, next) => sum + "','" + next);
                Logger.Debug(unable + "No '" + attributesL + "' attribute specified. " + ifDefault);
                return defalt;
            }
            foreach (var attribute in attrs) {
                if (map.ContainsKey(attribute.Value))
                    return map[attribute.Value];
                if (factories == null) continue;
                IFactory<T> factory = factories.FirstOrDefault(f => f.Name == attribute.Value);
                if (factory == null) continue;
                return Create(factory, node);
            }
            string values = attrs.Select(a => a.Value).Aggregate((sum, next) => sum + "','" + next);
            Logger.Debug(unable + "'" + values + "' did not map to any factories or instances." + ifDefault);
            return defalt;
        }

        private T GetInstanceFromDefaults<T>(XmlNode node, Dictionary<string, T> map, T defalt, string reason, string unable, string ifDefault) {
            XmlAttribute nameAttr = node.Attributes["Name"];
            XmlAttribute factoryAttr = node.Attributes["Factory"];
            //No attributes are specified - screw it
            if (nameAttr == null && factoryAttr == null) {
                Logger.Debug(unable + "No 'Name' or 'Factory' attribute mapped." + ifDefault);
                return defalt;
            }

            //Use name - name value is bound or name attribute exists and factory attribute isn't bound
            if (nameAttr != null && map.ContainsKey(nameAttr.Value))
                return map[nameAttr.Value];
            else if (factoryAttr == null) {
                Logger.Debug(unable + nameAttr.Value + " is not bound to a known instance. " + ifDefault);
                return defalt;
            }

            //Name attribute didn't get anywhere, use factory attribute
            IFactory<T> factory = GetFactory<T>(node, reason);
            if (factory == null)
                return defalt;
            return Create(factory, node);
        }


        public string[] GetFactoryNames<T>() {
            return GetFactories<T>().Select(f => f.Name).ToArray();
        }

        public IEnumerable<IFactory<T>> GetFactories<T>() {
            if (typeof(T).FullName == typeof(IFeature).FullName)
                return (IEnumerable<IFactory<T>>)mDrawableFactories;
            else if (typeof(T).FullName == typeof(ITransitionStyle).FullName)
                return (IEnumerable<IFactory<T>>)mTransitionStyleFactories;
            else if (typeof(T).FullName == typeof(IImageTransition).FullName)
                return (IEnumerable<IFactory<T>>)mImageTransitionFactories;
            else if (typeof(T).FullName == typeof(ISelectionRenderer).FullName)
                return (IEnumerable<IFactory<T>>)mSelectionRendererFactories;
            else if (typeof(T).FullName == typeof(State).FullName)
                return (IEnumerable<IFactory<T>>)mStateFactories;
            else if (typeof(T).FullName == typeof(ITrigger).FullName)
                return (IEnumerable<IFactory<T>>)mTriggerFactories;
            else {
                return null;
            }
        }

        private T Create<T>(IFactory<T> factory, XmlNode node) {
            return mClipLoaded ? factory.Create(this, node, mClip) : factory.Create(this, node);
        }

        private IFactory<T> GetFactory<T>(XmlNode node, string reason, params string[] attributes) {
            string unable = "Unable to get " + typeof(T).Name + " factory for " + reason + ". ";
            if (node == null) {
                Logger.Debug(unable + " No node.");
                return null;
            }
            IEnumerable<IFactory<T>> factories = GetFactories<T>();
            if (factories == null) {
                Logger.Debug(unable + ". No factories found for that type.");
                return null;
            }
            if (attributes.Length == 0)
                attributes = new string[] { "Factory" };
            XmlAttribute factoryAttr = attributes.Where(a => node.Attributes[a] != null).Select(a => node.Attributes[a]).FirstOrDefault();
            if (factoryAttr == null) {
                Logger.Debug(unable + " No Factory attribute specified.");
                return null;
            }
            IFactory<T> factory = factories.FirstOrDefault(f => f.Name == factoryAttr.Value);
            if (factory == null) {
                Logger.Debug(unable + factoryAttr.Value + " is not a mapped " + typeof(T).Name + " factory. Check Ninject config to make sure the binding exists.");
            }
            return factory;
        }

        #endregion
    }
}
