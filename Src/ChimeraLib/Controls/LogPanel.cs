/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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
            if (!e.LoggingEvent.LoggerName.Equals("OpenMetaverse") && source != null && !source.Equals(e.LoggingEvent.LoggerName))
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
