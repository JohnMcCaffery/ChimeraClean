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
        private CameraMaster mMaster;
        private MoveToEvent mMoveToEvent;

        public FlythroughPanel() {
            InitializeComponent();

            mMoveToEvent = new MoveToEvent(mContainer, (int) lengthValue.Value, targetVectorPanel.Value);
            mContainer.AddEvent(mMoveToEvent);
            targetVectorPanel.OnChange += (source, args) => mMoveToEvent.Target = targetVectorPanel.Value;
            lengthValue.ValueChanged += (source, args) => mMoveToEvent.Length = (int)lengthValue.Value;

            mContainer.OnPositionChange += (source, args) => mMaster.Position = mContainer.Position;
            mContainer.OnRotationChange += (source, args) => {
                mMaster.Rotation.Pitch = mContainer.Rotation.Pitch;
                mMaster.Rotation.Yaw = mContainer.Rotation.Yaw;
            };
        }

        public CameraMaster Master {
            get { return mMaster; }
            set { mMaster = value; }
        }

        private void playButton_Click(object sender, EventArgs e) {
            mContainer.Play(mMaster.Position, mMaster.Rotation);
        }
    }
}
