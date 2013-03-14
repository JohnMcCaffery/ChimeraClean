using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using OpenMetaverse;

namespace Chimera.Util {
    public abstract class ConfigBase {
        public string Name;
        protected IConfig generalConfig;
        protected IConfig specificConfig;
        private ArgvConfigSource argConfig;
        private string file;
        private string[] args;

        private bool configLoaded;

        public ConfigBase(params string[] args) {
            argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "File", "f");
            argConfig.AddSwitch("General", "Name", "n");

            IConfigSource config = Init.AddFile(argConfig, out file);
            Name = Init.Get(config.Configs["General"], "Name", "MainWindow");

            argConfig = Init.InitArgConfig(args);

            InitConfig();
        }

        public ConfigBase(string name, string file, string[] args) {
            Name = name;
            this.file = file;
            argConfig = Init.InitArgConfig(args);
            InitConfig();
        }

        protected void LoadConfig() {
            IConfigSource config = Init.AddFile(argConfig, file);
            specificConfig = config.Configs[Name];
            generalConfig = config.Configs["General"];
            configLoaded = true;
        }

        protected void AddKey(bool general, string key, string shortkey) {
            argConfig.AddSwitch(general ? "General" : Name, key, shortkey);
        }

        protected void AddKey(bool general, string key) {
            argConfig.AddSwitch(general ? "General" : Name, key);
        }

        protected Vector3 Get(bool general, string key, Vector3 defalt) {
            if (!configLoaded)
                LoadConfig();
            return Init.GetV(general ? generalConfig : specificConfig, key, defalt);
        }
        protected double Get(bool general, string key, double defalt) {
            if (!configLoaded)
                LoadConfig();
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }
        protected string Get(bool general, string key, string defalt) {
            if (!configLoaded)
                LoadConfig();
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }
        protected float Get(bool general, string key, float defalt) {
            if (!configLoaded)
                LoadConfig();
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }
        protected int Get(bool general, string key, int defalt) {
            if (!configLoaded)
                LoadConfig();
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }

        protected abstract void InitConfig();
    }
}
