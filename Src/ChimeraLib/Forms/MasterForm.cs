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
using ChimeraLib;
using ChimeraLib.Controls;
using OpenMetaverse.Skeletal.Kinect;
using OpenMetaverse.Skeletal;
using OpenMetaverse.Packets;
using GridProxy;

namespace ConsoleTest {
    public partial class MasterForm : Form {
        private readonly CameraMaster master;
        private readonly Dictionary<string, TabPage> slaveTabs = new Dictionary<string, TabPage>();
        private readonly Dictionary<string, Color> slaveColours = new Dictionary<string, Color>();
        private bool ignorePitch = false;
        private KinectSource k = new KinectSource();

        public MasterForm() : this(new CameraMaster()) { }

        public MasterForm(CameraMaster master) {
            InitializeComponent();

            proxyPanel.Proxy = master;
            masterWindowPanel.Window = master.Window;

            this.master = master;

            rawPosition.Value = master.Position;
            cameraOffsetPanel.Value = master.CameraOffset;
            rawRotation.LookAtVector = master.Rotation.LookAtVector;

            addressBox.Text = master.ProxyConfig.MasterAddress;
            portBox.Text = master.ProxyConfig.MasterPort.ToString();


            k.OnImage += imageFrame => {
                float xScale = (float)kinectFramePanel.Width / (float)k.ImageWidth;
                float yScale = (float)kinectFramePanel.Height / (float)k.ImageHeight;
                int width = xScale > yScale ? (int)(kinectFramePanel.Height * ((float)k.ImageWidth / (float)k.ImageHeight)) : kinectFramePanel.Width;
                int height = yScale > xScale ? (int)(kinectFramePanel.Width * ((float)k.ImageHeight / (float)k.ImageWidth)) : kinectFramePanel.Height;

                lock (this) {
                    Bitmap scaled = new Bitmap(imageFrame, width, height);
                    kinectFramePanel.Image = scaled;
                }
            };
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);
            Vector3 scale = new Vector3(-1000f, 1000f, 1000f);
            k.OnChange += position => {
                master.Window.EyePosition = position * scale;
                Vector3 oldMin = min;
                Vector3 oldMax = max;
                if (min.X > position.X)
                    min.X = position.X;
                if (min.Y > position.Y)
                    min.Y = position.Y;
                if (min.Z > position.Z)
                    min.Z = position.Z;

                if (max.X < position.X)
                    max.X = position.X;
                if (max.Y < position.Y)
                    max.Y = position.Y;
                if (max.Z < position.Z)
                    max.Z = position.Z;

                if (oldMin != min)
                    Console.WriteLine("Min: " + min);
                if (oldMax != max)
                    Console.WriteLine("Max: " + min);
            };

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
                    BeginInvoke(a);
                else
                    a();
            };
            master.Window.OnEyeChange += (diff) => {
                if (master.Window.LockScreenPosition)
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
            Action a = () => {
                hvSplit.Panel1.Refresh();
                hvSplit.Panel2.Refresh();
            };
            if (InvokeRequired)
                Invoke(a);
            else
                a();
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
                slaveTab.Size = new Size(masterTab.Width, slaveTab.Height + colourButton.Height);
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
            k.Stop();
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
        private float scaleScale = 6000000f;

        private void hTab_Paint(object sender, PaintEventArgs e) {
            Vector3 origin = new Vector3(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2f, 0f);
            e.Graphics.DrawLine(Pens.LightGreen, 0f, origin.Y, e.ClipRectangle.Width, origin.Y);
            e.Graphics.DrawLine(Pens.Red, origin.X, 0f, origin.X, e.ClipRectangle.Height);

            float scale = ((scaleBar.Maximum - scaleBar.Value) * e.ClipRectangle.Width) / scaleScale;
            foreach (var slave in master.Slaves) {
                lock (slaveColours)
                    DrawWindow(e.Graphics, slave.Window, origin, true, master.Window.EyePosition, slaveColours.ContainsKey(slave.Name) ? slaveColours[slave.Name] : Color.Black, scale);
            }
            DrawWindow(e.Graphics, master.Window, origin, true, Vector3.Zero, Color.Black, scale);

        }

        private void vTab_Paint(object sender, PaintEventArgs e) {
            Vector3 origin = new Vector3(e.ClipRectangle.Width / 2f, e.ClipRectangle.Height / 2f, 0f);
            e.Graphics.DrawLine(Pens.Red, 0f, origin.Y, e.ClipRectangle.Width, origin.Y);
            e.Graphics.DrawLine(Pens.Blue, origin.X, 0f, origin.X, e.ClipRectangle.Height);

            float scale = ((scaleBar.Maximum - scaleBar.Value) * e.ClipRectangle.Width) / scaleScale;
            foreach (var slave in master.Slaves) {
                lock (slaveColours)
                    DrawWindow(e.Graphics, slave.Window, origin, false, master.Window.EyePosition, slaveColours.ContainsKey(slave.Name) ? slaveColours[slave.Name] : Color.Black, scale);
            }
            DrawWindow(e.Graphics, master.Window, origin, false, Vector3.Zero, Color.Black, scale);

        }

        private void DrawWindow(Graphics g, Window window, Vector3 paneOrigin, bool yaw, Vector3 originOffset, Color colour, float scale) {
            Vector3 end1 = new Vector3(yaw ? 0f : -(float)window.Height/2f, yaw ? -(float)window.Width/2f : 0f, 0f) * scale;
            Vector3 end2 = new Vector3(yaw ? 0f :  (float)window.Height/2f, yaw ?  (float)window.Width/2f : 0f, 0f) * scale;
            Vector3 origin = (originOffset + window.EyePosition) * scale;
            Vector3 screenPosition = window.ScreenPosition * scale;
            Vector3 diff = screenPosition - origin;

            Quaternion q = Quaternion.CreateFromEulers(0f, 0f, (float) Rotation.DEG2RAD * window.RotationOffset.Yaw);
            Vector3 lookAt = new Vector3(window.RotationOffset.LookAtVector.X, window.RotationOffset.LookAtVector.Y, 0f);
            //Vector3 avatar = screenPosition - (master.CameraOffset * 100 * scale);

            if (!yaw) {
                //avatar = to2DV(avatar);
                screenPosition = to2DV(screenPosition);
                diff = to2DV(diff);
                lookAt = to2DV(window.RotationOffset.LookAtVector);
                origin = screenPosition - diff;
                q = Quaternion.CreateFromEulers(0f, 0f, (float)Rotation.DEG2RAD * window.RotationOffset.Pitch);
            }

            float lookatDistance = Vector3.Dot(diff, Vector3.Normalize(lookAt));
            lookAt = new Vector3(yaw ? lookatDistance : 0f, yaw ? 0f : lookatDistance, 0f);

            lookAt *= q;
            end1 *= q;
            end2 *= q;

            Vector3 cross1 = screenPosition + (Vector3.Normalize(lookAt) * 30f * scale);
            Vector3 cross2 = screenPosition + (Vector3.Normalize(lookAt) * -30f * scale);
            Vector3 end1Far = ((end1 + diff) * 3) + origin;
            Vector3 end2Far = ((end2 + diff) * 3) + origin;

            end1 += diff + origin;
            end2 += diff + origin;
            lookAt += origin;

            g.DrawLine(new Pen(colour), toPoint(end1, paneOrigin, true), toPoint(end2, paneOrigin, true));
            g.DrawLine(new Pen(colour), toPoint(cross1, paneOrigin, true), toPoint(cross2, paneOrigin, true));
            g.DrawLine(new Pen(colour), toPoint(origin, paneOrigin, true), toPoint(end1Far, paneOrigin, true));
            g.DrawLine(new Pen(colour), toPoint(origin, paneOrigin, true), toPoint(end2Far, paneOrigin, true));
            g.DrawLine(new Pen(colour), toPoint(origin, paneOrigin, true), toPoint(lookAt, paneOrigin, true));
            //g.DrawLine(new Pen(colour), toPoint(origin, paneOrigin, true), toPoint(avatar, paneOrigin, true));

            Point centre = toPoint(origin, paneOrigin, true);
            //Point avatarP = toPoint(avatar, paneOrigin, true);

            g.FillEllipse(new SolidBrush(colour), centre.X - r, centre.Y - r, r * 2, r * 2);
            //g.FillEllipse(new SolidBrush(colour), avatarP.X - r, avatarP.Y - r, r * 2, r * 2);
        }

        private Vector3 to2DV(Vector3 v) {
            return new Vector3(v.Z, new Vector2(v.X, v.Y).Length(), 0f);
        }

        /// <summary>
        /// Only takes the x and y component when creating the point
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Point toPoint(Vector3 v, Vector3 centre, bool yaw) {
            return new Point((int)(v.Y + centre.X), (int)(centre.Y - v.X));
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
                case Keys.Left: yawLeftDown = false; break;
                case Keys.Right: yawRightDown = false; break;
                case Keys.Up: pitchUpDown = false; break;
                case Keys.Down: pitchDownDown = false; break;
            }
        }

