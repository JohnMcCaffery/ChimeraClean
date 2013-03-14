using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using OpenMetaverse;
using Chimera.Interfaces;

namespace Chimera.Kinect.GUI {
    class ScalarUpdater : IUpdater<float> {
        private Scalar mScalar = Scalar.Create(0f);

        public Scalar Scalar {
            get { return mScalar; }
            set {
                if ((object)mScalar == null)
                    throw new ArgumentException("Unable to set Scalar. Value cannot be null.");
                if ((object)mScalar != null)
                    mScalar.OnChange -= mScalar_OnChange;
                mScalar = value;
                mScalar.OnChange += mScalar_OnChange;
            }
        }

        public float Value {
            get { return mScalar.Value; }
            set { 
                mScalar.Value = value;
                Nui.Poll();
            }
        }

        public event Action<float> Changed;

        private void mScalar_OnChange() {
            if (Changed != null)
                Changed(mScalar.Value);
        }

        public ScalarUpdater() { }

        public ScalarUpdater(Scalar scalar) {
            Scalar = scalar;
        }
    }

    class VectorUpdater : IUpdater<Vector3> {
        private Vector mVector = Vector.Create(0f, 0f, 0f);

        public Vector Vector {
            get { return mVector; }
            set {
                if ((object)mVector == null)
                    throw new ArgumentException("Unable to set Vector. Value cannot be null.");
                if ((object)mVector != null)
                    mVector.OnChange -= mVector_OnChange;
                mVector = value;
                mVector.OnChange += mVector_OnChange;
            }
        }

        public Vector3 Value {
            get { return new Vector3(mVector.X, mVector.Y, mVector.Z); }
            set { 
                mVector.Set(value.X, value.Y, value.Z);
                Nui.Poll();
            }
        }

        public event Action<Vector3> Changed;

        private void mVector_OnChange() {
            if (Changed != null)
                Changed(Value);
        }

        public VectorUpdater() { }

        public VectorUpdater(Vector vector) {
            Vector = vector;
        }
    }

    class ConditionUpdater : IUpdater<bool> {
        private Condition mCondition = Condition.Create(false);

        public Condition Condition {
            get { return mCondition; }
            set {
                if ((object)mCondition == null)
                    throw new ArgumentException("Unable to set Condition. Value cannot be null.");
                if ((object)mCondition != null)
                    mCondition.OnChange -= mCondition_OnChange;
                mCondition = value;
                mCondition.OnChange += mCondition_OnChange;
            }
        }

        public bool Value {
            get { return mCondition.Value; }
            set { 
                mCondition.Value = value;
                Nui.Poll();
            }
        }

        public event Action<bool> Changed;

        private void mCondition_OnChange() {
            if (Changed != null)
                Changed(mCondition.Value);
        }

        public ConditionUpdater() { }

        public ConditionUpdater(Condition condition) {
            Condition = condition;
        }
    }
}