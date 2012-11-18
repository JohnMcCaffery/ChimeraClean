using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;

namespace ConsoleTest {
    public partial class ProxyForm : Form {
        ProxyManager Proxy {
            get { return proxyPanel.Proxy; }
            set { proxyPanel.Proxy = value; }
        }

        public ProxyForm() {
            InitializeComponent();
        }

        public ProxyForm(ProxyManager proxy) : this() {
            proxyPanel.Proxy = proxy;
        }

        private void ProxyForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (proxyPanel.Proxy != null)
                proxyPanel.Proxy.StopProxy();
        }
    }
}
