using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using System.Runtime.Remoting;
using System.Threading;
using OpenMetaverse.Packets;

namespace UtilLib {
    [XmlRpcUrl("http://diana.apollo.cs.st-andrews.ac.uk")]
    public interface IMaster {
        [XmlRpcMethod]
        void Register(string uri);
    }

    public interface IMasterProxy : IMaster, IXmlRpcProxy { }

    public class XMLRPCMaster : MarshalByRefObject, IMaster {
        private List<ISlave> slaves = new List<ISlave>();

        public event EventHandler SlaveConnected;

        public XMLRPCMaster() {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(IMaster), "Master.rem", WellKnownObjectMode.Singleton);
            RemotingServices.Marshal(this, "Master.rem");
        }

        #region IMaster Members

        public void Register(string uri) {
            ISlaveProxy slave = XmlRpcProxyGen.Create<ISlaveProxy>();
            slave.Url = uri;
            slaves.Add(slave);

            new Thread(() => {
                slave.Ping();
                string name = slave.GetName();

                if (SlaveConnected != null)
                    SlaveConnected(name, null);
            }).Start();
        }

        #endregion

        public void BroadcastPacket(Packet packet) {
            new Thread(() => {
                lock (slaves)
                    foreach (var slave in slaves)
                        slave.ProcessPacket(packet);
            }).Start();
        }
    }
}
