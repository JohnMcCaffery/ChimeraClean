using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using CefSharp.WinForms;
using CefSharp.Example;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Chimera.Util;

namespace SandboxTest {
    public partial class UDPTest : Form {
	private const int LISTEN_PORT = 5001;
	private const int TARGET_PORT = 5000;
        private const int BUFLEN = 2048;
        private UdpClient mServer;
        private byte[] mData = new Byte[BUFLEN];
        private bool mShutdown = false;

	private ChromiumWebBrowser mBrowser;

        public UDPTest() {
            InitializeComponent(); 
            

            // 
            // panel1
            // 
            mBrowser = new ChromiumWebBrowser("http://openvirtualworlds.org/omeka/exhibits/show/groamhouse/rosemarkie-stone-information");
            mBrowser.Location = new System.Drawing.Point(12, 52);
            mBrowser.Name = "WebPanel";
            mBrowser.Dock = DockStyle.Fill;
            mBrowser.RegisterJsObject("bound", new BoundObject());
            this.MainSplit.Panel1.Controls.Add(mBrowser);
        }

        private void Start() {
            sendButton.Text = "Send";

            mServer = new UdpClient(LISTEN_PORT);
            ReceiveMessages();
            SendString("C# started");
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public static bool messageReceived = false;

        public class UdpState {
            public IPEndPoint e;
            public UdpClient u;
        }

        public void ReceiveCallback(IAsyncResult ar) {
            ReceiveMessages();

            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            try {
                Byte[] bytes = mServer.EndReceive(ar, ref e);
                string receiveString = Encoding.ASCII.GetString(bytes);

                Console.WriteLine("Received: {0} from {1}", receiveString, e.Port);
                Invoke(new Action(() => responseLabel.Text = Encoding.ASCII.GetString(bytes, 0, bytes.Length)));
                ProcessString(receiveString);
            } catch (ObjectDisposedException ex) { }
        }

        private void ProcessString(string str) {
            switch (str) {
                case "Front": mBrowser.Load("http://openvirtualworlds.org/omeka/exhibits/show/groamhouse/pictish-symbols/pictish-designs-and-symbols"); break;
                case "Back": mBrowser.Load("http://openvirtualworlds.org/omeka/exhibits/show/groamhouse/pictish-symbols/pictish-animals"); break;
            }
        }

        private void SendString(string str) {            IPAddress serverAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(serverAddr, TARGET_PORT);

            byte[] send_buffer = Encoding.ASCII.GetBytes(str);

            try {
                mServer.Send(send_buffer, send_buffer.Length, endPoint);
		Console.WriteLine("Sent " + str + " to " + TARGET_PORT);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Action debug = new Action(() => responseLabel.Text = e.Message);
                if (InvokeRequired)
                    Invoke(debug);
                else
                    debug();
            }
        }

        public void ReceiveMessages() {
            // Receive a message and write it to the console.
            IPEndPoint e = new IPEndPoint(IPAddress.Any, LISTEN_PORT);
            UdpState s = new UdpState();
            s.e = e;
            s.u = mServer;

            Console.WriteLine("listening for messages");
            try {
                mServer.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            } catch (ObjectDisposedException ex) { }	
        }

        private void sendButton_Click(object sender, EventArgs e) {
            if (sendButton.Text == "Start") {
                Start();
            }
            SendString(toSendBox.Text);
        }
        private void UDPTest_FormClosing(object sender, FormClosingEventArgs e) {
            mShutdown = true;
            if (mServer != null) {
                mServer.Close();
            }
        }

        private void toSendBox_TextChanged(object sender, EventArgs e) {
            sendButton_Click(sendButton, e);
        }

        private void unrealPanel_DoubleClick(object sender, EventArgs e) {
            Process p = Process.Start("E:\\Engines\\Unreal Editor\\ChimeraLinkTest\\WindowsNoEditor\\ChimeraLinkTest\\Binaries\\Win64\\ChimeraLinkTest.exe");
            Thread.Sleep(15000); // Allow the process to open it's window
            ProcessController pc = new ProcessController(p);
            pc.FullScreen = true;
            SetParent(p.MainWindowHandle, unrealPanel.Handle);

            Start();
        }
    }
}
