using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinGridProxy;
using OpenMetaverse;

namespace UtilLib {
    public partial class LogPanel : UserControl {
        public LogPanel() {
            InitializeComponent();

            Logger.Log("Log Panel Ready", Helpers.LogLevel.Info);
            if (FireEventAppender.Instance != null) {
                FireEventAppender.Instance.MessageLoggedEvent += new MessageLoggedEventHandler(Instance_MessageLoggedEvent);
            }
        }

        void Instance_MessageLoggedEvent(object sender, MessageLoggedEventArgs e)
        {
            if (this.IsDisposed || this.Disposing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Instance_MessageLoggedEvent(sender, e)));
            }
            else
            {
                string s = String.Format("{0} [{1}] {2} {3}", e.LoggingEvent.TimeStamp, e.LoggingEvent.Level,
                    e.LoggingEvent.RenderedMessage, e.LoggingEvent.ExceptionObject);
                debugTextBox.AppendText(s + "\n");
            }
        }
    }
}
