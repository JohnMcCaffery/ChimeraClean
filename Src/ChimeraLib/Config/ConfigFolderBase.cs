using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.IO;

namespace Chimera.Config {
    public abstract class ConfigFolderBase : ConfigBase {
        private const string DEFAULT_FOLDER = "../Config";

        private static string GetFile(string group, string[] args) {
            IConfigSource source = GetMainConfig(args);
            IConfig cfg = source.Configs["Config"];
            if (cfg == null)
                return Path.GetFullPath("../Config");

            string folder = cfg.Get("ConfigFolder", DEFAULT_FOLDER);
            string file = cfg.Get(group, group + ".ini");
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
