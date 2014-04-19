using ConfigurationTool.Controls;
namespace Chimera.ConfigurationTool {
    partial class ConfigurationTool {
        private const string NAME_DEFAULT = "Configuration Name";

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.FoldersTab = new System.Windows.Forms.TabControl();
            this.loader = new System.ComponentModel.BackgroundWorker();
            this.applicationList = new System.Windows.Forms.ComboBox();
            this.folderList = new System.Windows.Forms.ComboBox();
            this.bindButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.copyList = new System.Windows.Forms.ComboBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FoldersTab
            // 
            this.FoldersTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FoldersTab.Location = new System.Drawing.Point(0, 33);
            this.FoldersTab.Name = "FoldersTab";
            this.FoldersTab.SelectedIndex = 0;
            this.FoldersTab.Size = new System.Drawing.Size(909, 440);
            this.FoldersTab.TabIndex = 0;
            // 
            // applicationList
            // 
            this.applicationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.applicationList.FormattingEnabled = true;
            this.applicationList.Location = new System.Drawing.Point(68, 6);
            this.applicationList.Name = "applicationList";
            this.applicationList.Size = new System.Drawing.Size(121, 21);
            this.applicationList.TabIndex = 1;
            // 
            // folderList
            // 
            this.folderList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.folderList.FormattingEnabled = true;
            this.folderList.Location = new System.Drawing.Point(237, 6);
            this.folderList.Name = "folderList";
            this.folderList.Size = new System.Drawing.Size(121, 21);
            this.folderList.TabIndex = 2;
            // 
            // bindButton
            // 
            this.bindButton.Location = new System.Drawing.Point(364, 4);
            this.bindButton.Name = "bindButton";
            this.bindButton.Size = new System.Drawing.Size(75, 23);
            this.bindButton.TabIndex = 3;
            this.bindButton.Text = "Bind";
            this.bindButton.UseVisualStyleBackColor = true;
            this.bindButton.Click += new System.EventHandler(this.bindButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Application";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Folder";
            // 
            // createButton
            // 
            this.createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createButton.Location = new System.Drawing.Point(834, 4);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 7;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // copyList
            // 
            this.copyList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.copyList.FormattingEnabled = true;
            this.copyList.Location = new System.Drawing.Point(707, 6);
            this.copyList.Name = "copyList";
            this.copyList.Size = new System.Drawing.Size(121, 21);
            this.copyList.TabIndex = 8;
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Location = new System.Drawing.Point(541, 6);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(100, 20);
            this.nameBox.TabIndex = 9;
            this.nameBox.Enter += new System.EventHandler(this.nameBox_Enter);
            this.nameBox.Leave += new System.EventHandler(this.nameBox_Leave);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(500, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(644, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Copy From";
            // 
            // ConfigurationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 473);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.copyList);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bindButton);
            this.Controls.Add(this.folderList);
            this.Controls.Add(this.applicationList);
            this.Controls.Add(this.FoldersTab);
            this.Name = "ConfigurationTool";
            this.Text = "Chimera Configuration Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ConfigurationTool_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl FoldersTab;
        private System.ComponentModel.BackgroundWorker loader;
        private System.Windows.Forms.ComboBox applicationList;
        private System.Windows.Forms.ComboBox folderList;
        private System.Windows.Forms.Button bindButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.ComboBox copyList;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;


    }
}