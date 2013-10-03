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
                if (ManuallyChanged != null)
                    ManuallyChanged(value);
            }
        }

        public event Action<float> Changed;

        public event Action<float> ManuallyChanged;

        private void mScalar_OnChange() {
            if (Changed != null)
                Changed(mScalar.Value);
        }

        public ScalarUpdater(Scalar scalar) {
            Scalar = scalar;
        }

        public override string ToString() {
            return Name + ":" + Value;
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
                if (ManuallyChanged != null)
                    ManuallyChanged(value);
            }
        }

        public event Action<Vector3> Changed;

        public event Action<Vector3> ManuallyChanged;

        private void mVector_OnChange() {
            if (Changed != null)
                Changed(Value);
        }

        public VectorUpdater(Vector vector) {
            Vector = vector;
        }

        public override string ToString() {
            return Name + ":" + Value;
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
                if (ManuallyChanged != null)
                    ManuallyChanged(value);
            }
        }

        public event Action<bool> Changed;

        public event Action<bool> ManuallyChanged;

        private void mCondition_OnChange() {
            if (Changed != null)
                Changed(mCondition.Value);
        }

        public ConditionUpdater(Condition condition) {
            Condition = condition;
        }

        public override string ToString() {
            return Name + ":" + Value;
        }
    }
}
