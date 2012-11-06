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
            int masterPort = 8090;
            int masterProxyPort = 8080;
            int slaveProxyPort = 8081;
            CameraMaster m = new CameraMaster();
            CameraSlave s = new CameraSlave();

            Run("Bind Master", () => m.StartMaster(masterPort));
            //Run("Bind Slave Proxy", () => s.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", slaveProxyPort));
            Run("Bind Master GUI", () => {
                Thread t = new Thread(() => {
                    MasterForm f = new MasterForm(m);
                    f.ShowDialog();
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                //Application.EnableVisualStyles();
                //Application.Run(new SlaveForm(s));
            });
            Run("Bind Slave GUI", () => {
                Thread t = new Thread(() => {
                    SlaveForm f = new SlaveForm(s);
                    f.ShowDialog();
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                //Application.EnableVisualStyles();
                //Application.Run(new SlaveForm(s));
            });
            //Run("Connect Slave", () => s.Connect(masterPort));
            //Run("Change Yaw", () => m.Rotation.Yaw += 45);
            //Run("Change Pitch", () => m.Rotation.Pitch += 45);
            //Run("Change Vector", () => m.Rotation.LookAtVector = new Vector3(1f, 1f, 1f));
            //Run("Change MasterRotation", () => m.Rotation.Quaternion = Quaternion.CreateFromEulers(1f, 1f, 1f));
            //Run("Bind Proxy", () => m.StartProxy("http://apollo.cs.st-andrews.ac.uk:8002", masterProxyPort));
            Run("Disconnect Master", () => m.Stop());
            Run("Disconnect Slave", () => s.Stop());

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


            Console.ReadKey();
        }
    }
}
