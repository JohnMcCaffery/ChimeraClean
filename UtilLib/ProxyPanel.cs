using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GridProxy;

namespace UtilLib {
    public partial class ProxyPanel : UserControl {
        private Proxy proxy;
        private bool loggedIn;

        public Proxy Proxy { get { return proxy; } }
        public bool HasStarted { get { return proxy != null; } }

        public string Port {
            get { return portBox.Text; }
            set { portBox.Text = value; }
        }
        public string ListenIP {
            get { return listenIPBox.Text; }
            set { listenIPBox.Text = value; }
        }
        public string LoginURI {
            get { return loginURIBox.Text; }
            set { loginURIBox.Text = value; }
        }

        public event EventHandler LoggedIn;
        public event EventHandler OnStarted;

        public ProxyPanel() {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (proxy != null)
                proxy.Stop();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string portArg = "--proxy-login-port="+portBox.Text;
            string listenIPArg = "--proxy-client-facing-address="+listenIPBox.Text;
            string loginURIArg = "--proxy-remote-login-uri="+loginURIBox.Text;
            string[] args = { portArg, listenIPArg, loginURIArg };
            ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            proxy = new Proxy(config);
            proxy.AddLoginResponseDelegate(response => {
                loggedIn = true;
                if (LoggedIn != null)
                    LoggedIn(proxy, null);
                return response;
            });

            proxy.Start();

            if (OnStarted != null)
                OnStarted(proxy, null);

            startButton.Text = "ReStart";
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            if (loggedIn) {
                startButton.Enabled = false;
                portBox.Enabled = false;
                listenIPBox.Enabled = false;
                loginURIBox.Enabled = false;
            } 
        }
    }
}
