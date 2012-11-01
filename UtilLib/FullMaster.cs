using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilLib {
    public class FullMaster : Master {
        public GridProxy.Proxy SlaveProxy {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }

        protected override OpenMetaverse.Packets.Packet ReceiveIncomingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            throw new NotImplementedException();
        }

        protected override OpenMetaverse.Packets.Packet ReceiveOutgoingPacket(OpenMetaverse.Packets.Packet p, System.Net.IPEndPoint ep) {
            throw new NotImplementedException();
        }
    }
}
