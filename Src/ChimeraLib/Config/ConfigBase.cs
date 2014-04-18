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
using log4net;
using Nini.Ini;

namespace Chimera.Config {
    public enum ParameterTypes {
        Bool,
        Float,
        Double,
        Int,
        String,
        Enum,
        Vector3,
        File,
        Folder
    }

    public abstract class ConfigBase {
        public const string IGNORE_FRAME = "Irrelevant";

        public string Frame = null;
        private IConfigSource mSource;
        private ArgvConfigSource mArgConfig;
        private string mFile;

        private bool configLoaded;

        private readonly Dictionary<string, HashSet<string>> commandLineKeys = new Dictionary<string, HashSet<string>>();
        private readonly Dictionary<string, Dictionary<string, string>> commandLineShortKeys = new Dictionary<string, Dictionary<string, string>>();
        private readonly Dictionary<string, Dictionary<string, ConfigParam>> mParameters = new Dictionary<string, Dictionary<string, ConfigParam>>();


        public IEnumerable<ConfigParam> Parameters {
            get { return mParameters.Values.SelectMany(d => d.Values); }
        }

        private void AddParam(string key, string description, ParameterTypes type, string section, string defalt, object value, params string[] values) {
            bool commandLine = commandLineKeys.ContainsKey(section) && commandLineKeys[section].Contains(key);
            string shortKey = commandLine && commandLineShortKeys.ContainsKey(section) ? commandLineShortKeys[section][key] : null;

            if (!mParameters.ContainsKey(section))
                mParameters.Add(section, new Dictionary<string, ConfigParam>());
            if (!mParameters[section].ContainsKey(key))
                mParameters[section].Add(key, new ConfigParam(key, description, type, section, Group, defalt, commandLine, shortKey, value != null ? value.ToString() : "null", mFile, values));
            else {
                ConfigParam param = mParameters[section][key];
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
            //Frame = Init.Get(mSource.Configs["General"], "Section", "MainWindow");

            InitConfig();
        }

        public ConfigBase(string frame, string[] args) {
            GetMainConfig(args, out mArgConfig, out mFile);

            Frame = frame;

            InitConfig();
        }

        public ConfigBase(string file, string frame, string[] args) {
            Frame = frame;
            mFile = file;
            mArgConfig = Init.InitArgConfig(args);
            
            InitConfig();
        }

        private void LoadConfig() {
            if (configLoaded)
                return;
            if (!File.Exists(mFile))
                File.Create(mFile);

            if (Path.GetExtension(mFile).ToUpper() == ".INI") {
                IniDocument doc = new IniDocument(mFile, IniFileType.WindowsStyle);
                mSource = new IniConfigSource(doc);
            } else if (Path.GetExtension(mFile).ToUpper() == ".CONFIG") {
                mSource = new DotNetConfigSource(mFile);
            }
            //mSource = Init.AddFile(mArgConfig, mFile);
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
            AddCommandLineKey(general ? "General" : Frame, key);
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
            AddCommandLineKey(general ? "General" : Frame, key);
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

        protected Vector3 GetV(string section, string key, Vector3 defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            Vector3 value =Init.GetV(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.Vector3, section, defalt.ToString(), value);
            return value;
        }
        protected double Get(string section, string key, double defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            double value = Init.Get(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.Double, section, defalt.ToString(), value);
            return value;
        }
        protected string GetSection(string section, string key, string defalt, string description, params string[] values) {
            if (!configLoaded)
                LoadConfig();
            string value = Init.Get(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.String, section, defalt, value, values);
            return value;
        }        protected string GetFileSection(string section, string key, string defalt, string description, params string[] values) {
            if (!configLoaded)
                LoadConfig();
            string value = Init.Get(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.File, section, defalt, value, values);
            return value;
        }
        protected string GetFolderSection(string section, string key, string defalt, string description, params string[] values) {
            if (!configLoaded)
                LoadConfig();
            string value = Init.Get(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.Folder, section, defalt, value, values);
            return value;
        }
        protected float Get(string section, string key, float defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            float value = Init.Get(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.Float, section, defalt.ToString(), value);
            return value;
        }
        protected int Get(string section, string key, int defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            int value = Init.Get(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.Int, section, defalt.ToString(), value);
            return value;
        }
        protected bool Get(string section, string key, bool defalt, string description) {
            if (!configLoaded)
                LoadConfig();
            bool value = Init.Get(mSource.Configs[section], key, defalt);
            AddParam(key, description, ParameterTypes.Bool, section, defalt.ToString(), value);
            return value;
        }
        protected T GetEnum<T>(string section, string key, T defalt, string description, ILog logger) where T : struct  {
            if (!configLoaded)
                LoadConfig();
            T value;
            string val = Init.Get(mSource.Configs[section], key, defalt.ToString());
            if (!Enum.TryParse<T>(val, out value)) {
                value = defalt;
                logger.Warn("Unable to load " + key + ". " + value + " is not a valid member of " + typeof(T).Name + ".");
            }
            //Init.Get(mSource.Configs[general], key, defalt);
            List<object> vs = new List<object>();
            foreach (var v in Enum.GetValues(typeof(T)))
                vs.Add(v);
            string[] values = vs.Select(v => v.ToString()).ToArray();
            AddParam(key, description, ParameterTypes.Enum, section, defalt.ToString(), value, values);
            return value;
        }


        protected Vector3 GetV(string key, Vector3 defalt, string description) {
            return GetV("General", key, defalt, description);
        }
        protected double Get(string key, double defalt, string description) {
            return Get("General", key, defalt, description);
        }
        protected string GetStr(string key, string defalt, string description, params string[] values) {
            return GetSection("General", key, defalt, description, values);
        }
        protected string GetFile(string key, string defalt, string description, params string[] values) {
            return GetFileSection("General", key, defalt, description, values);
        }
        protected string GetFolder(string key, string defalt, string description, params string[] values) {
            return GetFolderSection("General", key, defalt, description, values);
        }
        protected float Get(string key, float defalt, string description) {
            return Get("General", key, defalt, description);
        }
        protected int Get(string key, int defalt, string description) {
            return Get("General", key, defalt, description);
        }
        protected bool Get(string key, bool defalt, string description) {
            return Get("General", key, defalt, description);
        }
        protected T GetEnum<T>(string key, T defalt, string description, ILog logger) where T : struct {
            return GetEnum<T>("General", key, defalt, description, logger);
        }

        protected Vector3 GetVFrame(string key, Vector3 defalt, string description) {
            return Frame == IGNORE_FRAME ? defalt : GetV(Frame, key, defalt, description);
        }
        protected double GetFrame(string key, double defalt, string description) {
            return Frame == IGNORE_FRAME ? defalt : Get(Frame, key, defalt, description);
        }
        protected string GetFrame(string key, string defalt, string description, params string[] values) {
            return Frame == IGNORE_FRAME ? defalt : GetSection(Frame, key, defalt, description, values);
        }
        protected float GetFrame(string key, float defalt, string description) {
            return Frame == IGNORE_FRAME ? defalt : Get(Frame, key, defalt, description);
        }
        protected int GetFrame(string key, int defalt, string description) {
            return Frame == IGNORE_FRAME ? defalt : Get(Frame, key, defalt, description);
        }
        protected bool GetFrame(string key, bool defalt, string description) {
            return Frame == IGNORE_FRAME ? defalt : Get(Frame, key, defalt, description);
        }
        protected T GetFrameEnum<T>(string key, T defalt, string description, ILog logger) where T : struct {
            return Frame == IGNORE_FRAME ? defalt : GetEnum<T>(Frame, key, defalt, description, logger);
        }

        protected abstract void InitConfig();
    }
}
