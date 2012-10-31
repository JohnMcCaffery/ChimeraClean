using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using OpenMetaverse.Packets;

namespace CommandLineSlave {
    class Slave {
        static void Main(string[] args) {
            InterProxyClient s = new InterProxyClient();
            s.OnPacketReceived += (p, ep) => { 
                Console.WriteLine("Recevied " + p.Type + " from master."); 
                return null;
            };
            s.OnConnected += (source, arg) => Console.WriteLine("Connected to master.");
            s.OnDisconnected += (source, arg) => Console.WriteLine("Disconnected from master.");
            s.Connect(8080);
            Console.ReadLine();
            s.Stop();
        }
    }
}
