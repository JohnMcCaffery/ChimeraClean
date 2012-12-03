/*************************************************************************
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
using ChimeraLib;
using ChimeraLib.Controls;

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
            masterWindowPanel.Window = master.Window;

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
            master.Window.OnEyeChange += (diff) => {
                foreach (var slave in master.Slaves)
                    slave.Window.ScreenPosition += diff;
            };
            master.Window.OnChange += (source, args) => RefreshDrawings();
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
                WindowPanel slaveWindowPanel = new WindowPanel(slave.Window);
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
                slaveTab.Controls.Add(slaveWindowPanel);
                slaveTab.Controls.Add(colourButton);
                slaveTab.Location = new System.Drawing.Point(4, 22);
                slaveTab.Name = name + "Tab";
                slaveTab.Padding = new System.Windows.Forms.Padding(3);
                slaveTab.Size = new Size(rawTab.Width, slaveTab.Height + colourButton.Height);
                slaveTab.TabIndex = 0;
                slaveTab.Text = name;
                slaveTab.UseVisualStyleBackColor = true;
                // 
                // slaveWindowPanel
                // 
                slaveTab.AutoScroll = false;
                slaveWindowPanel.Anchor = (AnchorStyles)(
                  System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
                slaveWindowPanel.AutoScroll = true;
                slaveWindowPanel.Location = masterWindowPanel.Location;
                slaveWindowPanel.Name = "slaveWindowPanel";
                slaveWindowPanel.Size = new Size(slaveTab.Width, slaveWindowPanel.Height);
                slaveWindowPanel.TabIndex = 0;
                slaveWindowPanel.Window = null;
                //
                // colourButton
                //
                colourButton.Anchor = (AnchorStyles)(
                  System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
                colourButton.Text = "Choose Colour";
                colourButton.Location = new Point(slaveWindowPanel.Location.X, slaveWindowPanel.Location.Y + slaveWindowPanel.Size.Height);
                colourButton.Name = name + "ChooseColour";
                colourButton.Size = new Size(slaveWindowPanel.Size.Width, colourButton.Size.Height);
                colourButton.TabIndex = 1;

                slave.Window.OnChange += (p, ep) => {
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
            //master.VirtualRotationOffset.Quaternion = rawRotation.VirtualRotationOffset;
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
            Vector3 origin = new Vector3(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2f, 0f);
            foreach (var slave in master.Slaves) {
                lock (slaveColours)
                    DrawWindow(e.Graphics, slave.Window, origin, true, master.Window.EyeOffset, slaveColours.ContainsKey(slave.Name) ? slaveColours[slave.Name] : Color.Black);
            }
            DrawWindow(e.Graphics, master.Window, origin, true, Vector3.Zero, Color.Black);

            e.Graphics.DrawLine(Pens.LightGreen, 0f, origin.Y, e.ClipRectangle.Width, origin.Y);
            e.Graphics.DrawLine(Pens.Red, origin.X, 0f, origin.X, e.ClipRectangle.Height);
        }

        private void vTab_Paint(object sender, PaintEventArgs e) {
            Vector3 origin = new Vector3(r, e.ClipRectangle.Height / 2, 0f);
            foreach (var slave in master.Slaves) {
                lock (slaveColours)
                    DrawWindow(e.Graphics, slave.Window, origin, false, master.Window.EyeOffset, slaveColours.ContainsKey(slave.Name) ? slaveColours[slave.Name] : Color.Black);
            }
            DrawWindow(e.Graphics, master.Window, origin, false, Vector3.Zero, Color.Black);

            e.Graphics.DrawLine(Pens.LightGreen, 0f, origin.Y, e.ClipRectangle.Width, origin.Y);
            e.Graphics.DrawLine(Pens.Blue, r, 0f, r, e.ClipRectangle.Height);
        }

        private void DrawWindow(Graphics g, Window window, Vector3 paneOrigin, bool yaw, Vector3 originOffset, Color colour) {
            float scale = (scaleBar.Maximum - scaleBar.Value) / 2000f;
            float end1Free = (float) (((yaw ? window.Width : window.Height) / -2) + (yaw ? window.ScreenPosition.X : window.ScreenPosition.Y));
            float end2Free = (float) (((yaw ? window.Width : window.Height) / 2) + (yaw ? window.ScreenPosition.X : window.ScreenPosition.Y));
            float fixedEnd = window.ScreenPosition.Z;
            Vector3 end1 = new Vector3(yaw ? end1Free : fixedEnd, yaw ? -fixedEnd : -end1Free, 0f) * scale;
            Vector3 end2 = new Vector3(yaw ? end2Free : fixedEnd, yaw ? -fixedEnd : -end2Free, 0f) * scale;
            Vector3 origin = (originOffset + window.EyeOffset) * scale;
            if (!yaw)
                origin = new Vector3(-origin.Y, origin.Z, 0f);

            end1 += origin;
            end2 += origin;

            float angle = yaw ? window.RotationOffset.Yaw : window.RotationOffset.Pitch;
            Quaternion q = Quaternion.CreateFromEulers(0f, 0f, (float) (angle * Rotation.DEG2RAD));
            end1 *= q;
            end2 *= q;

            origin += paneOrigin;
            g.DrawLine(new Pen(colour), toPoint(end1 + paneOrigin), toPoint(end2 + paneOrigin));
            g.DrawLine(new Pen(colour), toPoint(origin), toPoint((end1 * 3) + origin));
            g.DrawLine(new Pen(colour), toPoint(origin), toPoint((end2 * 3) + origin));
            g.FillEllipse(new SolidBrush(colour), origin.X - r, origin.Y - r, r * 2, r * 2);
        }

        public static Point toPoint(Vector2 v) {
            return new Point((int) v.X, (int) v.Y);
        }
        /// <summary>
        /// Only takes the x and y component when creating the point
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Point toPoint(Vector3 v) {
            return new Point((int) v.X, (int) v.Y);
        }
        public static Point combine(Point op1, Point op2) {
            return new Point(op1.X + op2.X, op2.Y + op1.Y);
        }
        public static Point mult(Point op1, int op2) {
            return new Point(op1.X * op2, op1.Y * op2);
        }

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

        private void scaleBar_Scroll(object sender, EventArgs e) {
            RefreshDrawings();
        }
    }
}
