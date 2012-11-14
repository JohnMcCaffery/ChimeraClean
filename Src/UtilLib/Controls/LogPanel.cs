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
using System.Threading;

namespace UtilLib {
    public partial class LogPanel : UserControl {
        private object createdLock = new object();
        private bool created = false;
        
        public LogPanel() {
            InitializeComponent();

            //Logger.Log("Log Panel Ready", Helpers.LogLevel.Info);
            if (FireEventAppender.Instance != null) 
                FireEventAppender.Instance.MessageLoggedEvent += new MessageLoggedEventHandler(Instance_MessageLoggedEvent);

            //Only start processing events when a handle has been created.
            HandleCreated += (source, args) => {
                Thread.Sleep(10);
                created = true;
                lock (createdLock)
                    Monitor.PulseAll(createdLock);
            };
        }

        protected override void DestroyHandle() {
            base.DestroyHandle();
            lock (createdLock)
                Monitor.PulseAll(createdLock);
        }

        void Instance_MessageLoggedEvent(object sender, MessageLoggedEventArgs e) {
            if (!created)
                lock (createdLock)
                    Monitor.Wait(createdLock);

            if (!created || this.IsDisposed || this.Disposing)
                return;

            string s = String.Format("{0} [{1}] {2} {3}", e.LoggingEvent.TimeStamp, e.LoggingEvent.Level,
                e.LoggingEvent.RenderedMessage, e.LoggingEvent.ExceptionObject);
            Action a = () => debugTextBox.AppendText(s + "\n");

            if (InvokeRequired)
                Invoke(a);
            else
                a();
        }
    }
}
