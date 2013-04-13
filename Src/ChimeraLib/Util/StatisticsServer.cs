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
        private Coordinator mCoordinator;
        private DateTime mStarted;

        public StatisticsServer(Coordinator coordinator) {
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

            byte[] outBuff = Encoding.ASCII.GetBytes(mCoordinator.StateManager.Statistics);
            stream.Write(outBuff, 0, outBuff.Length);
        }
    }
}
