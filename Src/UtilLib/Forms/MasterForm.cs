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

            proxyPanel.Proxy = master;

            this.master = master;

            rawPosition.Value = master.Position;
            rawRotation.LookAtVector = master.Rotation.LookAtVector;

            if (master.MasterRunning) {
                statusLabel.Text = "Bound to " + master.ProxyConfig.MasterAddress + ":" + master.ProxyConfig.MasterPort;
                bindButton.Text = "Unbind";
                portBox.Enabled = false;
            }

            master.OnProxyStarted += (source, args) => {
                Invoke(new Action(() => Text = "Master: " + master.ProxyConfig.ProxyPort));
            };
            master.OnCameraUpdated += (source, args) => {
                Invoke(new Action(() => {
                    rawPosition.Value = master.Position;
                    rawRotation.LookAtVector = master.Rotation.LookAtVector;
                    RefreshDrawings();
                    receivedLabel.Text = master.PacketsReceived.ToString();
                    processedLabel.Text = master.PacketsProcessed.ToString();
                    generatedLabel.Text = master.PacketsGenerated.ToString();
                    forwardedLabel.Text = master.PacketsForwarded.ToString();
                }));
            };
            master.OnSlaveConnected += AddSlaveTab;
            master.OnSlaveDisconnected += (slave) => {
                if (slaveTabs.ContainsKey(slave)) {
                    Invoke(new Action(() => {
                        TabPage slaveTab = slaveTabs[slave];

                        this.slavesTab.SuspendLayout();
                        slaveTab.SuspendLayout();
                        this.SuspendLayout();

                        this.slavesTab.TabPages.Remove(slaveTab);
                        slaveTabs.Remove(slave);

                        this.slavesTab.ResumeLayout(false);
                        slaveTab.ResumeLayout(false);
                        this.ResumeLayout(false);
                    }));
                }
            };

            foreach (var slave in master.Slaves)
                AddSlaveTab(slave);
        }

        private void RefreshDrawings() {
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
                slaveTab.Size = new System.Drawing.Size(slavesTab.Size.Width - 12, 231);
                slaveTab.TabIndex = 0;
                slaveTab.Text = name;
                slaveTab.UseVisualStyleBackColor = true;
                // 
                // rotationRotation
                // 
                rotationOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                rotationOffset.DisplayName = name + " Rotation";
                rotationOffset.Location = rawRotation.Location;
                rotationOffset.Name = name + "Rotation";
                rotationOffset.Pitch = 0F;
                rotationOffset.Size = rawRotation.Size;
                rotationOffset.TabIndex = 0;
                rotationOffset.Yaw = 0F;
                rotationOffset.OnChange += new System.EventHandler(this.rawRotation_OnChange);
                // 
                // positionOffset
                // 
                positionOffset.DisplayName = name + " Position";
                positionOffset.Location = rawPosition.Location;
                positionOffset.Max = maxOffset;
                positionOffset.Min = -maxOffset;
                positionOffset.Name = name + "Position";
                positionOffset.Size = rawPosition.Size;
                positionOffset.TabIndex = 1;
                positionOffset.X = 0F;
                positionOffset.Y = 0F;
                positionOffset.Z = 0F;
                positionOffset.OnChange += new System.EventHandler(this.rawPosition_OnChange);

                positionOffset.OnChange += (p, ep) => {
                    slave.PositionOffset = positionOffset.Value;
                    RefreshDrawings();
                };
                rotationOffset.OnChange += (p, ep) => {
                    slave.RotationOffset.Pitch = rotationOffset.Pitch;
                    slave.RotationOffset.Yaw = rotationOffset.Yaw;
                    hvSplit.Panel1.Refresh();
                    hvSplit.Panel2.Refresh();
                };

                this.slavesTab.ResumeLayout(false);
                slaveTab.ResumeLayout(false);
                this.ResumeLayout(false);
            });
            if (InvokeRequired)
                Invoke(add);
            else
                add();
        }

        private void MasterForm_FormClosing(object sender, FormClosingEventArgs e) {
            master.StopMaster();
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

        private void bindButton_Click(object sender, EventArgs e) {
            if (bindButton.Text.Equals("Bind")) {
                if (master.StartMaster(Int32.Parse(portBox.Text))) {
                    statusLabel.Text = "Bound to " + master.ProxyConfig.MasterAddress + ":" + master.ProxyConfig.MasterPort;
                    bindButton.Text = "Unbind";
                    portBox.Enabled = false;
                } else
                    statusLabel.Text = "Unable to bind to " + master.ProxyConfig.MasterAddress + ":" + master.ProxyConfig.MasterPort;
            } else {
                master.StopMaster();
                bindButton.Text = "Bind";
                statusLabel.Text = "Unbound";
                portBox.Enabled = true;
            }
        }

        private int x, y, currentX, currentY;
        private float pitch, yaw;
        private bool mouseDown;

        private void mouseTab_MouseDown(object sender, MouseEventArgs e) {
            x = e.X;
            y = e.Y;
            currentX = e.X;
            currentY = e.Y;
            pitch = rawRotation.Pitch;
            yaw = rawRotation.Yaw;
            mouseDown = true;
        }

        private Point GetCentre(Control c) {
            if (c.Parent == null)
                return c.Location;

            Point parentLoc = GetCentre(c.Parent);
            return new Point(c.Location.X + parentLoc.X, c.Location.Y + parentLoc.Y);
        }

        private void mouseTab_MouseUp(object sender, MouseEventArgs e) {
            mouseDown = false;
            Point centre = new Point(mousePanel.Width / 2, mousePanel.Height / 2);
            System.Windows.Forms.Cursor.Position = mousePanel.PointToScreen(centre);
            mousePanel.Refresh();
        }

        private void mouseTab_MouseMove(object sender, MouseEventArgs e) {
            if (mouseDown) {
                currentX = e.X;
                currentY = e.Y;
                master.Rotation.Pitch = pitch + (((e.Y - y) * mouseScaleSlider.Value) / 20);
                master.Rotation.Yaw = yaw + ((((x - e.X) / 2) * mouseScaleSlider.Value) / 10);
                mousePanel.Refresh();
            }
        }

        private void mousePanel_Paint(object sender, PaintEventArgs e) {
            if (mouseDown)
                e.Graphics.DrawLine(new Pen(Color.Black), x, y, currentX, currentY);
            else {
                e.Graphics.Clear(mousePanel.BackColor);
                string text = "Click and Drag to Mouselook";
                Point pos = new Point((e.ClipRectangle.Width / 2) - 70, e.ClipRectangle.Height / 2);
                e.Graphics.DrawString(text, label1.Font, Brushes.Black, pos);
            }
        }

        private bool leftDown, rightDown, forwardDown, backwardDown;
        private bool upDown, downDown;
        private bool yawRightDown, yawLeftDown, pitchUpDown, pitchDownDown;

        private void keyDown(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.A: leftDown = true; break;
                case Keys.D: rightDown = true; break;
                case Keys.W: forwardDown = true; break;
                case Keys.S: backwardDown = true; break;
                case Keys.PageUp: upDown = true; break;
                case Keys.PageDown: downDown = true; break;
                case Keys.Left: yawLeftDown = true; break;
                case Keys.Right: yawRightDown = true; break;
                case Keys.Up: pitchUpDown = true; break;
                case Keys.Down: pitchDownDown = true; break;
            }
        }

        private void keyUp(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.A: leftDown = false; break;
                case Keys.D: rightDown = false; break;
                case Keys.W: forwardDown = false; break;
                case Keys.S: backwardDown = false; break;
                case Keys.PageUp: upDown = false; break;
                case Keys.PageDown: downDown = false; break;
                case Keys.D4: yawLeftDown = false; break;
                case Keys.D6: yawRightDown = false; break;
                case Keys.D8: pitchUpDown = false; break;
                case Keys.D5: pitchDownDown = false; break;
            }
        }

        private void moveTimer_Tick(object sender, EventArgs e) {
            float shift = .05f * moveScaleSlider.Value;
            if (yawLeftDown) master.Rotation.Yaw -= shift;
            if (yawRightDown) master.Rotation.Yaw += shift;
            if (pitchUpDown) master.Rotation.Pitch += shift;
            if (pitchDownDown) master.Rotation.Pitch -= shift;

            Vector3 move = Vector3.Zero;
            if (forwardDown) move.X += shift;
            if (backwardDown) move.X -= shift;
            if (leftDown) move.Y += shift;
            if (rightDown) move.Y -= shift;
            if (upDown) move.Z += shift;
            if (downDown) move.Z -= shift;

            if (move != Vector3.Zero) {
                move *= rawRotation.Rotation;
                master.Position += move;
            }
        }   
    }
}
