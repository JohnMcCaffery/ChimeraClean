using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.GUI.Controls.Inputs;
using Chimera.Util;
using OpenMetaverse;
using System.Drawing;

namespace Chimera.Inputs {
    public class SelectableInput : ISystemInput {
        private readonly List<ISystemInput> mInputs = new List<ISystemInput>();
        private readonly string mName;

        private ISystemInput mCurrentInput;
        private SelectableInputPanel mControlPanel;
        private Coordinator mCoordinator;
        private bool mEnabled;

        public event Action<ISystemInput> InputAdded;

        public ISystemInput CurrentInput {
            get { return mCurrentInput; }
            set {
                if (mCurrentInput != null)
                    mCurrentInput.Enabled = false;
                mCurrentInput = value;
                mCurrentInput.Enabled = mEnabled;
            }
        }

        public IEnumerable<ISystemInput> Inputs {
            get { return mInputs; }
        }

        public SelectableInput(string name, params ISystemInput[] inputs) {
            mName = name;

            foreach (var input in inputs)
                AddInput(input);
        }

        public void AddInput(ISystemInput input) {
            if (mCurrentInput == null) {
                mCurrentInput = input;
                mCurrentInput.Enabled = mEnabled;
            }
            mInputs.Add(input);
            if (InputAdded != null)
                InputAdded(input);
        }

        #region ISystemInput Members

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            foreach (var input in mInputs)
                input.Init(coordinator);
        }

        #endregion

        #region IInput Members

        public event Action<IInput, bool> EnabledChanged;

        public System.Windows.Forms.UserControl ControlPanel {
            get {
                if (mControlPanel == null)
                    mControlPanel = new SelectableInputPanel(this);
                return mControlPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (mCurrentInput != null)
                        mCurrentInput.Enabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return mName; }
        }

        public string State {
            get {
                string ret = mName + " -- combined input. Current Input: " + mCurrentInput.Name + Environment.NewLine;
                ret += mCurrentInput.State;
                ret += "---------------------------" + Environment.NewLine;
                foreach (var input in mInputs) {
                    if (input != mCurrentInput) {
                        ret += input.State;
                        ret += "---------------------------" + Environment.NewLine;
                    }
                }
                return ret;
            }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
            foreach (var input in mInputs)
                input.Close();
        }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) {
            mCurrentInput.Draw(to2D, graphics, redraw);
        }

        #endregion
    }
}
