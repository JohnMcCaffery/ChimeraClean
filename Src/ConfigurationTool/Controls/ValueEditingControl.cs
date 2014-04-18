using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Config;

namespace Chimera.ConfigurationTool.Controls {
    public partial class ValueEditingControl : UserControl, IDataGridViewEditingControl {
        private ConfigParam mParameter;

        public ValueEditingControl() {
            InitializeComponent();
        }

        public ConfigParam Parameter {
            get { return mParameter; }
            set {
                mParameter = value;
                LoadParameter(value);
            }
        }

        private void LoadParameter(ConfigParam value) {
            switch (value.Type) {
                case ParameterTypes.Bool: BoolLoaded();  break;
                case ParameterTypes.Double: DoubleLoaded(); break;
                case ParameterTypes.Float: FloatLoaded(); break;
                case ParameterTypes.Int: IntLoaded(); break;
                case ParameterTypes.String: StringLoaded(); break;
                case ParameterTypes.File: FileLoaded(); break;
                case ParameterTypes.Folder: FolderLoaded(); break;
                case ParameterTypes.Enum: EnumLoaded(); break;
            }
        }

        private void BoolLoaded() {
            boolInput.Visible = true;
            boolInput.CheckedChanged += new EventHandler(boolInput_CheckedChanged);
            boolInput.Checked = bool.Parse(mParameter.Value);
        }

        private void DoubleLoaded() {
            numberInput.Visible = true;
            numberInput.DecimalPlaces = 100;
            numberInput.ValueChanged += new EventHandler(numberInput_ValueChanged);
            numberInput.Value = decimal.Parse(mParameter.Value);
        }

        private void FloatLoaded() {
            numberInput.Visible = true;
            numberInput.DecimalPlaces = 10;
            numberInput.ValueChanged += new EventHandler(numberInput_ValueChanged);
            numberInput.Value = decimal.Parse(mParameter.Value);
        }

        private void IntLoaded() {
            numberInput.Visible = true;
            numberInput.DecimalPlaces = 0;
            numberInput.Value = decimal.Parse(mParameter.Value);
            numberInput.ValueChanged += new EventHandler(numberInput_ValueChanged);
        }

        private void StringLoaded() {
            textInput.Visible = true;
            textInput.DropDownStyle = ComboBoxStyle.DropDown;
            textInput.TextChanged += new EventHandler(textInput_TextChanged);

            textInput.Items.Add(mParameter.Value);
            textInput.Items.AddRange(mParameter.Values);
            textInput.SelectedValue = mParameter.Value;
        }

        private void FileLoaded() {
            textInput.Visible = true;
            textInput.DropDownStyle = ComboBoxStyle.DropDown;
            textInput.TextChanged += new EventHandler(textInput_TextChanged);

            textInput.Items.Add(mParameter.Value);
            textInput.Items.AddRange(mParameter.Values);
            textInput.SelectedValue = mParameter.Value;
        }

        private void FolderLoaded() {
            textInput.Visible = true;
            textInput.DropDownStyle = ComboBoxStyle.DropDown;
            textInput.TextChanged += new EventHandler(textInput_TextChanged);

            textInput.Items.Add(mParameter.Value);
            textInput.Items.AddRange(mParameter.Values);
            textInput.SelectedValue = mParameter.Value;
        }

        private void EnumLoaded() {
            textInput.Visible = true;
            textInput.DropDownStyle = ComboBoxStyle.DropDownList;
            textInput.TextChanged += new EventHandler(textInput_TextChanged);

            textInput.Items.AddRange(mParameter.Values);
            textInput.SelectedValue = mParameter.Value;
        }

        void textInput_TextChanged(object sender, EventArgs e) {
            mParameter.Value = textInput.Text;
        }

        void numberInput_ValueChanged(object sender, EventArgs e) {
            mParameter.Value = numberInput.Value.ToString();
        }

        void boolInput_CheckedChanged(object sender, EventArgs e) {
            mParameter.Value = boolInput.Checked.ToString();
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) {
            throw new NotImplementedException();
        }

        public DataGridView EditingControlDataGridView {
            get { return null; }
            set {
            }
        }

        public object EditingControlFormattedValue {
            get { return null; }
            set {
            }
        }

        public int EditingControlRowIndex {
            get {
                throw new NotImplementedException();
            }
            set {
            }
        }

        public bool EditingControlValueChanged {
            get { return false; }
            set {
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) {
            return false;
        }

        public Cursor EditingPanelCursor {
            get { throw new NotImplementedException(); }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) {
            return null;
        }

        public void PrepareEditingControlForEdit(bool selectAll) {
        }

        public bool RepositionEditingControlOnValueChange {
            get { return false; }
        }
    }
}
