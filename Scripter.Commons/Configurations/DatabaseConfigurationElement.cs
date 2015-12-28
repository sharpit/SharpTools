using System.Configuration;

namespace SharpIT.Tools.Sql.Scripter.Commons.Configurations
{
    public class DatabaseConfigurationElement : ConfigurationElement
    {
        private const string NameKey = "name";
        private const string ConnectionStringKey = "connectionString";

        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
        }

        [ConfigurationProperty(ConnectionStringKey, IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)base[ConnectionStringKey]; }
        }
    }
}