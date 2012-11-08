using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilLib {
    public static class Init {
        public static CameraMaster InitCameraMaster(string args) {
            bool autostartProxy;
            bool autostartMaster;
            bool autostartClient;
            string clientExe;
            string clientFirstName;
            string clientLastName;
            string clientPassword;
            int proxyPort;
            int masterPort;
            string slavesFile;
            CameraMaster m = new CameraMaster();
            return m;
        }

        public static CameraSlave InitCameraSlave(string args) {
            bool autostartProxy;
            bool autoConnect;
            bool autostartClient;
            string clientExe;
            string clientFirstName;
            string clientLastName;
            string clientPassword;
            string name;
            int masterPort;
            string masterAddress;
            string configFile;
            CameraSlave s = new CameraSlave();
            return s;
        }
    }
}
