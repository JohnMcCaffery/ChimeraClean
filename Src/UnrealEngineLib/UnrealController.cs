using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using UnrealEngineLib.GUI;
using Chimera.Util;
using System.Net.Sockets;
using System.Net;
using log4net;
using Chimera.Interfaces;
using System.IO;
using System.Drawing;

namespace UnrealEngineLib {
    public class UnrealControllerFactor : IOutputFactory {
        public IOutput Create() {
            return new UnrealController();
        }
    }

    public class UnrealController : IOutput {
        private const int BUFLEN = 2048;

        private readonly ILog ThisLogger = LogManager.GetLogger("OpenSim");

        private Frame mFrame;
        private UnrealControlPanel mControlPanel;
        private UnrealConfig mConfig;
        private bool mAutoRestart;
        private ProcessController mProcess;

        private TcpListener mServer;
        private NetworkStream mUnrealCommunicator;
        private byte[] mData = new Byte[BUFLEN];
        private byte[] mStreamBuffer = new byte[BUFLEN];

        public event Action<string> StringReceived;

        public UnrealConfig Config { get { return mConfig; } }

        public Frame Frame {
            get { return mFrame; }
        }

        public System.Windows.Forms.Control ControlPanel {
            get { 
                if (mControlPanel == null)
                    mControlPanel = new UnrealControlPanel(this);
                return mControlPanel;
            }
        }

