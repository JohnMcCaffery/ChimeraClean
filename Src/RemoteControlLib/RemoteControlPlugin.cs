using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using log4net;

namespace Chimera.RemoteControl {
    public class RemoteControlPlugin : ISystemPlugin {
        private static readonly ILog Logger = LogManager.GetLogger("RemoteControl");
        public const string SHUTDOWN = "Exit";
        private int mPort = 8050;

        private Core mCore;
        private Form mForm;
        private bool mEnabled;
        private bool mCont;
        private IPEndPoint ep;
        private UdpClient mListener;

        public void Init(Core coordinator) {
            mCore = coordinator;
            mPort = new RemoteControlConfig().Port;
            ep = new IPEndPoint(IPAddress.Any, mPort);
        }

        public void SetForm(System.Windows.Forms.Form form) {
            mForm = form;
        }

        public event Action<IPlugin, bool> EnabledChanged;

        public System.Windows.Forms.Control ControlPanel {
            get { return new Panel(); }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (value)
                        Listen();
                    else if (mListener != null)
                        StopListening();

                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "RemoteControl"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
            StopListening();
        }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }

        private void Listen() {
            mListener = new UdpClient(mPort);
            Thread listenThread = new Thread(ListenThread);
            listenThread.Name = "RemoteControlListener";
            listenThread.Start();
        }

        private void ListenThread() {
            mCont = true;
            while (mCont) {
                try {
                    byte[] data = mListener.Receive(ref ep);
                    string msg = Encoding.ASCII.GetString(data);

                    Logger.Info("Received '" + msg + "'.");

                    if (msg == SHUTDOWN) {
                        mCont = false;
                        mForm.Close();
                    } else if (mCore.HasFrame(msg))
                        mCore[msg].Output.Restart("RemoteInstructionReceived");
                } catch (Exception e) {
                    //Do nothing
                }
            }
        }

        private void StopListening() {
            mCont = false;
            if (mListener != null) {
                mListener.Close();
                mListener = null;
            }
        }
    }
}
