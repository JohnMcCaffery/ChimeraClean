<<<<<<< HEAD
﻿/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
﻿using System;
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
        private readonly Dictionary<string, Color> slaveColours = new Dictionary<string, Color>();
        private bool ignorePitch = false;
        private bool changing;

        public MasterForm() : this(new CameraMaster()) { }
        public MasterForm(CameraMaster master) {
            InitializeComponent();

            proxyPanel.Proxy = master;

            this.master = master;

            rawPosition.Value = master.Position;
            rawRotation.LookAtVector = master.Rotation.LookAtVector;

            addressBox.Text = master.ProxyConfig.MasterAddress;
            portBox.Text = master.ProxyConfig.MasterPort.ToString();

            if (master.MasterRunning) {
                statusLabel.Text = "Bound to " + master.MasterAddress + ":" + master.ProxyConfig.MasterPort;
                bindButton.Text = "Unbind";
                addressBox.Enabled = false;
                portBox.Enabled = false;
            }

            master.OnMasterBound += (source, args) => {
                statusLabel.Text = "Bound to " + source;
                bindButton.Text = "Unbind";
                addressBox.Enabled = false;
                portBox.Enabled = false;
            };

            master.OnProxyStarted += (source, args) => {
                BeginInvoke(new Action(() => Text = "Master: " + master.ProxyConfig.ProxyPort));
            };
            master.OnCameraUpdated += (source, args) => {
                Action a = () => {
                    rawPosition.Value = master.Position;
                    rawRotation.LookAtVector = master.Rotation.LookAtVector;
                    RefreshDrawings();
                    receivedLabel.Text = master.PacketsReceived.ToString();
                    processedLabel.Text = master.PacketsProcessed.ToString();
                    generatedLabel.Text = master.PacketsGenerated.ToString();
                    forwardedLabel.Text = master.PacketsForwarded.ToString();
                };
                if (InvokeRequired)
                    Invoke(a);
                else
                    a();
            };
            master.OnSlaveConnected += AddSlaveTab;
            master.OnSlaveDisconnected += RemoveSlave;

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
                Button colourButton = new Button();
                this.slavesTabContainer.SuspendLayout();
                slaveTab.SuspendLayout();
                this.debugTab.SuspendLayout();
                this.SuspendLayout();

                string name = slave.Name;
                this.slaveTabs.Add(name, slaveTab);
                lock (slaveColours)
                    if (!slaveColours.ContainsKey(name))
                        this.slaveColours.Add(name, Color.Purple);

                // 
                // mainTab
                // 
                this.slavesTabContainer.TabPages.Remove(this.debugTab);
                this.slavesTabContainer.Controls.Add(slaveTab);
                this.slavesTabContainer.Controls.Add(this.debugTab);
                // 
                // slaveTab
                // 
                slaveTab.AutoScroll = true;
                slaveTab.Controls.Add(rotationOffset);
                slaveTab.Controls.Add(positionOffset);
                slaveTab.Controls.Add(colourButton);
                slaveTab.Location = new System.Drawing.Point(4, 22);
                slaveTab.Name = name + "Tab";
                slaveTab.Padding = new System.Windows.Forms.Padding(3);
                slaveTab.Size = new System.Drawing.Size(slavesTabContainer.Size.Width - 12, 231);
                slaveTab.TabIndex = 0;
                slaveTab.Text = name;
                slaveTab.UseVisualStyleBackColor = true;
                // 
                // rotationRotation
                // 
                rotationOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
                rotationOffset.DisplayName = name + " Rotation";
                rotationOffset.Location = rawRotation.Location;
                rotationOffset.Name = name + "Rotation";
                rotationOffset.Pitch = 0F;
                rotationOffset.Size = rawRotation.Size;
                rotationOffset.TabIndex = 0;
                rotationOffset.Yaw = 0F;
                // 
                // positionOffset
                // 
                positionOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
                positionOffset.DisplayName = name + " Position";
                positionOffset.Location = rawPosition.Location;
                positionOffset.Max = maxOffset;
                positionOffset.Min = -maxOffset;
                positionOffset.Name = name + "Position";
                positionOffset.Size = rawPosition.Size;
                positionOffset.TabIndex = 2;
                positionOffset.X = 0F;
                positionOffset.Y = 0F;
                positionOffset.Z = 0F;
                //
                // colourButton
                //
                colourButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
                colourButton.Text = "Choose Colour";
                colourButton.Location = new Point(positionOffset.Location.X, positionOffset.Location.Y + positionOffset.Size.Height);
                colourButton.Name = name + "ChooseColour";
                colourButton.Size = new Size(rawPosition.Size.Width, bindButton.Size.Height);
                colourButton.TabIndex = 3;

                positionOffset.OnChange += (p, ep) => {
                    slave.PositionOffset = positionOffset.Value;
                    RefreshDrawings();
                };
                rotationOffset.OnChange += (p, ep) => {
                    slave.RotationOffset.Pitch = rotationOffset.Pitch;
                    slave.RotationOffset.Yaw = rotationOffset.Yaw;
                    RefreshDrawings();
                };
                colourButton.Click += (source, args) => {
                    BeginInvoke(new Action(() => {
                        if (slaveColourPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            slaveColours[slave.Name] = slaveColourPicker.Color;
                        RefreshDrawings();
                    }));
                };

                this.slavesTabContainer.ResumeLayout(false);
                slaveTab.ResumeLayout(false);
                this.ResumeLayout(false);
            });
            if (InvokeRequired)
                Invoke(add);
            else
                add();
        }

        private void RemoveSlave(string slave) {
            if (slaveTabs.ContainsKey(slave)) {
                Invoke(new Action(() => {
                    TabPage slaveTab = slaveTabs[slave];

                    this.slavesTabContainer.SuspendLayout();
                    slaveTab.SuspendLayout();
                    this.SuspendLayout();

                    this.slavesTabContainer.TabPages.Remove(slaveTab);
                    slaveTabs.Remove(slave);
                    slaveColours.Remove(slave);

                    this.slavesTabContainer.ResumeLayout(false);
                    slaveTab.ResumeLayout(false);
                    this.ResumeLayout(false);
                }));
            }
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
            float scale = Math.Min(w, h);
            Point origin = new Point(w / 2, h / 2);
            Vector3 originV = new Vector3(w / 2, h / 2, 0);

            float offsetMultiplierH = (scale / 2) / maxOffset;
            float offsetMultiplierV = (scale / 2) / maxOffset;

            foreach (var slave in master.Slaves) {
                Vector3 slaveOffset = slave.PositionOffset * master.Rotation.Quaternion;
                Point slaveOrigin = new Point(origin.X + (int)(slaveOffset.Y * offsetMultiplierH), origin.Y + (int)(-slaveOffset.X * offsetMultiplierV));

                Rotation rot;
                if (!lockMasterCheck.Checked)
                    rot = new Rotation(master.Rotation.Pitch + slave.RotationOffset.Pitch, master.Rotation.Yaw + slave.RotationOffset.Yaw);
                else
                    rot = slave.RotationOffset;
                Point slaveLookAtEnd = new Point(slaveOrigin.X + (int)(rot.LookAtVector.Y * scale), slaveOrigin.Y + (int)(-rot.LookAtVector.X * scale));

                lock (slaveColours)
                    if (!slaveColours.ContainsKey(slave.Name))
                        slaveColours.Add(slave.Name, Color.Purple);
                g.DrawLine(new Pen(slaveColours[slave.Name]), slaveOrigin, slaveLookAtEnd);
            }

            g.DrawLine(Pens.LightGreen, 0f, h / 2f, w, h / 2f);
            g.DrawLine(Pens.Red, w / 2f, 0f, w / 2f, h);
            g.FillEllipse(Brushes.Black, new Rectangle(origin.X - r, origin.Y - r, r * 2, r * 2));

            float scaleV = (scaleBar.Maximum - scaleBar.Value) / 100f;
            float masterLeftX = (float) ((decimal.ToDouble(widthValue.Value) / -2) + frustumPosition.X);
            float masterRightX = (float) ((decimal.ToDouble(widthValue.Value) / 2) + frustumPosition.X);
            Vector3 masterLeft = new Vector3(masterLeftX, -frustumPosition.Z * 2, 0) * scaleV;
            Vector3 masterRight = new Vector3(masterRightX, -frustumPosition.Z * 2, 0) * scaleV;

            if (!lockMasterCheck.Checked) {
                Vector3 masterLookAt = new Vector3(0f, -masterRight.Length() * 3, 0f);

                Quaternion q = Quaternion.CreateFromEulers(0f, 0f, (float) (master.Rotation.Yaw * Rotation.DEG2RAD));
                masterLookAt *= q;
                masterLeft *= q;
                masterRight *= q;

                g.DrawLine(Pens.Black, origin, toPointH(masterLookAt + originV));
            }
            g.DrawLine(Pens.Black, toPointH(masterLeft + originV), toPointH(masterRight + originV));
            g.DrawLine(Pens.Black, origin, toPointH((masterLeft * 3) + originV));
            g.DrawLine(Pens.Black, origin, toPointH((masterRight * 3) + originV));
        }

        private void vTab_Paint(object sender, PaintEventArgs e) {
            //float scale = scaleBar.Value / 10f;
            Graphics g = e.Graphics;
            int w = e.ClipRectangle.Width;
            int h = e.ClipRectangle.Height;
            float scale = Math.Min(w, h);
            Point origin = new Point(r, h / 2);
            Vector3 originV = new Vector3(r, 0f, h / 2);

            float offsetMultiplierH = (scale / 2) / maxOffset;
            float offsetMultiplierV = (scale / 2) / maxOffset;

            foreach (var slave in master.Slaves) {
                Vector3 slaveOffset = slave.PositionOffset * master.Rotation.Quaternion;
                Point slaveOrigin = new Point(origin.X + (int)(slaveOffset.X * offsetMultiplierH), origin.Y + (int)(-slaveOffset.Z * offsetMultiplierV));

                Rotation rot;
                if (!lockMasterCheck.Checked)
                    rot = new Rotation(master.Rotation.Pitch + slave.RotationOffset.Pitch, master.Rotation.Yaw + slave.RotationOffset.Yaw);
                else
                    rot = slave.RotationOffset;
                Vector2 slaveHVector = new Vector2(rot.LookAtVector.Y, rot.LookAtVector.X);
                Vector3 slaveCross = Vector3.Cross(new Vector3(slaveHVector, 0f), Vector3.UnitX);
                int slaveX = (int)(slaveHVector.Length() * scale * (slaveCross.Z < 0 ? 1 : -1));
                Point slaveLookAtEnd = new Point(slaveOrigin.X + slaveX, slaveOrigin.Y + (int)(rot.LookAtVector.Z * scale));

                lock (slaveColours)
                    if (!slaveColours.ContainsKey(slave.Name))
                        slaveColours.Add(slave.Name, Color.Purple);
                g.DrawLine(new Pen(slaveColours[slave.Name]), slaveOrigin, slaveLookAtEnd);
            }

            g.DrawLine(Pens.LightGreen, 0f, h / 2f, w, h / 2f);
            g.DrawLine(Pens.Blue, r, 0f, r, h);
            g.FillEllipse(Brushes.Black, 0, origin.Y - r, r * 2, r * 2);

            float scaleV = (scaleBar.Maximum - scaleBar.Value) / 100f;
            float masterTopY = (float) ((decimal.ToDouble(heightValue.Value) / -2) + frustumPosition.Y);
            float masterBottomY = (float) ((decimal.ToDouble(heightValue.Value) / 2) + frustumPosition.Y);
            Vector3 masterTop = new Vector3(frustumPosition.Z * 2, 0, -masterTopY) * scaleV;
            Vector3 masterBottom = new Vector3(frustumPosition.Z * 2, 0, -masterBottomY) * scaleV;

            if (!lockMasterCheck.Checked) {
                Vector3 masterLookAt = new Vector3(masterTop.Length() * 3, 0f, 0f);

                Quaternion q = Quaternion.CreateFromEulers(0f, (float) (master.Rotation.Pitch * Rotation.DEG2RAD), 0f);
                masterLookAt *= q;
                masterTop *= q;
                masterBottom *= q;

                g.DrawLine(Pens.Black, origin, toPointV(masterLookAt + originV));
            
            }
            g.DrawLine(Pens.Black, toPointV(masterTop + originV), toPointV(masterBottom + originV));
            g.DrawLine(Pens.Black, origin, toPointV((masterTop * 3) + originV));
            g.DrawLine(Pens.Black, origin, toPointV((masterBottom * 3) + originV));
        }

        public static Point toPoint(Vector2 v) {
            return new Point((int) v.X, (int) v.Y);
        }
        public static Point toPointH(Vector3 v) {
            return new Point((int) v.X, (int) v.Y);
        }
        public static Point toPointV(Vector3 v) {
            return new Point((int) v.X, (int) v.Z);
        }
        public static Point combine(Point op1, Point op2) {
            return new Point(op1.X + op2.X, op2.Y + op1.Y);
        }
        public static Point mult(Point op1, int op2) {
            return new Point(op1.X * op2, op1.Y * op2);
        }
        /*
        public static Point operator +(Point op1, Point op2) {
            return new Point(op1.X + op2.X, op1.Y + op2.Y);
        }
        public static Point operator *(Point op1, Point op2) {
            return new Point(op1.X * op2.X, op1.Y * op2.Y);
        }*/

        private void bindButton_Click(object sender, EventArgs e) {
            if (bindButton.Text.Equals("Bind")) {
                if (!master.StartMaster(addressBox.Text, Int32.Parse(portBox.Text)))
                    statusLabel.Text = "Unable to bind to " + master.MasterAddress + ":" + master.ProxyConfig.MasterPort;
            } else {
                master.StopMaster();
                foreach (var slave in slaveTabs.Keys.ToArray())
                    RemoveSlave(slave);
                bindButton.Text = "Bind";
                statusLabel.Text = "Unbound";
                addressBox.Enabled = true;
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
                if (!ignorePitch)
                    master.Rotation.Pitch = pitch + (((e.Y - y) * mouseScaleSlider.Value) / 20);
                master.Rotation.Yaw = yaw + ((((x - e.X) / 2) * mouseScaleSlider.Value) / 10);
                mousePanel.Refresh();
            }
        }

        private void mousePanel_Paint(object sender, PaintEventArgs e) {
            if (mouseDown)
                e.Graphics.DrawLine(new Pen(Color.Black), x, y, currentX, ignorePitch ? y : currentY);
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
            if (!ignorePitch) {
                if (pitchUpDown) master.Rotation.Pitch += shift;
                if (pitchDownDown) master.Rotation.Pitch -= shift;
            }

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

        private void ignorePitchCheck_CheckedChanged(object sender, EventArgs e) {
            ignorePitch = ignorePitchCheck.Checked;
        }

        private void lockMasterCheck_CheckedChanged(object sender, EventArgs e) {
            RefreshDrawings();
        }

        private void frustumPosition_OnChange(object sender, EventArgs e) {
            RefreshDrawings();
        }
        private void widthScale_Scroll(object sender, EventArgs e) {
            if (!changing) {
                changing = true;
                //widthValue.Value = widthScale.Value / 100;
                widthValue.Value = widthScale.Value;
                changing = false;
            }
            RefreshDrawings();
        }

        private void widthValue_ValueChanged(object sender, EventArgs e) {
            if (!changing) {
                changing = true;
                //widthScale.Value = (int) (widthValue.Value * 100);
                widthScale.Value = (int) (widthValue.Value);
                changing = false;
            }
            RefreshDrawings();
        }

        private void heightValue_ValueChanged(object sender, EventArgs e) {
            if (!changing) {
                changing = true;
                //heightScale.Value = (int) (heightValue.Value * 100);
                heightSlider.Value = (int) (heightValue.Value);
                changing = false;
            }
            RefreshDrawings();

        }

        private void heightSlider_Scroll(object sender, EventArgs e) {
            if (!changing) {
                changing = true;
                //heightValue.Value = heightScale.Value / 100;
                heightValue.Value = heightSlider.Value;
                changing = false;
            }
            RefreshDrawings();

        }

        private void scaleBar_Scroll(object sender, EventArgs e) {
            RefreshDrawings();
        }
    }
}
