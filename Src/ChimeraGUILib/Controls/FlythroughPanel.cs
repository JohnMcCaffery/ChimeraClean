using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;

namespace FlythroughLib.Panels {
    public partial class FlythroughPanel : UserControl {
        private FlythroughManager mContainer = new FlythroughManager();
        private Master mMaster;
        private MoveToEvent mMoveToEvent;

        public FlythroughPanel() {
            InitializeComponent();

            mMoveToEvent = new MoveToEvent(mContainer, (int) lengthValue.Value, targetVectorPanel.Value);
            mContainer.AddEvent(mMoveToEvent);
            targetVectorPanel.OnChange += (source, args) => mMoveToEvent.Target = targetVectorPanel.Value;
        }

        public Master Master {
            get { return mMaster; }
            set { mMaster = value; }
        }
    }
}
