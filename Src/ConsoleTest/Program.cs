using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using OpenMetaverse.Packets;
using System.Threading;
using OpenMetaverse;
using System.Windows.Forms;

namespace ConsoleTest {
    class Program {
        private static void Run(string msg, Action action) {
            Console.WriteLine("\n" + msg);
            Console.ReadKey();
            action();
            Thread.Sleep(1000);
        }

        static void Main(string[] args) {
            CameraMaster m = Init.InitCameraMaster(args, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            CameraSlave s1 = Init.InitCameraSlave(args, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, "Slave1");
            CameraSlave s2 = Init.InitCameraSlave(args, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, "Slave2");

            Run("Start Master GUI", () => {
                Init.StartGui(() => new MasterForm(m));
            });
            Run("Start Slave1 GUI", () => {
                Init.StartGui(() => new SlaveForm(s1));
            });
            Run("Start Slave2 GUI", () => {
                Init.StartGui(() => new SlaveForm(s2));
            });

            /*
            int masterPort = 8090;
            int masterProxyPort = 8080;
            int slave1ProxyPort = 8081;
            int slave2ProxyPort = 8082;
            CameraMaster m = new CameraMaster();
            CameraSlave s1 = new CameraSlave();
            CameraSlave s2 = new CameraSlave();

            m.StartMaster(masterPort);
            s1.Connect(masterPort);
            s2.Connect(masterPort);
            StartGui(() => new MasterForm(m));
            StartGui(() => new SlaveForm(s1));
            StartGui(() => new SlaveForm(s2));
            //Run("Bind Master", () => m.StartMaster(masterPort));
            //Run("Bind Slave Proxy", () => s.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", slaveProxyPort));
            //Run("Bind Proxy", () => m.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", masterProxyPort));
            //Run("Bind Master GUI", () => {
            //});
            //Run("Bind Master GUI", () => StartGui(() => new MasterForm(m)));
            //Run("Bind Slave 1 GUI", () => StartGui(() => new SlaveForm(s1)));
            //Run("Bind Slave 2 GUI", () => StartGui(() => new SlaveForm(s2)));
            Run("Start Proxys", () => {
                s1.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", slave1ProxyPort);
                s2.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", slave2ProxyPort);
                //m.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", masterProxyPort);
            });
            //Run("Connect Slave", () => s.Connect(masterPort));
            //Run("Change Yaw", () => m.Rotation.Yaw += 45);
            //Run("Change Pitch", () => m.Rotation.Pitch += 45);
            //Run("Change Vector", () => m.Rotation.LookAtVector = new Vector3(1f, 1f, 1f));
            //Run("Change MasterRotation", () => m.Rotation.Quaternion = Quaternion.CreateFromEulers(1f, 1f, 1f));
            Run("Disconnect Master", () => m.Stop());
            Run("Disconnect Slave 1", () => s1.Stop());
            Run("Disconnect Slave 2", () => s2.Stop());
            Run("Stop All", () => {
                s1.Stop();
                s2.Stop();
                m.Stop();
            });

            /*
            InterProxyServer m = new InterProxyServer(masterPort);
            InterProxyClient s = new InterProxyClient();

            Run("Connect Slave", () => s.Connect(masterPort));
            Run("Send Packet", () => {
                Packet p = new DisableSimulatorPacket();
                m.BroadcastPacket(p);
            });
            Run("Disconnect slave", () => s.Disconnect());
            Run("Reconnect slave", () => {
                s.Name = "Slave 2";
                s.Connect(masterPort);
            });
            Run("Disconnect master", () => m.Stop());
            Run("Restart master", () => m.Start());
            Run("Reconnect slave", () => {
                s.Name = "Slave 3";
                s.Connect(masterPort);
            });
            Run("Close Master", () => m.Stop());
            Run("Close Slave", () => s.Disconnect());
            */

            Run("Exit", () => { });
        }
    }
}
