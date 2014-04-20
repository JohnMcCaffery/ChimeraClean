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
using System.Xml;
using log4net;

namespace Chimera.Overlay.Triggers {
    public class MultiTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return "None"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new MultiTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "Multi"; }
        }
    }

    public class MultiTrigger : TriggerBase, ITrigger {
        private bool mActive = false;
        private Core mCore;
        private ITrigger[] mTriggers;
        private Dictionary<string, bool> mTriggerStates;
        private Action<ITrigger> mTriggerListener;
        private int numTriggered;
        private readonly ILog Logger = LogManager.GetLogger("Trigger.MultiTrigger");

         public MultiTrigger(OverlayPlugin plugin, XmlNode node)
            : base(node) {

                mTriggerListener = new Action<ITrigger>(TriggerListener);
            mCore = plugin.Core;
            List<ITrigger> triggers = new List<ITrigger>();
            mTriggerStates = new Dictionary<string, bool>();
            foreach (XmlNode trigger in GetChildrenOfChild(node, "Triggers")) {
                ITrigger t = plugin.GetTrigger(trigger, "Multi trigger", null);
                if (t != null) {
                    triggers.Add(t);
                    Logger.InfoFormat("Adding trigger {0}", t.Name);
                }
            }
            mTriggers = triggers.ToArray();
            numTriggered = 0;
        }

         public void TriggerListener(ITrigger source) {
             Logger.InfoFormat("{0} triggered", source.Name);
             bool state;
             if (!mTriggerStates.TryGetValue(source.Name, out state)) {
                 mTriggerStates.Add(source.Name, true);
                 numTriggered++;
             }
             Logger.InfoFormat("Num triggerd {0} out of {1}", numTriggered, mTriggers.Length);
             if (numTriggered == mTriggers.Length)
                 Trigger();
         }

         public override bool Active {
             get { return mActive; }
             set {
                 Logger.InfoFormat("Multi trigger from {0} to {1}", mActive, value);
                 if (mActive != value) {
                     mActive = value;
                     foreach (var trigger in mTriggers)
                         if (value) {
                             trigger.Active = true;
                             trigger.Triggered += mTriggerListener;
                         } else {
                             trigger.Triggered -= mTriggerListener;
                             trigger.Active = false;
                         }
                     mTriggerStates.Clear();
                     numTriggered = 0;
                 }
             }
         }
    }
}