        public bool AutoRestart {
            get { return mConfig.AutoRestartUnreal; }
            set { mConfig.AutoRestartUnreal = value; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public string Type {
            get { return "Unreal"; }
        }

        public bool Active {
            get { throw new NotImplementedException(); }
        }

        public System.Diagnostics.Process Process {
            get { return mProcess == null ? null : mProcess.Process; }
        }

        public void Init(Frame frame) {
            mFrame = frame;
            mConfig = new UnrealConfig(frame.Name);

            if (mConfig.AutoStartUnreal)
                Launch();
            else if (mConfig.AutoStartServer)
                StartServer();
        }

        public bool Launch() {
            StartServer();
            string windowPosition = "";


            if (mConfig.Fill != Chimera.Fill.Windowed) {
                windowPosition += " -WinX=" + mFrame.Monitor.Bounds.X;
                windowPosition += " -WinY=" + mFrame.Monitor.Bounds.Y;
                if (mConfig.Fill == Chimera.Fill.Full) {
                    windowPosition += " -ResX=" + mFrame.Monitor.Bounds.Width;
                    windowPosition += " -ResY=" + mFrame.Monitor.Bounds.Height;
                } else if (mConfig.Fill == Chimera.Fill.Left || mConfig.Fill == Chimera.Fill.Right) {
                    Rectangle position = mFrame.Monitor.Bounds;
                    position.Width /= 2;
                    if (mConfig.Fill == Chimera.Fill.Right)
                        position.X += position.Width;
                    windowPosition += " -ResX=" + position.Width;
                    windowPosition += " -ResY=" + position.Height;
                }
            }


            mProcess = new ProcessController(mConfig.UnrealExecutable, mConfig.UnrealWorkingDirectory, mConfig.UnrealArguments + windowPosition);
            mProcess.Start();

            return false;
        }

        public void Close() {
            if (mServer != null) {
                SendString(mConfig.ShutdownStr);
                SendString("~console exit");
            }
        }

        public void Restart(string reason) {
            throw new NotImplementedException();
        }

        public Fill Fill {
            get { return mConfig.Fill; }
            set {
                mConfig.Fill = value;

                if (value == Chimera.Fill.Full) {
                    //mProcess.Monitor = mFrame.Monitor;
                    //SendString("~Position " + mFrame.Monitor.Bounds.X + "," + mFrame.Monitor.Bounds.Y);
                    //SendString("~console r.setRes " + mFrame.Monitor.Bounds.Width + "x" + mFrame.Monitor.Bounds.Height);
                    SendString("~console fullscreen");
                } else if (value == Chimera.Fill.Left || value == Chimera.Fill.Right) {
                    Rectangle position = mFrame.Monitor.Bounds;
                    position.Width /= 2;
                    if (value == Chimera.Fill.Right)
                        position.X += position.Width;
                    //SendString("~Position " + position.X + "," + position.Y);
                    //SendString("~console r.setRes " + position.Width + "x" + position.Height);
                    //mProcess.Position = position;
                } else {
                }

		/*
                mProcess.FullScreen = value == Fill.Full;
                if (value == Fill.Left)
                    mProcess.Split(ProcessController.Side.Left);
                else if (value == Fill.Right)
                    mProcess.Split(ProcessController.Side.Right);
		*/
            }
        }

        public class TcpState {
            public IPEndPoint e;
            public UdpClient u;
        }

        public void SendString(string str) {
            if (mUnrealCommunicator == null) {
                ThisLogger.Warn("Cannot send message to unreal, communicator is not yet established: " + str);
                return;
            }
            byte[] send_buffer = Encoding.ASCII.GetBytes(str + '\0');

            try {
                mUnrealCommunicator.Write(send_buffer, 0, send_buffer.Length);
                ThisLogger.Info("Sent string to unreal: " + str);
            } catch (Exception e) {
                ThisLogger.Warn("Problem sending string.", e);
            }
        }

        private void WaitForMessages() {
            mUnrealCommunicator.BeginRead(mStreamBuffer, 0, mStreamBuffer.Length, MessageReceived, mUnrealCommunicator);
        }

        private void MessageReceived(IAsyncResult ar) {
            try {
                NetworkStream stream = (NetworkStream)ar.AsyncState;
                int bytesReceived = stream.EndRead(ar);
                string receiveString = Encoding.ASCII.GetString(mStreamBuffer, 0, bytesReceived);

                // message received may be larger than buffer size so loop through until you have it all. 
                while (stream.DataAvailable) {
                    stream.Read(mStreamBuffer, 0, mStreamBuffer.Length);
                    receiveString += Encoding.ASCII.GetString(mStreamBuffer, 0, bytesReceived);
                }


                if (receiveString == mConfig.UnrealInitialisedStr) {
                    UnrealStarted();
                } else if (receiveString == mConfig.UnrealShutdownStr) {
                    ThisLogger.Warn("Unreal shut down, waiting for a new connection.");
                    WaitForConnections();
                } else {
                    WaitForMessages();
                    ThisLogger.Info(string.Format("Received: {0}", receiveString));
                    if (StringReceived != null) {
                        StringReceived(receiveString);
                    }
                }
            } catch (ObjectDisposedException ex) { } 
            catch (IOException ex) {
                if (ex.Message.Contains("forcibly closed"))
                    ThisLogger.Warn("Unreal stream closed.");
                else
                    ThisLogger.Warn("Read failed", ex);
            }
        }

        private void UnrealStarted() {
            ThisLogger.Warn("Unreal launched and connected.");
            SendString(mConfig.UnrealInitialisedAck);
            WaitForMessages();
	    //if (mProcess != null)
            Fill = mConfig.Fill;
        }

        private void WaitForConnections() {
            try {
                mServer.Start();
                mServer.BeginAcceptTcpClient(new AsyncCallback(ReceiveClientConnection), mUnrealCommunicator);
                ThisLogger.Info("Listening for connections on " + mConfig.ListenPort);
            } catch (ObjectDisposedException ex) {
                ThisLogger.Fatal("Server is already disposed so cannot wait for connections.");
            }
        }

        private void ReceiveClientConnection(IAsyncResult ar) {
            TcpClient c = mServer.EndAcceptTcpClient(ar);
            ThisLogger.Info(string.Format("Received connection"));
            mUnrealCommunicator = c.GetStream();

            WaitForMessages();
        }

        public void StartServer() {
            if (mServer == null) {
                mServer = new TcpListener(IPAddress.Loopback, mConfig.ListenPort);
                WaitForConnections();
                //SendString("Init message");
            }
        }

    }
}
