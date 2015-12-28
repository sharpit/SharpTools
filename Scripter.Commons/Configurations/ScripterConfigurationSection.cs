using System;
using System.Configuration;
using System.IO;
using SharpIT.Tools.Sql.Scripter.Commons.Helpers;

namespace SharpIT.Tools.Sql.Scripter.Commons.Configurations
{
    public class ScripterConfigurationSection : ConfigurationSection
    {
        private const string ConfigurationSectionName = "SqlScripter";
        private const string DatabasesKey = "databases";

        private static readonly Lazy<ScripterConfigurationSection> _instance = new Lazy<ScripterConfigurationSection>(() => CreateSection(), true);

        public static ScripterConfigurationSection Instance
        {
            get { return _instance.Value; }
        }
        
        [ConfigurationProperty(DatabasesKey)]
        public DatabaseCollection Databases
        {
            get { return (DatabaseCollection)base[DatabasesKey]; }
            set { base[DatabasesKey] = value; }
        }
        
        protected ScripterConfigurationSection()
        {

        }

        public void EnsureValidConfiguration()
        {

        }

        static ScripterConfigurationSection GetSectionFromDefaultConfig()
        {
            return ConfigurationManager.GetSection(ConfigurationSectionName) as ScripterConfigurationSection;
        }

        static ScripterConfigurationSection CreateSection(bool fromDefaultConfig = true)
        {
            return fromDefaultConfig ? GetSectionFromDefaultConfig() : GetSectionFromExternalConfig();
        }

        static ScripterConfigurationSection GetSectionFromExternalConfig()
        {
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(IoHelper.GetThisAssemblyDirectory(), "SqlScripter.config")
            };

            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            return config.GetSection(ConfigurationSectionName) as ScripterConfigurationSection;
        }
    }
}
