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
using UtilLib;
using OpenMetaverse.Packets;
using System.Threading;
using OpenMetaverse;
using System.Windows.Forms;
using GridProxy;
using log4net;

namespace ConsoleTest {
    class Program {
        private static void Run(string msg, Action action) {
            Console.WriteLine("\n" + msg);
            Console.ReadKey();
            action();
            Thread.Sleep(1000);
        }

        static void Main(string[] args) {

            string portArg = "--proxy-login-port=" + 8080;
            string listenIPArg = "--proxy-proxyAddress-facing-address=127.0.0.1";
            string loginURIArg = "--proxy-remote-login-uri=http://apollo.cs.st-andrews.ac.uk:8002";
            string[] args1 = args.Concat(new string[]{ portArg, listenIPArg, loginURIArg }).ToArray();
            ProxyConfig config1 = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args1);
 
            portArg = "--proxy-login-port=" + 8082;
            listenIPArg = "--proxy-proxyAddress-facing-address=127.0.0.1";
            loginURIArg = "--proxy-remote-login-uri=http://localhost:9000";
            string[] args2 = args.Concat(new string[]{ portArg, listenIPArg, loginURIArg }).ToArray();

            ProxyConfig config2 = new ProxyConfig("Routing Project", "jm726@st-andrews.ac.uk", args2);           
            
            Proxy p1 = new Proxy(config1);
            //Proxy p2 = new Proxy(config2);

            p1.Start();
            //p2.Start();

            Run("Stop", () => {
                p1.Stop();
                //p2.Stop();
            });

            /*
            Init.Config cfg1 = new Init.Config();
            cfg1.ProxyLoginURI = "http://localhost:9000";
            cfg1.ProxyPort = 8080;
            cfg1.ViewerExecutable = "C:\\Program Files (x86)\\Firestorm-release\\firestorm-release.exe";
            cfg1.LoginPassword = "1245";
            cfg1.LoginFirstName = "Routing";
            cfg1.LoginLastName = "God";
            cfg1.LoginGrid = "8080";

            Init.Config cfg2 = new Init.Config();
            cfg2.ProxyLoginURI = "http://localhost:9000";
            cfg2.ProxyPort = 8081;
            cfg2.ViewerExecutable = "C:\\Program Files (x86)\\Firestorm-release\\firestorm-release.exe";
            cfg2.LoginPassword = "1245";
            cfg2.LoginFirstName = "Routing";
            cfg2.LoginLastName = "Project";
            cfg2.LoginGrid = "8081";

            //ProxyManager p1 = new ProxyManager(cfg1, LogManager.GetLogger("Proxy1"));
            //ProxyManager p2 = new ProxyManager(cfg2, LogManager.GetLogger("Proxy2"));


            //Init.StartGui(() => new ProxyForm(p1));
            //Init.StartGui(() => new ProxyForm(p2));


            Run("Stop", () => {
                p1.StopProxy();
                p2.StopProxy();
            });
            */

            /*
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
