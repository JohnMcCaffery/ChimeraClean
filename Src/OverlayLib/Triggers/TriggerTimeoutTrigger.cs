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

namespace Chimera.Overlay.Triggers
{
    public class TriggerTimeoutTriggerFactory : ITriggerFactory
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
            return new TriggerTimeoutTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "TriggerTimeout"; }
        }
    }

    public class TriggerTimeoutTrigger : TriggerBase, ITrigger
    {
        private bool mActive = false;
        private Core mCore;
        private ITrigger[] mTriggers;
        private Action<ITrigger> mTriggerListener;
        private double mLengthMS = 5000;
        private Action mTickListener;
        private DateTime mStart;

        public TriggerTimeoutTrigger(OverlayPlugin plugin, XmlNode node)
            : base(node)
        {

            mTriggerListener = new Action<ITrigger>(TriggerListener);
            mCore = plugin.Core;
            List<ITrigger> triggers = new List<ITrigger>();
            foreach (XmlNode trigger in GetChildrenOfChild(node, "Triggers"))
            {
                ITrigger t = plugin.GetTrigger(trigger, "TriggerTimeout trigger", null);
                if (t != null)
                {
                    triggers.Add(t);
                }
            }
            mTriggers = triggers.ToArray();
            mTickListener = new Action(mCore_Tick);
            mLengthMS = GetDouble(node, mLengthMS, "LengthMS");
        }

        void mCore_Tick()
        {
            if (DateTime.UtcNow.Subtract(mStart).TotalMilliseconds > mLengthMS)
            {
                mStart = DateTime.UtcNow;
                mCore.Tick -= mTickListener;
                Trigger();
            }
        }

        public void TriggerListener(ITrigger source)
        {
            mStart = DateTime.UtcNow;
        }

        public override bool Active
        {
            get { return mActive; }
            set
            {
                if (mActive != value)
                {
                    mActive = value;
                    foreach (var trigger in mTriggers)
                        if (value)
                        {
                            trigger.Active = true;
                            trigger.Triggered += mTriggerListener;
                        }
                        else
                        {
                            trigger.Triggered -= mTriggerListener;
                            trigger.Active = false;
                        }
                    if (value)
                    {
                        mStart = DateTime.UtcNow;
                        mCore.Tick += mTickListener;
                    }
                    else
                        mCore.Tick -= mTickListener;
                }
            }
        }
    }
}
