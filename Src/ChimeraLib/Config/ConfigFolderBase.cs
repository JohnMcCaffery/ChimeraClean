using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.IO;

namespace Chimera.Config {
    public abstract class ConfigFolderBase : ConfigBase {
        private const string DEFAULT_FOLDER = "../Configs/Config";

        private static string GetFile(string group, string[] args) {
            IConfigSource source = GetMainConfig(args);
            IConfig cfg = source.Configs["Config"];
            if (cfg == null)
                return Path.GetFullPath("../Config");

            string folder = cfg.Get("ConfigFolder", DEFAULT_FOLDER);
            string file = cfg.Get(group, group + ".ini");
            file = Path.GetFullPath(Path.Combine(folder, file));

            if (!File.Exists(file)) {
                file = cfg.Get(group, group + ".ini");
            }

            return file;
        }

        protected ConfigFolderBase(string group, params string[] args) :
            this(group, null, args) {
        }
        protected ConfigFolderBase(string group, string frame, params string[] args) :
            base(GetFile(group, args), frame, args) {
        }
    }
}