        private void moveTimer_Tick(object sender, EventArgs e) {
            float shift = .05f * moveScaleSlider.Value;
            //if (yawLeftDown) master.Rotation.Yaw -= shift;
            //if (yawRightDown) master.Rotation.Yaw += shift;
            if (!ignorePitch) {
                //if (pitchUpDown) master.Rotation.Pitch += shift;
                //if (pitchDownDown) master.Rotation.Pitch -= shift;
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
                //master.Position += move;
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

        private void cameraOffsetPanel_OnChange(object sender, EventArgs e) {
            master.CameraOffset = cameraOffsetPanel.Value;
        }

        private void frustumHCheck_CheckedChanged(object sender, EventArgs e) {
            master.UpdateFrustumH = frustumHCheck.Checked;
        }

        private void frustumVCheck_CheckedChanged(object sender, EventArgs e) {
            master.UpdateFrustumV = frustumVCheck.Checked;
        }

        private void fovCheck_CheckedChanged(object sender, EventArgs e) {
            master.UpdateFoV = fovCheck.Checked;
        }

        private void aspectRatioCheck_CheckedChanged(object sender, EventArgs e) {
            master.UpdateAspectRatio = aspectRatioCheck.Checked;
        }

        private void eyeOffsetCheck_CheckedChanged(object sender, EventArgs e) {
            master.UpdateEyeOffset = eyeOffsetCheck.Checked;
        }

        private void rotationCheck_CheckedChanged(object sender, EventArgs e) {
            master.UpdateRotation = rotationCheck.Checked;
        }

        private void frustumNearCheck_CheckedChanged(object sender, EventArgs e) {
            master.UpdateFrustumNear = frustumNearCheck.Checked;
        }

        private void viewerControlCheck_CheckedChanged(object sender, EventArgs e) {
            master.ViewerControl = viewerControlCheck.Checked;
        }
    }
}
