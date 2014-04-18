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
        private DataGridView mView;
        private int mRowIndex;

        private bool mFirstLoad = true;

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
                case ParameterTypes.Vector3: Vector3Loaded(); break;
            }
        }

        private void Vector3Loaded() {
            numberInput.Visible = true;
        }

        private void BoolLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                boolInput.Visible = true;
                boolInput.CheckedChanged += new EventHandler(boolInput_CheckedChanged);
            }
            boolInput.Checked = bool.Parse(mParameter.Value);
        }

        private void DoubleLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                numberInput.Visible = true;
                numberInput.DecimalPlaces = 100;
                numberInput.ValueChanged += new EventHandler(numberInput_ValueChanged);
            }
            numberInput.Value = decimal.Parse(mParameter.Value);
        }

        private void FloatLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                numberInput.Visible = true;
                numberInput.DecimalPlaces = 10;
                numberInput.ValueChanged += new EventHandler(numberInput_ValueChanged);
            }
            numberInput.Value = decimal.Parse(mParameter.Value);
        }

        private void IntLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                numberInput.Visible = true;
                numberInput.DecimalPlaces = 0;
                numberInput.ValueChanged += new EventHandler(numberInput_ValueChanged);
            }
            numberInput.Value = decimal.Parse(mParameter.Value);
        }

        private void StringLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                textInput.Visible = true;
                textInput.DropDownStyle = ComboBoxStyle.Simple;
                textInput.TextChanged += new EventHandler(textInput_TextChanged);

                textInput.Items.Add(mParameter.Value);
                textInput.Items.AddRange(mParameter.Values);
            }
            textInput.Text = mParameter.Value;
        }

        private void FileLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                textInput.Visible = true;
                textInput.DropDownStyle = ComboBoxStyle.Simple;
                textInput.TextChanged += new EventHandler(textInput_TextChanged);

                textInput.Items.Add(mParameter.Value);
                textInput.Items.AddRange(mParameter.Values);
            }
            textInput.Text = mParameter.Value;
        }

        private void FolderLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                textInput.Visible = true;
                textInput.DropDownStyle = ComboBoxStyle.Simple;
                textInput.TextChanged += new EventHandler(textInput_TextChanged);

                textInput.Items.Add(mParameter.Value);
                textInput.Items.AddRange(mParameter.Values);
            }
            textInput.Text = mParameter.Value;
        }

        private void EnumLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
                textInput.Visible = true;
                textInput.DropDownStyle = ComboBoxStyle.DropDownList;
                textInput.TextChanged += new EventHandler(textInput_TextChanged);

                textInput.Items.AddRange(mParameter.Values);
            }
            textInput.SelectedItem = mParameter.Values.First(p => p == mParameter.Value);
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

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) { }

        public DataGridView EditingControlDataGridView {
            get { return mView; }
            set { mView = value; }
        }

        public object EditingControlFormattedValue {
            get { return mParameter.Value; }
            set { }
        }

        public int EditingControlRowIndex {
            get { return mRowIndex; }
            set { mRowIndex = value; }
        }

        public bool EditingControlValueChanged {
            get;
            set;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) {
            return keyData != Keys.Escape;
        }

        public Cursor EditingPanelCursor {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll) {
        }

        public bool RepositionEditingControlOnValueChange {
            get { return true; }
        }
    }
}
