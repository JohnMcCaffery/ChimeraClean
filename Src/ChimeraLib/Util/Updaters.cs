using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;

namespace Chimera.Util {
    public class Updater<T> : IUpdater<T> {
        private readonly string mName;
        private T mVal;

        public Updater(string name) {
            mName = name;
        }

        public Updater(string name, T val)
            : this(name) {
            mVal = val;
        }


        #region IUpdater<T> Members

        public T Value {
            get { return mVal; }
            set {
                if (mVal == null || !mVal.Equals(value)) {
                    mVal = value;
                    if (Changed != null)
                        Changed(mVal);
                    if (ManuallyChanged != null)
                        ManuallyChanged(mVal);
                }
            }
        }

        public event Action<T> Changed;

        public string Name {
            get { return mName; }
        }

        #endregion


        public event Action<T> ManuallyChanged;
    }
}
