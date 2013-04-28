using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;

namespace Chimera.Plugins {
    public class SplitAxis : IAxis {
        private IAxis mPositive;
        private IAxis mNegative;



        #region IAxis Members

        public System.Windows.Forms.UserControl ControlPanel {
            get { throw new NotImplementedException(); }
        }

        public float Delta {
            get { throw new NotImplementedException(); }
        }

        public AxisBinding Binding {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string Name {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
