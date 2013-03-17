using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using OpenMetaverse;
using Chimera.Interfaces;

namespace Chimera.Kinect.GUI {
    public class ScalarUpdater : IUpdater<float> {
        private Scalar mScalar;

        public Scalar Scalar {
            get { return mScalar; }
            set {
                if ((object)value == null)
                    throw new ArgumentException("Unable to set Scalar. Value cannot be null.");
                if ((object)mScalar != null)
                    mScalar.OnChange -= mScalar_OnChange;
                mScalar = value;
                mScalar.OnChange += mScalar_OnChange;
            }
        }

        public string Name {
            get { return (object)mScalar == null ? "Not Set" : mScalar.Name; }
        }

        public float Value {
            get { return mScalar.Value; }
            set { 
                mScalar.Value = value;
                //Nui.Poll();
            }
        }

        public event Action<float> Changed;

        private void mScalar_OnChange() {
            if (Changed != null)
                Changed(mScalar.Value);
        }

        public ScalarUpdater(Scalar scalar) {
            Scalar = scalar;
        }
    }

    public class VectorUpdater : IUpdater<Vector3> {
        private Vector mVector;

        public Vector Vector {
            get { return mVector; }
            set {
                if ((object)value == null)
                    throw new ArgumentException("Unable to set Vector. Value cannot be null.");
                if ((object)mVector != null)
                    mVector.OnChange -= mVector_OnChange;
                mVector = value;
                mVector.OnChange += mVector_OnChange;
            }
        }

        public string Name {
            get { return (object)mVector == null ? "Not Set" : mVector.Name; }
        }

        public Vector3 Value {
            get { return new Vector3(mVector.X, mVector.Y, mVector.Z); }
            set { 
                mVector.Set(value.X, value.Y, value.Z);
                //Nui.Poll();
            }
        }

        public event Action<Vector3> Changed;

        private void mVector_OnChange() {
            if (Changed != null)
                Changed(Value);
        }

        public VectorUpdater(Vector vector) {
            Vector = vector;
        }
    }

    public class ConditionUpdater : IUpdater<bool> {
        private Condition mCondition;

        public Condition Condition {
            get { return mCondition; }
            set {
                if ((object)value == null)
                    throw new ArgumentException("Unable to set Condition. Value cannot be null.");
                if ((object)mCondition != null)
                    mCondition.OnChange -= mCondition_OnChange;
                mCondition = value;
                mCondition.OnChange += mCondition_OnChange;
            }
        }

        public string Name {
            get { return (object)mCondition == null ? "Not Set" : mCondition.Name; }
        }

        public bool Value {
            get { return mCondition.Value; }
            set { 
                mCondition.Value = value;
                //Nui.Poll();
            }
        }

        public event Action<bool> Changed;

        private void mCondition_OnChange() {
            if (Changed != null)
                Changed(mCondition.Value);
        }

        public ConditionUpdater(Condition condition) {
            Condition = condition;
        }
    }
}