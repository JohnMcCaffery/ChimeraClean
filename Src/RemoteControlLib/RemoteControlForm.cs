using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Util;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
using log4net;

namespace Chimera.RemoteControl {
    public partial class RemoteControlForm : Form {
        private static readonly ILog Logger = LogManager.GetLogger("RemoteControl");
        private RemoteControlConfig mConfig = new RemoteControlConfig();
        private ProcessController mOpensim;

        public RemoteControlForm() {
            InitializeComponent();

            mOpensim = new ProcessController(mConfig.OpenSimExe, Path.GetDirectoryName(mConfig.OpenSimExe), "");
            if (File.Exists(mConfig.OpenSimExe))
                mOpensim.Start();

            TopMost = true;
            Text = mConfig.Title;
            shutdownCheck.Checked = mConfig.ShutdownEverything;
        }

        private void shutdownButton_Click(object sender, EventArgs e) {
            Send(RemoteControlPlugin.SHUTDOWN);
            if (shutdownCheck.Checked) {
                mOpensim.PressKey("q{ENTER}");
                Thread.Sleep(10000);
                Process.Start("shutdown", "/s /t 0");
            }
        }

        private void Send(string msg) {
            //From http://social.msdn.microsoft.com/Forums/en-US/92846ccb-fad3-469a-baf7-bb153ce2d82b/simple-udp-example-code

            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress send_to_address = IPAddress.Parse(mConfig.ClientAddress);
            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, mConfig.Port);

            // the socket object must have an array of bytes to send.
            // this loads the string entered by the user into an array of bytes.
            byte[] send_buffer = Encoding.ASCII.GetBytes(msg);

            // Remind the user of where this is going.
            Logger.Info(string.Format("sending {2} to {0}:{1}", sending_end_point.Address, sending_end_point.Port, msg));
            try {
                sending_socket.SendTo(send_buffer, sending_end_point);
            } catch (Exception e) {
                Logger.Warn(string.Format("Unable to send {2} to {0}:{1}", sending_end_point.Address, sending_end_point.Port, msg), e);
                Console.WriteLine(e.StackTrace);
            }
            sending_socket.Close();
        }

        private void leftButton_Click(object sender, EventArgs e) {
            Send("LeftWindow");
        }

        private void centreButton_Click(object sender, EventArgs e) {
            Send("MainWindow");
        }

        private void rightButton_Click(object sender, EventArgs e) {
            Send("RightWindow");
        }
    }
}
