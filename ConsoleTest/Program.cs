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
            Console.ReadLine();
            action();
            Thread.Sleep(1000);

        }

        static void Main(string[] args) {
            int masterPort = 8090;
            int masterProxyPort = 8080;
            int slaveProxyPort = 8081;
            CameraMaster m = new CameraMaster();
            //InterProxyClient s = new InterProxyClient();
            CameraSlave s = new CameraSlave();

            Run("Start Master", () => m.StartMaster(masterPort));
            Run("Start Slave Proxy", () => s.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", slaveProxyPort));
            Run("Start Slave GUI", () => {
                Thread t = new Thread(() => {
                    SlaveForm f = new SlaveForm(s);
                    f.ShowDialog();
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                //Application.EnableVisualStyles();
                //Application.Run(new SlaveForm(s));
            });


            Run("Connect Slave", () => s.Connect(masterPort));
            Run("Change Yaw", () => m.Rotation.Yaw += 5);
            Run("Change Pitch", () => m.Rotation.Pitch += 5);
            Run("Change Vector", () => m.Rotation.LookAtVector = new Vector3(1f, 1f, 1f));
            Run("Change Rotation", () => m.Rotation.Rot = Quaternion.CreateFromEulers(1f, 1f, 1f));
            Run("Start Proxy", () => m.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", masterProxyPort));
            Run("Stop Master", () => m.Stop());
            Run("Stop Slave", () => s.Stop());
            /*
            InterProxyServer m = new InterProxyServer(8080);
            InterProxyClient s = new InterProxyClient();

            Run("Connect Slave", () => s.Connect(8080));
            Run("Send Packet", () => {
                Packet p = new DisableSimulatorPacket();
                m.BroadcastPacket(p);
            });
            Run("Disconnect slave", () => s.Disconnect());
            Run("Reconnect slave", () => {
                s.Name = "Slave 2";
                s.Connect(8080);
            });
            Run("Stop master", () => m.Stop());
            Run("Restart master", () => m.Start());
            Run("Reconnect slave", () => {
                s.Name = "Slave 3";
                s.Connect(8080);
            });
            Run("Finish", () => s.Stop());
            */
        }
    }
}
