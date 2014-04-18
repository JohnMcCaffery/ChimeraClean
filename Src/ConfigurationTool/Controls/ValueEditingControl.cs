using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Config;
using System.IO;
using OpenMetaverse;

namespace Chimera.ConfigurationTool.Controls {
    public partial class ValueEditingControl : UserControl, IDataGridViewEditingControl {
        private ConfigParam mParameter;
        private DataGridView mView;
        private int mRowIndex;
        private string mValue;

        private bool mFirstLoad = true;

        public ValueEditingControl() {
            InitializeComponent();
            HideAll();
        }

        private void HideAll() {
            boolInput.Visible = false;
            textInput.Visible = false;
        }

        public ConfigParam Parameter {
            get { return mParameter; }
            set {
                mParameter = value;
                mValue = value.Value;
                LoadParameter(value);
            }
        }

        private void LoadParameter(ConfigParam value) {
            switch (value.Type) {
                case ParameterTypes.Bool: BoolLoaded();  break;
                case ParameterTypes.Folder: 
                case ParameterTypes.File: FileLoaded(); break;
                case ParameterTypes.Enum: EnumLoaded(); break;
                case ParameterTypes.Vector3: 
                case ParameterTypes.Double: 
                case ParameterTypes.Float: 
                case ParameterTypes.Int: 
                case ParameterTypes.String: StringLoaded(); break;
            }
        }

        private void BoolLoaded() {
            if (mFirstLoad) {
                mFirstLoad = false;
            }
            textInput.Visible = false;
            boolInput.Visible = true;
            dialogButton.Visible = false;

            boolInput.Checked = bool.Parse(mParameter.Value);
        }

        private void StringLoaded() {
            textInput.DropDownStyle = ComboBoxStyle.Simple;
            textInput.Items.Add(mParameter.Value);
            textInput.Items.AddRange(mParameter.Values);

            boolInput.Visible = false;
            dialogButton.Visible = false;
            textInput.Visible = true;

            textInput.Text = mParameter.Value;
        }

        private void FileLoaded() {
            StringLoaded();

            if (mParameter.Type == ParameterTypes.File) {
                openFileDialog.FileName = Path.GetFileName(mParameter.Value);
                openFileDialog.InitialDirectory = Path.GetDirectoryName(ToAbsolute(mParameter.Value));
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    textInput.Text = ToRelative(openFileDialog.FileName);
            }

            if (mParameter.Type == ParameterTypes.Folder) {
                folderBrowserDialog.SelectedPath = ToAbsolute(mParameter.Value);
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                    textInput.Text = ToRelative(folderBrowserDialog.SelectedPath);
                }
            }

            //textInput.Width = Width - (dialogButton.Width + 3);
            //dialogButton.Visible = true;
        }

        private string ToAbsolute(string uri) {
            string rootFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
            return Path.GetFullPath(Path.Combine(rootFolder, uri));
        }

        private string ToRelative(string uri) {
            Uri x = new Uri(Environment.CurrentDirectory);
            Uri xy = x.MakeRelativeUri(new Uri(uri));
            return xy.OriginalString;
        }

        private void EnumLoaded() {
            StringLoaded();

            textInput.DropDownStyle = ComboBoxStyle.DropDownList;
            textInput.Items.AddRange(mParameter.Values);
            textInput.SelectedItem = mParameter.Values.First(p => p == mParameter.Value);
        }

        void textInput_TextChanged(object sender, EventArgs e) {
            mParameter.Value = textInput.Text;
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
            return keyData != Keys.Escape && keyData != Keys.Enter;
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

        private void textInput_Validating(object sender, CancelEventArgs e) {
            double d;
            float f;
            int i;
            if (
                (mParameter.Type == ParameterTypes.Double && !double.TryParse(textInput.Text, out d)) ||
                (mParameter.Type == ParameterTypes.Float && !float.TryParse(textInput.Text, out f)) ||
                (mParameter.Type == ParameterTypes.Int && !int.TryParse(textInput.Text, out i))
                )
                e.Cancel = true;

            Vector3 v;
            if (mParameter.Type == ParameterTypes.Vector3 && !Vector3.TryParse(textInput.Text, out v))
                e.Cancel = true;

            if (mParameter.Type == ParameterTypes.File && !File.Exists(ToAbsolute(textInput.Text)))
                e.Cancel = true;

            if (mParameter.Type == ParameterTypes.Folder && !Directory.Exists(ToAbsolute(textInput.Text)))
                e.Cancel = true;

            if (e.Cancel)
                mParameter.Value = mValue;
            else
                mValue = mParameter.Value;
        }

        private void dialogButton_Click(object sender, EventArgs e) {

            if (mParameter.Type == ParameterTypes.File) {
                openFileDialog.FileName = mParameter.Value;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    textInput.Text = openFileDialog.FileName;
            }

            if (mParameter.Type == ParameterTypes.Folder) {
                folderBrowserDialog.SelectedPath = mParameter.Value;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    textInput.Text = folderBrowserDialog.SelectedPath;
            }
        }

    }
}
