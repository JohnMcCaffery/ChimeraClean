using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using OpenMetaverse.Packets;
using System.Threading;

namespace ConsoleTest {
    class Program {
        static void Main(string[] args) {            InterProxyServer m = new InterProxyServer(8080);
            InterProxyClient s = new InterProxyClient();

            Console.WriteLine("\nConnect Slave");
            Console.ReadLine();
            s.Connect(8080);
            Thread.Sleep(1000);

            Console.WriteLine("\nSend Packet");
            Console.ReadLine();
            Packet p = new DisableSimulatorPacket();
            m.BroadcastPacket(p);
            Thread.Sleep(1000);

            Console.WriteLine("\nDisconnect slave");
            Console.ReadLine();
            s.Disconnect();
            Thread.Sleep(1000);

            Console.WriteLine("\nReconnect slave");
            Console.ReadLine();
            s.Name = "Slave 2";
            s.Connect(8080);
            Thread.Sleep(1000);

            Console.WriteLine("\nStop master");
            Console.ReadLine();
            m.Stop();
            Thread.Sleep(1000);

            Console.WriteLine("\nRestart master");
            Console.ReadLine();
            m.Start();
            Thread.Sleep(1000);

            Console.WriteLine("\nReconnect slave");
            Console.ReadLine();
            s.Name = "Slave 3";
            s.Connect(8080);
            Thread.Sleep(1000);

            Console.WriteLine("\nFinish");
            Console.ReadLine();
            s.Stop();
            Thread.Sleep(1000);

        }
    }
}
