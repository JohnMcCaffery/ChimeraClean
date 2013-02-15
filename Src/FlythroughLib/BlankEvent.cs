using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FlythroughLib {
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
