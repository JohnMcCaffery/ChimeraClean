using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;
using Chimera.GUI.Controls.Plugins;

namespace Chimera.Plugins {
    public class SplitAxis : IAxis {
        private readonly List<IAxis> mAxes = new List<IAxis>();
        private readonly string mName;

        private SplitAxisPanel mPanel;
        private AxisBinding mBinding;
        private IAxis mPositive;
        private IAxis mNegative;

        public event Action<IAxis> AxisAdded;

        public IEnumerable<IAxis> Axes { 
            get { return mAxes; } 
        }

        public IAxis Positive {
            get { return mPositive; }
            set { mPositive = value; }
        }

        public IAxis Negative {
            get { return mNegative; }
            set { mNegative = value; }
        }

        public void AddAxis(IAxis axis) {
            if (mPositive == null)
                mPositive = axis;
            else if (mNegative == null)
                mNegative = axis;

            mAxes.Add(axis);

            if (AxisAdded != null)
                AxisAdded(axis);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="axes">First access will be positive, second negative.</param>
        public SplitAxis(string name, params IAxis[] axes) {
            mName = name;

            foreach (var axis in axes)
                AddAxis(axis);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="binding"></param>
        /// <param name="axes">First access will be positive, second negative.</param>
        public SplitAxis(string name, AxisBinding binding, params IAxis[] axes)
            : this(name, axes) {
            mBinding = binding;
        }


        #region IAxis Members

        public System.Windows.Forms.UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new SplitAxisPanel(this);
                return mPanel;
            }
        }

        public float Delta {
            get {
                if (mPositive == null)
                    return 0f;
                if (mNegative == null)
                    return Math.Abs(mPositive.Delta);

                return Math.Abs(mPositive.Delta) + (Math.Abs(mNegative.Delta) * -1f);
            }
        }

        public AxisBinding Binding {
            get { return mBinding; }
            set { mBinding = value; }
        }

        public string Name {
            get { return mName; }
        }

        #endregion
    }
}
