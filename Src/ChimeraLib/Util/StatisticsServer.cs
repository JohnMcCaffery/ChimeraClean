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
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Chimera.Util {
    public class StatisticsServer {
        private Socket mServer;
        private Thread mListenThread;
        private bool mCont;
        private Core mCoordinator;
        private DateTime mStarted;

        public StatisticsServer(Core coordinator) {
            mCoordinator = coordinator;
            mStarted = DateTime.Now;

            return;

            mServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mServer.Bind(new IPEndPoint(IPAddress.Loopback, 80));
            mServer.Listen(1);

            mListenThread = new Thread(Listen);
            mListenThread.Name = "Web Server Listener";
            mListenThread.Start();
        }

        public void Stop() {
            return;
            mCont = false;
            if (mServer.Connected) {
                mServer.Disconnect(false);
                mServer.Shutdown(SocketShutdown.Both);
            }
            mServer.Close();
        }

        private void Listen() {
            mCont = true;
            while (mCont) {
                HandleRequest(mServer.Accept());
            }
        }

        private void HandleRequest(Socket conn) {
            NetworkStream stream = new NetworkStream(conn);
            byte[] buffer = new byte[5000];
            stream.Read(buffer, 0, 5000);

            string str = Encoding.ASCII.GetString(buffer);

            Console.WriteLine("Received: " + str);

            //byte[] outBuff = Encoding.ASCII.GetBytes(mCoordinator.StateManager.Statistics);
            //stream.Write(outBuff, 0, outBuff.Length);
        }
    }
}
