using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Chimera.RemoteControl {
    public class RemoteControlPlugin : ISystemPlugin {
        public const string SHUTDOWN = "Exit";
        public const int PORT = 8050;

        private Core mCore;
        private Form mForm;
        private bool mEnabled;
        private bool mListening;
        private UdpClient mListener;

        public void Init(Core coordinator) {
            mCore = coordinator;
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
                    else if (mListening)
                        StopListening();
                        StopListening();
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
            mListener = new UdpClient(PORT);
            Thread listenThread = new Thread(ListenThread);
            listenThread.Name = "RemoteControlListener";
            listenThread.Start();
        }

        private void ListenThread() {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, PORT);
            byte[] data = mListener.Receive(ref ep);
            string msg = Encoding.ASCII.GetString(data);

            if (msg == SHUTDOWN)
                mForm.Close();
            else if (mCore.HasFrame(msg))
                mCore[msg].Output.Restart("RemoteInstructionReceived");
        }

        private void StopListening() {
            if (mListener != null) {
                mListener.Close();
            }
        }
    }
}
