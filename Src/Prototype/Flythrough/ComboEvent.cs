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
using System.Xml;
using OpenMetaverse;

namespace Chimera.Flythrough {
    public class ComboEvent : FlythroughEvent {
        /// <summary>
        /// How many LookAtEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// The event currently playing.
        /// </summary>
        private FlythroughEvent mStream1Current;

        /// <summary>
        /// The first event in the sequence.
        /// </summary>
        private FlythroughEvent mStream1First;

        /// <summary>
        /// The last event in the sequence.
        /// </summary>
        private FlythroughEvent mStream1Last;

        /// <summary>
        /// The event currently playing.
        /// </summary>
        private FlythroughEvent mStream2Current;

        /// <summary>
        /// The first event in the sequence.
        /// </summary>
        private FlythroughEvent mStream2First;

        /// <summary>
        /// The last event in the sequence.
        /// </summary>
        private FlythroughEvent mStream2Last;

        /// <summary>
        /// True if stream 1 is currently playing.
        /// </summary>
        private bool mStream1Playing;
        /// <summary>
        /// True if stream 2 is currently playing.
        /// </summary>
        private bool mStream2Playing;
        /// <summary>
        /// The name of the event.
        /// </summary>
        private readonly string mName;

        /// <summary>
        /// Selected whenever a new event is started. The bool is true if the event is transition sequence one.
        /// </summary>
        public event System.Action<FlythroughEvent, bool> OnNextEvent;

        /// <summary>
        /// Selected whenever this event starts playing.
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// Selected whenever this event finishes playing. True if sequence one is finishing, false if sequence two is finishing.
        /// </summary>
        public event System.Action<bool> OnComplete;

        public override int Length {
            get {
                int stream1L = 0, stream2L = 0;
                FlythroughEvent s1 = mStream1First, s2 = mStream2First;
                while (s1 != null) { stream1L = s1.Length; s1 = s1.NextEvent; }
                while (s2 != null) { stream2L = s2.Length; s2 = s2.NextEvent; }
                return Math.Max(stream1L, stream2L);
            }
            set {
                base.Length = value;
            }
        }

        public FlythroughEvent Stream1First {
            get { return mStream1First; }
        }

        public FlythroughEvent Stream2First {
            get { return mStream2First; }
        }

        public override string Name {
            get { return mName; }
        }

        public FlythroughEvent Stream1Current {
            get { return mStream1Current; }
            set { 
                if (CurrentStep == 0)
                    mStream1Current = value;
            }
        }

        public FlythroughEvent Stream2Current {
            get { return mStream2Current; }
            set { 
                if (CurrentStep == 0)
                    mStream2Current = value;
            }
        }

        /// <summary>
        /// Initialise with no events in the streams.
        /// </summary>
        /// <param name="container">The container this event is part of.</param>
        public ComboEvent(FlythroughManager container)
            : base(container, (FlythroughManager.TICK_LENGTH * 2) + 1) {

            mName = "Combo " + (++COUNT);
        }

        /// <summary>
        /// Add a new event to the sequence.
        /// </summary>
        /// <param name="evt">The event to add.</param>
        public void AddStream1Event(FlythroughEvent evt) {
            if (mStream1First == null)
                mStream1First = evt;
            else
                mStream1Last.NextEvent = evt;

            evt.PrevEvent = mStream1Last;
            mStream1Last = evt;
        }

        /// <summary>
        /// Add a new event to the sequence.
        /// </summary>
        /// <param name="evt">The event to add.</param>
        public void AddStream2Event(FlythroughEvent evt) {
            if (mStream2First == null)
                mStream2First = evt;
            else
                mStream2Last.NextEvent = evt;

            evt.PrevEvent = mStream2Last;
            mStream2Last = evt;
        }

        public override bool Step() {
            if (CurrentStep == 0) {
                mStream1Playing = mStream1First != null;
                mStream2Playing = mStream2First != null;
                if (mStream1Current == null)
                    mStream1Current = mStream1First;
                if (mStream2Current == null)
                    mStream2Current = mStream2First;

                DoStep();

                if ((mStream1Playing || mStream2Playing) && OnStart != null)
                    OnStart(this, null);
            }

            if (mStream1Playing && !mStream1Current.Step()) {
                mStream1Current = mStream1Current.NextEvent;
                if (mStream1Current == null) {
                    mStream1Playing = false;
                    mStream1Current = null;
                    if (OnComplete != null)
                        OnComplete(false);
                } else if (OnNextEvent != null) {
                    OnNextEvent(mStream1Current, true);
                }
            }

            if (mStream2Playing && !mStream2Current.Step()) {
                mStream2Current = mStream2Current.NextEvent;
                if (mStream2Current == null) {
                    mStream2Playing = false;
                    mStream2Current = null;
                    if (OnComplete != null)
                        OnComplete(false);
                } else if (OnNextEvent != null) {
                    OnNextEvent(mStream1Current, false);
                }
            }

            if (!mStream1Playing && !mStream2Playing) {
                DoStep();
                return false;
            }

            return true;
        }

