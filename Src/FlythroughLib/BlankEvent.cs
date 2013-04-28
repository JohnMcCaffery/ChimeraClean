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
using System.Xml;
using OpenMetaverse;
using Chimera.Flythrough.GUI;

namespace Chimera.Flythrough {
    public class BlankEvent<T> : FlythroughEvent<T> {
        /// <summary>
        /// How many Blank have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// The panel which can be used to control this event.
        /// </summary>
        private BlankPanel<T> mPanel;

        /// <summary>
        /// Initialise the event, specifying how long to do nothing for.
        /// </summary>
        /// <param name="target">The position the camera will end up in.</param>
        public BlankEvent(Flythrough container, int length)
            : base(container, length) {
            Name = "Blank " + (++COUNT);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("BlankEvent");

            XmlAttribute length = doc.CreateAttribute("Length");
            length.Value = Length.ToString();
            node.Attributes.Append(length);

            return node;
        }

        public override void Load(XmlNode node) {
            Length = int.Parse(node.Attributes["Length"].Value);
        }

        public override T this[int time] {
            get { return StartValue; }
        }

        public override System.Windows.Forms.UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new BlankPanel<T>(this);
                return mPanel;
            }
        }

        public override T FinishValue {
            get { return StartValue; }
        }

        public override T Value {
            get { return StartValue; }
        }

        protected override void StartChanged(T value) { }

        protected override void LengthChanged(int length) { }

        protected override void StartTimeChanged(int startTime) { }

        protected override void TimeChanged(int time) { }

        protected override string GetSpecificState() {
            return "";
        }
    }
}
