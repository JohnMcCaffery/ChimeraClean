using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Config;

namespace Chimera.ConfigurationTool.Controls {
    public class ValueColumn : DataGridViewColumn {
        public override DataGridViewCell CellTemplate {
            get {
                return base.CellTemplate;
            }
            set {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(ValueCell))) {
                    throw new InvalidCastException("Must be a ValueCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class ValueCell : DataGridViewCell {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle) {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            ValueEditingControl ctrl = new ValueEditingControl();

            if (Value != null)
                ctrl.Parameter = (ConfigParam) Value;

        }

        public override Type EditType {
            get {
                return typeof(ValueEditingControl);
            }
        }

        public override Type ValueType {
            get {
                return typeof (ConfigParam);
            }
        }

        public override object DefaultNewRowValue {
            get {
                return new ConfigParam("Default", "Default", ParameterTypes.String, "Default", "Default", "Default", true, "Default", "Default");
            }
        }
    }
}
