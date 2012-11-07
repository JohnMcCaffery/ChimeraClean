using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilLib {
    public class FullSlave : ProxyManager {
        protected override OpenMetaverse.Packets.Packet ReceiveOutgoingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            throw new NotImplementedException();
        }

        protected override OpenMetaverse.Packets.Packet ReceiveIncomingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            throw new NotImplementedException();
        }
    }
}
