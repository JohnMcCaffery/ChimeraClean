using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using OpenMetaverse.Packets;
using System.Runtime.Remoting;
using GridProxy;
using System.Threading;

namespace UtilLib {
    [XmlRpcUrl("http://diana.apollo.cs.st-andrews.ac.uk")]
    public interface ISlave {
        [XmlRpcMethod]
        string GetName();
        [XmlRpcMethod]
        void ProcessPacket(Packet packet);
        [XmlRpcMethod]
        void Ping();
    }

    public interface ISlaveProxy : ISlave, IXmlRpcProxy { }

    public class XMLRPCSlave : MarshalByRefObject, ISlave {
        private bool pinged = false;
        private string name;

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public bool Pinged {
            get { return pinged; }
        }

        public event PacketDelegate PacketReceived;

        public XMLRPCSlave() {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ISlave), "Slave.rem", WellKnownObjectMode.Singleton);
            RemotingServices.Marshal(this, "Slave.rem");
        }

        #region ISlave Members

        public string GetName() {
            return name;
        }

        public void ProcessPacket(OpenMetaverse.Packets.Packet packet) {
            new Thread(() => { 
                if (PacketReceived != null)
                    PacketReceived(packet, null);
            }).Start();
        }

        public void Ping() {
            pinged = true;
        }

        #endregion

        public void Connect(string masterURI) {
            IMasterProxy master = XmlRpcProxyGen.Create<IMasterProxy>();
            master.Url = masterURI;

            new Thread(() => master.Register("http://localhost:4567/Slave.rem")).Start();
        }
    }
}
