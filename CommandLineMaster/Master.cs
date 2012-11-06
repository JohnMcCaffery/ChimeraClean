using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using OpenMetaverse.Packets;

namespace CommandLineMaster {
    class Master {
        static void Main(string[] args) {
            InterProxyServer m = new InterProxyServer(8080);
            m.OnSlaveConnected += (name, source) => Console.WriteLine(name + " connected from " + source + ".");
            m.OnSlaveDisconnected += (name) => Console.WriteLine(name + " disconnected.");

            Console.ReadLine();

            Packet p = new DisableSimulatorPacket();
            m.BroadcastPacket(p);
            Console.WriteLine("Master sent DisableSimulator packet to all slaves.");

            Console.ReadLine();
            m.Stop();
            Console.ReadLine();
        }
    }
}