        protected override void LengthChanged() {
            //Do nothing
        }

        public void MoveUp(FlythroughEvent evt) {
            if (evt.PrevEvent != null) {
                FlythroughEvent one = evt.PrevEvent.PrevEvent;
                FlythroughEvent two = evt;
                FlythroughEvent three = evt.PrevEvent;
                FlythroughEvent four = evt.NextEvent;

                if (one != null)
                    one.NextEvent = two;

                two.PrevEvent = one;
                two.NextEvent = three;

                if (three != null) {
                    three.PrevEvent = two;
                    three.NextEvent = four;
                }

                if (four != null)
                    four.PrevEvent = three;

                if (three == mStream1First)
                    mStream1First = evt;
                if (three == mStream2First)
                    mStream2First = evt;

                if (evt == mStream1Last)
                    mStream1Last = evt.NextEvent;
                if (evt == mStream2Last)
                    mStream2Last = evt.NextEvent;
            }
        }

        public void RemoveEvent(FlythroughEvent evt, bool sequence1) {
            FlythroughEvent firstEvent = sequence1 ? mStream1First : mStream2First;
            FlythroughEvent lastEvent = sequence1 ? mStream1Last : mStream2Last;
            if (evt.PrevEvent != null) {
                evt.PrevEvent.NextEvent = evt.NextEvent;
                if (evt.PrevEvent.PrevEvent == null)
                    firstEvent = evt.PrevEvent;
            }

            if (evt.NextEvent != null) {
                evt.NextEvent.PrevEvent = evt.PrevEvent;
                if (evt.NextEvent.NextEvent == null)
                    lastEvent = evt.NextEvent;
            }
        }

        public override void Load(XmlNode node) {
            if (node.FirstChild != null) {
                LoadNode(node.FirstChild, true);
                if (node.FirstChild.NextSibling != null)
                    LoadNode(node.FirstChild.NextSibling, false);
            }
        }

        private void LoadNode(XmlNode root, bool sequence1) {
            foreach (XmlNode node in root.ChildNodes) {
                FlythroughEvent evt = null;
                switch (node.Name) {
                    case "ComboEvent": evt = new ComboEvent(Container); break;
                    case "RotateEvent": evt = new RotateEvent(Container, 0); break;
                    case "RotateToEvent": evt = new RotateToEvent(Container, 0); break;
                    case "MoveToEvent": evt = new MoveToEvent(Container, 0, Vector3.Zero); break;
                    case "CircleEvent": evt = new CircleEvent(Container, 0); break;
                    case "LookAtEvent": evt = new LookAtEvent(Container, 0); break;
                    case "BlankEvent": evt = new BlankEvent(Container, 0); break;
                }
                if (evt != null) {
                    evt.Load(node);
                    if (sequence1)
                        AddStream1Event(evt);
                    else
                        AddStream2Event(evt);
                }
            }
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode root = doc.CreateElement("ComboEvent");
            XmlNode stream1 = doc.CreateElement("Stream1");
            XmlNode stream2 = doc.CreateElement("Stream2");

            SaveStream(stream1, mStream1First);
            SaveStream(stream2, mStream2First);

            root.AppendChild(stream1);
            root.AppendChild(stream2);
            return root;
        }

        private void SaveStream(XmlNode root, FlythroughEvent firstEvt) {
            FlythroughEvent evt = firstEvt;
            while (evt != null) {
                root.AppendChild(evt.Save(root.OwnerDocument));
                evt = evt.NextEvent;
            }
        }

        public override void Reset() {
            base.Reset();
            if (mStream1Current != null)
                mStream1Current.Reset();
            if (mStream2Current != null)
                mStream2Current.Reset();
            mStream1Current = null;
            mStream2Current = null;
            mStream1Playing = false;
            mStream2Playing = false;
        }
    }
}
