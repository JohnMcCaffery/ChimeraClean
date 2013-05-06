using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.IO;

namespace Chimera.Config {
    public abstract class ConfigFolderBase : ConfigBase {
        private static string GetFile(string group, string[] args) {
            IConfigSource source = GetMainConfig(args);
            string folder = source.Configs["Config"].Get("ConfigFolder", "../Config");
            string file = source.Configs["Config"].Get(group, group + ".ini");
            return Path.GetFullPath(Path.Combine(folder, file));
        }
        protected ConfigFolderBase(string group, params string[] args) :
            this(group, group, args) {
        }
        protected ConfigFolderBase(string section, string group, params string[] args) :
            base (section, GetFile(group, args), args) {
        }
    }
}
