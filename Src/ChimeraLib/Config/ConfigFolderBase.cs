using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.IO;

namespace Chimera.Config {
    public abstract class ConfigFolderBase : ConfigBase {
        private static string GetFile(string group, string defaultFile, string[] args) {
            IConfigSource source = GetMainConfig(args);
            string folder = source.Configs["Config"].Get("ConfigFolder", "../Config");
            string file = source.Configs["Config"].Get(group, defaultFile);
            return Path.GetFullPath(Path.Combine(folder, file));
        }
        protected ConfigFolderBase(string group, params string[] args) :
            base(group, GetFile(group, group + ".ini", args), args) {
        }
    }
}
