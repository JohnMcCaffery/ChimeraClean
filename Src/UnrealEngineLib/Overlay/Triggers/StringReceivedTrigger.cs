using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Triggers;
using Chimera.Overlay;
using System.Xml;
using Chimera.Interfaces.Overlay;

namespace UnrealEngineLib.Overlay.Triggers {
    public class StringReceivedTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return "none"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new StringReceivedTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "UnrealStringReceived"; }
        }
    }
    public class StringReceivedTrigger : TriggerBase {
	
        private UnrealController mController;
        private Action<string> mTextReceivedTrigger;
	private string mString;
	private bool mActive;

	public StringReceivedTrigger(OverlayPlugin plugin, XmlNode node) : base (node) {

            mController = GetManager(plugin, node, "string received trigger").Frame.Output as UnrealController;
            if (mController == null)
                throw new Exception("Cannot use StringReceivedTrigger, UnrealController is not the controller.");

            mString = GetString(node, "", "String");
            mTextReceivedTrigger = (str) => {
                                if (mString == str)
                                    Trigger();
                            };
        }

        public override bool Active {
            get { return mActive; }
            set {
                if (value != mActive) {
                    if (value)
                        mController.StringReceived += mTextReceivedTrigger;
                    else
                        mController.StringReceived -= mTextReceivedTrigger;
			mActive = value;
                }
            }
        }
    }
}
