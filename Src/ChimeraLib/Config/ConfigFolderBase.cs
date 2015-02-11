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
		//Creating file crashes config tool on launcher config for some reason
                file = cfg.Get(group, group + ".ini");
                //File.Create(file);
            }

            return file;
        }

        protected ConfigFolderBase(string name, params string[] args) :
            this(name, null, args) {
        }
        protected ConfigFolderBase(string name, string frame, params string[] args) :
            base(GetFile(name, args), frame, args) {
        }
    }
}
