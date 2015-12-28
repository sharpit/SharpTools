using System;
using SharpIT.Tools.Sql.Scripter.Commons.Configurations;
using SharpIT.Tools.Sql.Scripter.Commons.Services;

namespace SharpIT.Tools.Sql.Scripter
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = ScripterConfigurationSection.Instance;

            foreach (var databaseConfiguration in configuration.Databases)
            {
                var scripter = new DatabaseScripter(databaseConfiguration as DatabaseConfigurationElement);
                scripter.ScriptObjects();
            }

            Console.WriteLine("... zakończone");
            Console.ReadLine();

        }


    }

    
}
