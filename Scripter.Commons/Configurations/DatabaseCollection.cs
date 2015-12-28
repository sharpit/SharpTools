using System;
using System.Configuration;

namespace SharpIT.Tools.Sql.Scripter.Commons.Configurations
{
    [ConfigurationCollection(typeof(DatabaseCollectionElement))]
    public class DatabaseCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "database";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DatabaseConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatabaseConfigurationElement)(element)).Name;
        }

        public DatabaseConfigurationElement this[int idx]
        {
            get { return (DatabaseConfigurationElement)BaseGet(idx); }
        }
    }


}
