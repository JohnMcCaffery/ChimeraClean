using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using OpenMetaverse;
using Chimera.FlythroughLib.GUI;

namespace Chimera.FlythroughLib {
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
            get { return Start; }
        }

        public override System.Windows.Forms.UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new BlankPanel<T>(this);
                return mPanel;
            }
        }

        public override T Finish {
            get { return Start; }
        }

        public override T Value {
            get { return Start; }
        }

        protected override void LengthChanged(int length) { }

        protected override void TimeChanged(int time) { }

        protected override void StartChanged(T value) { }
    }
}
