using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;
using OpenMetaverse;

namespace FlythroughLib.Panels {
    public partial class FlythroughPanel : UserControl {
        private FlythroughManager mContainer = new FlythroughManager();
        private CameraMaster mMaster;
        private MoveToEvent mMoveToEvent;
        private MoveToEvent mMoveToEvent2;
        private RotateToEvent mRotateToEvent;
        private LookAtLockEvent mLookAtLockEvent;
        private ComboEvent mComboEvent;

        public FlythroughPanel() {
            InitializeComponent();

            mMoveToEvent = new MoveToEvent(mContainer, (int) lengthValue.Value, moveToTargetVectorPanel.Value);
            moveToTargetVectorPanel.OnChange += (source, args) => mMoveToEvent.Target = moveToTargetVectorPanel.Value;
            lengthValue.ValueChanged += (source, args) => mMoveToEvent.Length = (int)lengthValue.Value;

            mMoveToEvent2 = new MoveToEvent(mContainer, (int) lengthValue.Value, moveToTargetVectorPanel.Value);
            moveToTargetVectorPanel.OnChange += (source, args) => mMoveToEvent2.Target = moveToTargetVectorPanel.Value + new Vector3(0, 50f, 0);

            mRotateToEvent = new RotateToEvent(mContainer, (int)lengthValue.Value);
            pitchValue.ValueChanged += (source, args) => mRotateToEvent.PitchTarget = (float)pitchValue.Value;
            yawValue.ValueChanged += (source, args) => mRotateToEvent.YawTarget = (float)yawValue.Value;

            mLookAtLockEvent = new LookAtLockEvent(mContainer, (int)lengthValue.Value);
            mLookAtLockEvent.Target = lockLookAtVectorPanel.Value;
            lockLookAtVectorPanel.OnChange += (source, args) => mLookAtLockEvent.Target = lockLookAtVectorPanel.Value;

            mComboEvent = new ComboEvent(mContainer);
            mComboEvent.AddStream1Event(mMoveToEvent);
            //mComboEvent.AddStream1Event(mMoveToEvent2);
            mComboEvent.AddStream2Event(mLookAtLockEvent);

            mContainer.OnPositionChange += (source, args) => mMaster.Position = mContainer.Position;
            mContainer.OnRotationChange += (source, args) => {
                mMaster.Rotation.Pitch = mContainer.Rotation.Pitch;
                mMaster.Rotation.Yaw = mContainer.Rotation.Yaw;
            };

            mContainer.AddEvent(mComboEvent);
            //mContainer.AddEvent(mRotateToEvent);
            //mContainer.AddEvent(mMoveToEvent);
        }

        public CameraMaster Master {
            get { return mMaster; }
            set { mMaster = value; }
        }

        private void playButton_Click(object sender, EventArgs e) {
            mContainer.Play(mMaster.Position, mMaster.Rotation);
        }

        private void button1_Click(object sender, EventArgs e) {
            mMoveToEvent.Target = mMaster.Position;
            moveToTargetVectorPanel.Value = mMoveToEvent.Target;
        }

        private void rotateToTakeCurrentButton_Click(object sender, EventArgs e) {
            mRotateToEvent.PitchTarget = mMaster.Rotation.Pitch;
            mRotateToEvent.YawTarget = mMaster.Rotation.Yaw;
            pitchValue.Value = new decimal(mRotateToEvent.PitchTarget);
            pitchValue.Value = new decimal(mRotateToEvent.YawTarget);
        }

        private void lookAtTakeCurrentButton_Click(object sender, EventArgs e) {
            mLookAtLockEvent.Target = mMaster.Position;
            lockLookAtVectorPanel.Value = mLookAtLockEvent.Target;
        }
    }
}
