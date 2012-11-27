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
        private string source = null;
        private readonly Queue<string> backlog = new Queue<string>();

        public string Source {
            get { return source; }
            set { source = value; }
        }
        
        public LogPanel() {
            //Only start processing events when a handle has been created.
            HandleCreated += (source, args) => {
                lock (backlog) {
                    created = true;
                    while (backlog.Count > 0)
                        debugTextBox.AppendText(backlog.Dequeue());
                }
            };
            InitializeComponent();

            log4net.Config.XmlConfigurator.Configure();
            Logger.Log("Log Panel Ready", Helpers.LogLevel.Info);
            if (FireEventAppender.Instance != null) 
                FireEventAppender.Instance.MessageLoggedEvent += new MessageLoggedEventHandler(Instance_MessageLoggedEvent);
        }

        protected override void DestroyHandle() {
            base.DestroyHandle();
            lock (createdLock)
                Monitor.PulseAll(createdLock);
        }

        void Instance_MessageLoggedEvent(object sender, MessageLoggedEventArgs e) {
            if (source != null && !source.Equals(e.LoggingEvent.LoggerName))
                return;
            //if (!created)
                //lock (createdLock)
                    //Monitor.Wait(createdLock);

            if (this.IsDisposed || this.Disposing)
                return;

            string s = String.Format("{0} [{1}] {2} {3}\n", e.LoggingEvent.TimeStamp, e.LoggingEvent.Level,
                e.LoggingEvent.RenderedMessage, e.LoggingEvent.ExceptionObject);

            lock (backlog) {
                if (!created) {
                    backlog.Enqueue(s);
                    return;
                }
            }
            Action a = () => debugTextBox.AppendText(s);

            if (InvokeRequired || !IsHandleCreated)
                BeginInvoke(a);
            else
                a();
        }
    }
}
