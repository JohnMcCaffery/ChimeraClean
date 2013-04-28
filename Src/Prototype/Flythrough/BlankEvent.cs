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

namespace Chimera.Flythrough {
    public class BlankEvent : FlythroughEvent {
        /// <summary>
        /// How many LookAtEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// The name of the event.
        /// </summary>
        private readonly string mName;

        public override string Name {
            get { return mName; }
        }

        public override bool Step() {
            return DoStep();
        }

        protected override void LengthChanged() {
            //Do Nothing
        }

        public override void Load(XmlNode node) {
            Length = int.Parse(node.Attributes["Length"].Value);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("BlankEvent");

            XmlAttribute length = doc.CreateAttribute("Length");
            length.Value = Length.ToString();
            node.Attributes.Append(length);

            return node;
        }

        /// <summary>
        /// Initialise the event, specificying where the camera will end up.
        /// </summary>
        /// <param name="target">The position the camera will end up in.</param>
        public BlankEvent(FlythroughManager container, int length)
            : base(container, length) {
            mName = "Blank " + (++COUNT);
        }
    }
}
