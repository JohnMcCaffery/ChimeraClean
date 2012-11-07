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
using OpenMetaverse;

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
                    RefreshDrawings();
                }));
            };
            master.OnSlaveConnected += AddSlaveTab;

            foreach (var slave in master.Slaves)
                AddSlaveTab(slave);
        }

        private void RefreshDrawings() {
            hTab.Refresh();
            vTab.Refresh();
            hvSplit.Panel1.Refresh();
            hvSplit.Panel2.Refresh();
        }

        private void AddSlaveTab(Master.Slave slave) {
            Action add = new Action(() => {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
                TabPage slaveTab = new System.Windows.Forms.TabPage();
                VectorPanel positionOffset = new ProxyTestGUI.VectorPanel();
                RotationPanel rotationOffset = new ProxyTestGUI.RotationPanel();
                this.slavesTab.SuspendLayout();
                slaveTab.SuspendLayout();
                this.SuspendLayout();

                string name = slave.Name;
                slaveTabs.Add(name, slaveTab);

                // 
                // mainTab
                // 
                slavesTab.Controls.Add(slaveTab);
                // 
                // slaveTab
                // 
                slaveTab.AutoScroll = true;
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
                // positionOffset
                // 
                positionOffset.DisplayName = name + " Position";
                positionOffset.Location = new System.Drawing.Point(0, 153);
                positionOffset.Max = maxOffset;
                positionOffset.Min = -maxOffset;
                positionOffset.Name = name + "Position";
                positionOffset.Size = new System.Drawing.Size(609, 98);
                positionOffset.TabIndex = 1;
                positionOffset.X = 0F;
                positionOffset.Y = 0F;
                positionOffset.Z = 0F;
                positionOffset.OnChange += new System.EventHandler(this.rawPosition_OnChange);
                // 
                // rotationOffset
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

                positionOffset.OnChange += (p, ep) => {
                    slave.PositionOffset = positionOffset.Value;
                    RefreshDrawings();
                };
                rotationOffset.OnChange += (p, ep) => {
                    slave.RotationOffset.Pitch = rotationOffset.Pitch;
                    slave.RotationOffset.Yaw = rotationOffset.Yaw;
                    hTab.Refresh();
                    vTab.Refresh();
                    hvSplit.Panel1.Refresh();
                    hvSplit.Panel2.Refresh();
                };
            });
            if (InvokeRequired)
                Invoke(add);
            else
                add();
        }

        private void MasterForm_FormClosing(object sender, FormClosingEventArgs e) {
            master.Stop();
        }

        private void rawRotation_OnChange(object sender, EventArgs e) {
            //master.Rotation.Quaternion = rawRotation.Rotation;
            master.Rotation.Pitch = rawRotation.Pitch;
            master.Rotation.Yaw = rawRotation.Yaw;
            RefreshDrawings();
        }

        private void rawPosition_OnChange(object sender, EventArgs e) {
            master.Position = rawPosition.Value;
            RefreshDrawings();
        }

        private int r = 5;
        private float maxOffset = 32f;

        private void hTab_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            int w = e.ClipRectangle.Width;
            int h = e.ClipRectangle.Height;
            //float scale = Math.Min(w, h) / 2f;
            float scale = Math.Min(w, h);
            Point origin = new Point(w / 2, h / 2);
            g.DrawLine(Pens.LightGreen, 0f, h / 2f, w, h / 2f);
            g.DrawLine(Pens.Red, w / 2f, 0f, w / 2f, h);
            g.FillEllipse(Brushes.Black, new Rectangle(origin.X - r, origin.Y - r, r * 2, r * 2));

            Vector3 masterLookAt = master.Rotation.LookAtVector;
            //Vector2 hVector = Vector2.Normalize(new Vector2(masterLookAt.X, masterLookAt.Y));
            Point masterLookAtEnd = new Point(origin.X + (int) (masterLookAt.Y * scale), origin.Y + (int) (-masterLookAt.X * scale));
            g.DrawLine(Pens.Black, origin, masterLookAtEnd);

            float offsetMultiplierH = (scale / 2) / maxOffset;
            float offsetMultiplierV = (scale / 2) / maxOffset;

            foreach (var slave in master.Slaves) {
                Vector3 slaveOffset = slave.PositionOffset * master.Rotation.Quaternion;
                Point slaveOrigin = new Point(origin.X + (int) (slaveOffset.Y * offsetMultiplierH), origin.Y + (int) (-slaveOffset.X * offsetMultiplierV));

                //Vector3 lookAt = master.Rotation.LookAtVector * slave.RotationOffset;
                Rotation rot = new Rotation(master.Rotation.Pitch + slave.RotationOffset.Pitch, master.Rotation.Yaw + slave.RotationOffset.Yaw);
                Point slaveLookAtEnd = new Point(slaveOrigin.X + (int)(rot.LookAtVector.Y * scale), slaveOrigin.Y + (int)(-rot.LookAtVector.X * scale));
                g.DrawLine(Pens.Black, slaveOrigin, slaveLookAtEnd);
            }
        }

        private void vTab_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            int w = e.ClipRectangle.Width;
            int h = e.ClipRectangle.Height;
            //float scale = Math.Min(w, h) / 2f;
            float scale = Math.Min(w, h);
            Point origin = new Point(r, h / 2);
            g.DrawLine(Pens.LightGreen, 0f, h / 2f, w, h / 2f);
            g.DrawLine(Pens.Blue, r, 0f, r, h);
            g.FillEllipse(Brushes.Black, 0, origin.Y - r, r * 2, r * 2);

            Vector3 masterLookAt = master.Rotation.LookAtVector;
            //Vector3 masterLookAt = Vector3.Normalize(master.Rotation.LookAtVector);
            Vector2 hVector = new Vector2(masterLookAt.Y, masterLookAt.X);
            Vector3 cross = Vector3.Cross(new Vector3(hVector, 0f), Vector3.UnitX);
            int x = (int)(hVector.Length() * scale * (cross.Z < 0 ? 1 : -1));
            Point masterLookAtEnd = new Point(origin.X + x, origin.Y + (int) (masterLookAt.Z * scale));
            g.DrawLine(Pens.Black, origin, masterLookAtEnd);

            float offsetMultiplierH = (scale / 2) / maxOffset;
            float offsetMultiplierV = (scale / 2) / maxOffset;

            foreach (var slave in master.Slaves) {
                Vector3 slaveOffset = slave.PositionOffset * master.Rotation.Quaternion;
                Point slaveOrigin = new Point(origin.X + (int) (slaveOffset.X * offsetMultiplierH), origin.Y + (int) (-slaveOffset.Z * offsetMultiplierV));

                //Vector3 lookAt = master.Rotation.LookAtVector * slave.RotationOffset;
                Rotation rot = new Rotation(master.Rotation.Pitch + slave.RotationOffset.Pitch, master.Rotation.Yaw + slave.RotationOffset.Yaw);
                Vector2 slaveHVector = new Vector2(rot.LookAtVector.Y, rot.LookAtVector.X);
                Vector3 slaveCross = Vector3.Cross(new Vector3(slaveHVector, 0f), Vector3.UnitX);
                int slaveX = (int)(slaveHVector.Length() * scale * (slaveCross.Z < 0 ? 1 : -1));
                Point slaveLookAtEnd = new Point(slaveOrigin.X + slaveX, slaveOrigin.Y + (int)(rot.LookAtVector.Z * scale));
                g.DrawLine(Pens.Black, slaveOrigin, slaveLookAtEnd);
            }
        }
    }
}
