/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using OpenMetaverse;
using Chimera.Util;
using System.IO;

namespace Chimera.Config {
    public enum ParameterTypes {
        Bool,
        Float,
        Double,
        Int,
        String,
        Enum,
        Vector3
    }

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
                    //TODO - make this write out!
                }
            }

            public override string ToString() {
                return base.ToString();
            }
            public ParameterTypes Type {
                get { return mType; }
            }

            public ConfigParam(string key, string description, ParameterTypes type, string section, string group, string defalt, bool commandLine, string shortKey, string value, params string[] values) {
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
    public abstract class ConfigBase {
        public string Section;
        private IConfigSource mSource;
        private ArgvConfigSource mArgConfig;
        private string mFile;

        private bool configLoaded;

        private readonly Dictionary<string, HashSet<string>> commandLineKeys = new Dictionary<string, HashSet<string>>();
        private readonly Dictionary<string, Dictionary<string, string>> commandLineShortKeys = new Dictionary<string, Dictionary<string, string>>();
        private readonly Dictionary<string, ConfigParam> _mParameters = new Dictionary<string,ConfigParam>();


        public IEnumerable<ConfigParam> Parameters {
            get { return _mParameters.Values; }
        }

        private void AddParam(string key, string description, ParameterTypes type, string section, string defalt, object value, params string[] values) {
            bool commandLine = commandLineKeys.ContainsKey(section) && commandLineKeys[section].Contains(key);
            string shortKey = commandLine && commandLineShortKeys.ContainsKey(section) ? commandLineShortKeys[section][key] : null;

            if (!_mParameters.ContainsKey(key))
                _mParameters.Add(key, new ConfigParam(key, description, type, section, Group, defalt, commandLine, shortKey, value != null ? value.ToString() : "null", values));
            else {
                ConfigParam param = _mParameters[key];
                param.AddGroup(Group);
                if (!param.CommandLine && commandLine)
                    param.CommandLine = true;
                if (param.ShortKey == null && shortKey != null)
                    param.ShortKey = shortKey;
            }
        }

        private void AddParam(string key, string description, string p, string general, string p_2, Type type) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get the main config file used across the whole application.
        /// </summary>
        /// <param name="args">Any command line arguments passed to the application.</param>
        /// <returns>A config source for the main configuration common across the whole application.</returns>
        public static IConfigSource GetMainConfig(string[] args) {
            ArgvConfigSource argConfig;
            string file;
            return GetMainConfig(args, out argConfig, out file);
        }

        protected static IConfigSource GetMainConfig(string[] args, out ArgvConfigSource argConfig, out string file) {
            argConfig = Init.InitArgConfig(args);
            argConfig.AddSwitch("General", "MainConfigFile", "f");
            file = argConfig.Configs["General"].Get("MainConfigFile", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            return Init.AddFile(argConfig, file);
        }

        /// <summary>
        /// Get the path for a configuration file.
        /// </summary>
        /// <param name="args">Any command line arguments passed to the application.</param>
        /// <param name="fileKey">The key to search for the file under.</param>
        /// <param name="defaultFile">The default name for the file.</param>
        /// <returns>The path of a config file found by looking up file key in the application wide configuration.</returns>
        protected static string GetFile(string[] args, string fileKey, string defaultFile) {
            IConfigSource s;
            return GetFile(args, fileKey, defaultFile, out s);
        }
        /// <summary>
        /// Get the path for a configuration file.
        /// </summary>
        /// <param name="args">Any command line arguments passed to the application.</param>
        /// <param name="fileKey">The key to search for the file under.</param>
        /// <param name="defaultFile">The default name for the file.</param>
        /// <param name="source">The configuration source, made by combining the application wide config with the command line argument based config, which the final config file path was found in.</param>
        /// <returns>The path of a config file found by looking up file key in the application wide configuration.</returns>
        protected static string GetFile(string[] args, string fileKey, string defaultFile, out IConfigSource source) {
            source = GetMainConfig(args);
            return source.Configs["Configs"].Get(fileKey, defaultFile);
        }

        /// <summary>
        /// The name of the type of configuration this is.
        /// </summary>
        public abstract string Group {
            get;
        }

        public string Folder {
            get { return Path.GetDirectoryName(mFile); }
        }

        public ConfigBase(params string[] args) {
            IConfigSource mSource = GetMainConfig(args, out mArgConfig, out mFile);

            mArgConfig.AddSwitch("General", "Section", "s");
            Section = Init.Get(mSource.Configs["General"], "Section", "MainWindow");

            InitConfig();
        }

        public ConfigBase(string section, string[] args) {
            GetMainConfig(args, out mArgConfig, out mFile);

            Section = section;

            InitConfig();
        }

        public ConfigBase(string section, string file, string[] args) {
            Section = section;
            mFile = file;
            mArgConfig = Init.InitArgConfig(args);
            
            InitConfig();
        }

        private void LoadConfig() {
            mSource = Init.AddFile(mArgConfig, mFile);
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
            AddCommandLineKey(general ? "General" : Section, key);
        }
        /// <summary>
        /// Add a key to the list of command line arguments that will be interpreted.
        /// This will have no effect if one of the Get methods has already been called.
        /// </summary>
        /// <param name="section">Whether to add it to the general config or Name.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="shortkey">The short version of the key.</param>
        protected void AddCommandLineKey(string section, string key, string shortkey) {
            mArgConfig.AddSwitch(section, key, shortkey);
            if (!commandLineKeys.ContainsKey(section))
                commandLineKeys.Add(section, new HashSet<string>());
            commandLineKeys[section].Add(key);


            if (!commandLineShortKeys.ContainsKey(section))
                commandLineShortKeys.Add(section, new Dictionary<string, string>());
            if (!commandLineShortKeys[section].ContainsKey(key))
                commandLineShortKeys[section].Add(key, shortkey);
        }

        /// <summary>
        /// Add a key to the list of command line arguments that will be interpreted.
        /// This will have no effect if one of the Get methods has already been called.
        /// </summary>
        /// <param name="general">Whether to add it to the general config or Name.</param>
        /// <param name="key">The key to add.</param>
        protected void AddCommandLineKey(bool general, string key) {
            AddCommandLineKey(general ? "General" : Section, key);
        }
        /// <summary>
        /// Add a key to the list of command line arguments that will be interpreted.
        /// This will have no effect if one of the Get methods has already been called.
        /// </summary>
        /// <param name="section">Whether to add it to the general config or Name.</param>
        /// <param name="key">The key to add.</param>
        protected void AddCommandLineKey(string section, string key) {
            mArgConfig.AddSwitch(section, key);
            if (!commandLineKeys.ContainsKey(section))
                commandLineKeys.Add(section, new HashSet<string>());
            commandLineKeys[section].Add(key);
        }

        protected Vector3 GetV(string general, string key, Vector3 defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            Vector3 value =Init.GetV(mSource.Configs[general], key, defalt);
            AddParam(key, description, ParameterTypes.Vector3, general, defalt.ToString(), value);
            return value;
        }
        protected double Get(string general, string key, double defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            double value = Init.Get(mSource.Configs[general], key, defalt);
            AddParam(key, description, ParameterTypes.Double, general, defalt.ToString(), value);
            return value;
        }
        protected string Get(string general, string key, string defalt, string description, params string[] values) {
            if (!configLoaded)
                LoadConfig();
            string value = Init.Get(mSource.Configs[general], key, defalt);
            AddParam(key, description, ParameterTypes.String, general, defalt, value, values);
            return value;
        }
        protected float Get(string general, string key, float defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            float value = Init.Get(mSource.Configs[general], key, defalt);
            AddParam(key, description, ParameterTypes.Float, general, defalt.ToString(), value);
            return value;
        }
        protected int Get(string general, string key, int defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            int value = Init.Get(mSource.Configs[general], key, defalt);
            AddParam(key, description, ParameterTypes.Int, general, defalt.ToString(), value);
            return value;
        }
        protected bool Get(string general, string key, bool defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            bool value = Init.Get(mSource.Configs[general], key, defalt);
            AddParam(key, description, ParameterTypes.Bool, general, defalt.ToString(), value);
            return value;
        }
        protected T Get<T>(string general, string key, T defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            T value = (T)Enum.Parse(typeof(T), Get(general, key, defalt.ToString(), ""));
            //Init.Get(mSource.Configs[general], key, defalt);
            List<object> vs = new List<object>();
            foreach (var v in Enum.GetValues(typeof(T)))
                vs.Add(v);
            string[] values = vs.Select(v => v.ToString()).ToArray();
            AddParam(key, description, ParameterTypes.Enum, general, defalt.ToString(), value, values);
            return value;
        }


        protected Vector3 GetV(bool general, string key, Vector3 defalt, string description) {
            return GetV(general ? "General" : Section, key, defalt, description);
        }
        protected double Get(bool general, string key, double defalt, string description) {
            return Get(general ? "General" : Section, key, defalt, description);
        }
        protected string Get(bool general, string key, string defalt, string description, params string[] values) {
            return Get(general ? "General" : Section, key, defalt, description, values);
        }
        protected float Get(bool general, string key, float defalt, string description) {
            return Get(general ? "General" : Section, key, defalt, description);
        }
        protected int Get(bool general, string key, int defalt, string description) {
            return Get(general ? "General" : Section, key, defalt, description);
        }
        protected bool Get(bool general, string key, bool defalt, string description) {
            return Get(general ? "General" : Section, key, defalt, description);
        }
        protected T Get<T>(bool general, string key, T defalt, string description) {
            return Get<T>(general ? "General" : Section, key, defalt, description);
        }

        protected abstract void InitConfig();
    }
}
