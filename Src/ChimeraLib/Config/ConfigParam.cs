using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Ini;
using Nini.Config;
using System.IO;

namespace Chimera.Config {
    public class ConfigParam : IComparable<ConfigParam> {
        private string mKey;
        private string mDescription;
        private string mShortKey;
        private bool mCommandLine;
        private string mSection;
        private ParameterTypes mType;
        private string mDefault;
        private string mValue;
        private string[] mValues;
        private string mFile;
        private readonly List<string> mGroups = new List<string>();

        public string Key {
            get { return mKey; }
        }

        public string Description {
            get { return mDescription; }
        }

        public string Default {
            get { return mDefault; }
        }

        public string Section {
            get { return mSection; }
        }

        public string[] Values {
            get { return mValues; }
        }

        public string Value {
            get { return mValue; }
            set {
                mValue = value;

                IniDocument doc = new IniDocument(mFile, IniFileType.WindowsStyle);
                IniConfigSource source = new IniConfigSource(doc);
                //TODO - make this write out!
                IConfig cfg = source.Configs[mSection];
                if (cfg == null)
                    cfg = source.Configs.Add(mSection);

                cfg.Set(mKey, value);

                using (var writer = File.CreateText(mFile)) {
                    source.Save(writer);
                }
            }
        }

        public override string ToString() {
            return mValue;
        }
        public ParameterTypes Type {
            get { return mType; }
        }

        public ConfigParam(string key, string description, ParameterTypes type, string section, string group, string defalt, bool commandLine, string shortKey, string value, string file, params string[] values) {
            mKey = key;
            mDescription = description;
            mType = type;
            mSection = section;
            mCommandLine = false;
            mGroups.Add(group);
            mShortKey = null;
            mDefault = defalt;
            mShortKey = shortKey;
            mCommandLine = commandLine;
            mFile = file;
            mValues = values;
            mValue = value;
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
}
