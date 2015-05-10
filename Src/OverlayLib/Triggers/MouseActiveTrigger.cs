using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Xml;
using log4net;

namespace Chimera.Overlay.Triggers
{
    public class MouseActiveTriggerFactory : ITriggerFactory
    {
        public SpecialTrigger Special
        {
            get { return SpecialTrigger.None; }
        }

        public string Mode
        {
            get { return "None"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node)
        {
            return new MouseActiveTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "MouseActive"; }
        }
    }

    public class MouseActiveTrigger : TriggerBase, ITrigger
    {
        private bool mActive;
        private Core mCore;
        private Action<FrameOverlayManager, EventArgs> mMoveListener;
        private readonly FrameOverlayManager mManager;
        private readonly ILog Logger = LogManager.GetLogger("Trigger.MouseActive");


        public MouseActiveTrigger(OverlayPlugin plugin, XmlNode node)
            : base(node)
        {
            mManager = GetManager(plugin, node, "trigger");
            mCore = plugin.Core;
            mMoveListener = new Action<FrameOverlayManager, EventArgs>(mManager_CursorMoved);
        }

        void mManager_CursorMoved(FrameOverlayManager manager, EventArgs args)
        {
            Trigger();
        }

        #region ITrigger Members

        public override bool Active
        {
            get { return mActive; }
            set
            {
                if (mActive != value)
                {
                    mActive = value;
                    if (value)
                    {
                        mManager.CursorMoved += mMoveListener;
                    }
                    else
                        mManager.CursorMoved -= mMoveListener;
                }
            }
        }

        #endregion
    }
}
