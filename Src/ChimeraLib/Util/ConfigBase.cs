using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using OpenMetaverse;

namespace Chimera.Util {
    public abstract class ConfigBase {
        private class ConfigParam : IComparable<ConfigParam> {
            private string mKey;
            private string mDescription;
            private string mShortKey;
            private bool mCommandLine;
            private bool mGeneral;
            private string mType;
            private string mDefault;
            private readonly List<string> mGroups = new List<string>();

            public override string ToString() {
                return base.ToString();
            }

            public ConfigParam(string key, string description, string type, bool general, string group, string defalt, bool commandLine, string shortKey) {
                mKey = key;
                mDescription = description;
                mType = type;
                mGeneral = general;
                mCommandLine = false;
                mGroups.Add(group);
                mShortKey = null;
                mDefault = defalt;
                mShortKey = shortKey;
                mCommandLine = commandLine;
            }

            public bool CommandLine {
                get { return mCommandLine; }
                set { mCommandLine = value; }
            }
            public string ShortKey {
                get { return mShortKey; }
                set { mShortKey = value; }
            }
            public bool Shared {
                get { return mGroups.Count > 0; }
            }

            public override int GetHashCode() {
                return Shared ? 0 : 1;
            }

            public void AddGroup(string group) {
                mGroups.Add(group);
            }

            public int CompareTo(ConfigParam other) {
                if (Shared && other.Shared)
                    return mKey.CompareTo(other.mKey);
                else if (Shared)
                    return -1;
                else if (other.Shared)
                    return 1;
                else
                    return mGroups[0].CompareTo(other.mGroups[0]);
            }
        }

        private void AddParam(string key, string description, string type, bool general, string defalt) {
            bool commandLine = general ? generalCommandLineKeys.Contains(key) : specificCommandLineKeys.Contains(key);
            string shortKey = general ? 
                (generalCommandLineShortKeys.ContainsKey(key) ? generalCommandLineShortKeys[key] : null) :
                (specificCommandLineShortKeys.ContainsKey(key) ? specificCommandLineShortKeys[key]: null);

            if (!_parameters.ContainsKey(key))
                _parameters.Add(key, new ConfigParam(key, description, type, general, Group, defalt, commandLine, shortKey));
            else {
                ConfigParam param = _parameters[key];
                param.AddGroup(Group);
                if (!param.CommandLine && commandLine)
                    param.CommandLine = true;
                if (param.ShortKey == null && shortKey != null)
                    param.ShortKey = shortKey;
            }
        }

        public string Name;
        protected IConfig generalConfig;
        protected IConfig specificConfig;
        private ArgvConfigSource argConfig;
        private string file;

        private bool configLoaded;

        private readonly static HashSet<string> specificCommandLineKeys = new HashSet<string>();
        private readonly static HashSet<string> generalCommandLineKeys = new HashSet<string>();
        private readonly static Dictionary<string, string> specificCommandLineShortKeys = new Dictionary<string, string>();
        private readonly static Dictionary<string, string> generalCommandLineShortKeys = new Dictionary<string, string>();
        private readonly static Dictionary<string, ConfigParam> _parameters = new Dictionary<string,ConfigParam>();

        /// <summary>
        /// The name of the type of configuration this is.
        /// </summary>
        public abstract string Group {
            get;
        }

        public ConfigBase(params string[] args) {
            argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "File", "f");
            argConfig.AddSwitch("General", "Name", "n");

            IConfigSource config = Init.AddFile(argConfig, out file);
            Name = Init.Get(config.Configs["General"], "Name", "MainWindow");

            argConfig = Init.InitArgConfig(args);

            InitConfig();
        }

        public ConfigBase(string name, string[] args) {
            Name = name;
            argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "File", "f");

            IConfigSource config = Init.AddFile(argConfig, out file);
            InitConfig();
        }

        public ConfigBase(string name, string file, string[] args) {
            Name = name;
            this.file = file;
            argConfig = Init.InitArgConfig(args);
            InitConfig();
        }

        private void LoadConfig() {
            IConfigSource config = Init.AddFile(argConfig, file);
            specificConfig = config.Configs[Name];
            generalConfig = config.Configs["General"];
            configLoaded = true;
        }

        /// <summary>
        /// Add a key to the list of command line arguments that will be interpreted.
        /// This will have no effect if one of the Get methods has already been called.
        /// </summary>
        /// <param name="general">Whether to add it to the general config or Name.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="shortkey">The short version of the key.</param>
        protected void AddCommandLineKey(bool general, string key, string shortkey) {
            argConfig.AddSwitch(general ? "General" : Name, key, shortkey);
            if (general) {
                generalCommandLineKeys.Add(key);
                if (!generalCommandLineShortKeys.ContainsKey(key))
                    generalCommandLineShortKeys.Add(key, shortkey);
            } else {
                specificCommandLineKeys.Add(key);
                if (!specificCommandLineShortKeys.ContainsKey(key))
                    specificCommandLineShortKeys.Add(key, shortkey);
            }
        }

        /// <summary>
        /// Add a key to the list of command line arguments that will be interpreted.
        /// This will have no effect if one of the Get methods has already been called.
        /// </summary>
        /// <param name="general">Whether to add it to the general config or Name.</param>
        /// <param name="key">The key to add.</param>
        protected void AddCommandLineKey(bool general, string key) {
            argConfig.AddSwitch(general ? "General" : Name, key);
            if (general) generalCommandLineKeys.Add(key);
            else specificCommandLineKeys.Add(key);
        }

        protected Vector3 GetV(bool general, string key, Vector3 defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            AddParam(key, description, "Vector3", general, defalt.ToString());
            return Init.GetV(general ? generalConfig : specificConfig, key, defalt);
        }
        protected double Get(bool general, string key, double defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            AddParam(key, description, "double", general, defalt.ToString());
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }
        protected string Get(bool general, string key, string defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            AddParam(key, description, "string", general, defalt);
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }
        protected float Get(bool general, string key, float defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            AddParam(key, description, "float", general, defalt.ToString());
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }
        protected int Get(bool general, string key, int defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            AddParam(key, description, "int", general, defalt.ToString());
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }
        protected bool Get(bool general, string key, bool defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            AddParam(key, description, "bool", general, defalt.ToString());
            return Init.Get(general ? generalConfig : specificConfig, key, defalt);
        }

        protected abstract void InitConfig();
    }
}
