using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;
using ProxyTestGUI;

namespace ConsoleTest {
    public partial class MasterForm : Form {
        private readonly CameraMaster master;
        private readonly Dictionary<string, TabPage> slaveTabs = new Dictionary<string, TabPage>();

        public MasterForm() : this (new CameraMaster()) { }
        public MasterForm(CameraMaster master) {
            InitializeComponent();

            this.master = master;
            rawPosition.Value = master.Position;
            rawRotation.LookAtVector = master.Rotation.LookAtVector;
            master.OnCameraUpdated += (source, args) => {
                Invoke(new Action(() => {
                    rawPosition.Value = master.Position;
                    rawRotation.LookAtVector = master.Rotation.LookAtVector;
                }));
            };
            master.OnSlaveConnected += AddSlaveTab;
        }

        private void AddSlaveTab(Master.Slave slave) {
            Invoke(new Action(() => {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
                TabPage slaveTab = new System.Windows.Forms.TabPage();
                VectorPanel positionOffset = new ProxyTestGUI.VectorPanel();
                RotationPanel rotationOffset = new ProxyTestGUI.RotationPanel();
                this.mainTab.SuspendLayout();
                slaveTab.SuspendLayout();
                this.SuspendLayout();

                string name = slave.Name;
                slaveTabs.Add(name, slaveTab);

                // 
                // mainTab
                // 
                mainTab.Controls.Add(slaveTab);
                // 
                // rawTab
                // 
                slaveTab.Controls.Add(positionOffset);
                slaveTab.Controls.Add(rotationOffset);
                slaveTab.Location = new System.Drawing.Point(4, 22);
                slaveTab.Name = name + "Tab";
                slaveTab.Padding = new System.Windows.Forms.Padding(3);
                slaveTab.Size = new System.Drawing.Size(609, 265);
                slaveTab.TabIndex = 0;
                slaveTab.Text = name;
                slaveTab.UseVisualStyleBackColor = true;
                // 
                // rawPosition
                // 
                positionOffset.DisplayName = name + " Position";
                positionOffset.Location = new System.Drawing.Point(0, 153);
                positionOffset.Max = 32D;
                positionOffset.Min = -32D;
                positionOffset.Name = name + "Position";
                positionOffset.Size = new System.Drawing.Size(609, 98);
                positionOffset.TabIndex = 1;
                positionOffset.X = 0F;
                positionOffset.Y = 0F;
                positionOffset.Z = 0F;
                positionOffset.OnChange += new System.EventHandler(this.rawPosition_OnChange);
                // 
                // rawRotation
                // 
                rotationOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                rotationOffset.DisplayName = name + " Rotation";
                rotationOffset.Location = new System.Drawing.Point(0, 0);
                rotationOffset.Name = name + "Rotation";
                rotationOffset.Pitch = 0F;
                rotationOffset.Size = new System.Drawing.Size(613, 147);
                rotationOffset.TabIndex = 0;
                rotationOffset.Yaw = 0F;
                rotationOffset.OnChange += new System.EventHandler(this.rawRotation_OnChange);

                positionOffset.OnChange += (p, ep) => slave.PositionOffset = positionOffset.Value;
                rotationOffset.OnChange += (p, ep) => slave.RotationOffset = rotationOffset.Rotation;
            }));
        }

        private void MasterForm_FormClosing(object sender, FormClosingEventArgs e) {
            master.Stop();
        }

        private void rawRotation_OnChange(object sender, EventArgs e) {
            master.Rotation.Quaternion = rawRotation.Rotation;
        }

        private void rawPosition_OnChange(object sender, EventArgs e) {
            master.Position = rawPosition.Value;
        }
    }
}
