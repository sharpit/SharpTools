using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using SharpIT.Tools.Sql.Scripter.Commons.Configurations;

namespace SharpIT.Tools.Sql.Scripter.Commons.Services
{
    public class DatabaseScripter
    {
        protected DatabaseConfigurationElement Configuration;

        // -----------------------------------------------------------------------------------------
        public DatabaseScripter(DatabaseConfigurationElement configuration)
        // -----------------------------------------------------------------------------------------
        {
            Configuration = configuration;

            var sqlBuilder = new SqlConnectionStringBuilder();
            var connectionInfo = new SqlConnectionInfo();

        }

        // -----------------------------------------------------------------------------------------
        public void ScriptObjects()
        // -----------------------------------------------------------------------------------------
        {
            var srv = new Server(GetServerConnection(Configuration));
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Configuration.ConnectionString);

            Console.WriteLine("Skryptowanie bazy '{0}' ... ", sqlConnectionStringBuilder.InitialCatalog);

            var db = srv.Databases[sqlConnectionStringBuilder.InitialCatalog];

            var outputFile = string.Format("{0}.{1:yyyy-MM-dd HH.mm.ss}.sql", sqlConnectionStringBuilder.InitialCatalog, DateTime.Now);

            //Define a Scripter object and set the required scripting options. 
            var scripter = new Microsoft.SqlServer.Management.Smo.Scripter(srv)
            {
                Options =
                {
                    AnsiPadding = false,
                    AllowSystemObjects = false,
                    Encoding = Encoding.UTF8,
                    IncludeIfNotExists = false,
                    ScriptDrops = false,
                    WithDependencies = false,
                    IncludeHeaders = false,
                    ExtendedProperties = false,
                    Default = true,
                    IncludeDatabaseContext = true,
                    NoCollation = true,
                    SchemaQualify = true,
                    DriAll = true,
                    Permissions = false,
                    Triggers = true,
                    ScriptSchema = true,
                    ScriptData = false,
                    FullTextIndexes = true,
                    TargetServerVersion = SqlServerVersion.Version105,
                    Indexes = true
                }                
            };

            var sb = new StringBuilder();

            ScriptTables(srv, db, scripter, sb);
            ScriptViews(srv, db, scripter, sb);
            ScriptStoredProcedures(srv, db, scripter, sb);
            ScriptUserDefinedFunctions(srv, db, scripter, sb);
            ScriptTriggers(srv, db, scripter, sb);

            File.WriteAllText(outputFile, sb.ToString());
        }

        // -----------------------------------------------------------------------------------------
        protected ServerConnection GetServerConnection(DatabaseConfigurationElement configuration)
        // -----------------------------------------------------------------------------------------
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(configuration.ConnectionString);
            var sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            return new ServerConnection(sqlConnection);
        }

        // -----------------------------------------------------------------------------------------
        protected void ScriptUserDefinedFunctions(Server server, Database db, Microsoft.SqlServer.Management.Smo.Scripter scripter, StringBuilder sb)
        // -----------------------------------------------------------------------------------------
        {
            sb.AppendLine("--> User Defined Functions (UDFs) --");
            server.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject");

            foreach (var udf in db.UserDefinedFunctions.Cast<UserDefinedFunction>().Where(udf => !udf.IsSystemObject).OrderBy(x => x.Name))
            {
                sb.Append(ScriptObject(new[] { udf.Urn }, scripter));
            }

            sb.AppendLine("-- User Defined Functions (UDFs) <--");
        }

        // -----------------------------------------------------------------------------------------
        protected void ScriptStoredProcedures(Server server, Database db, Microsoft.SqlServer.Management.Smo.Scripter scripter, StringBuilder sb)
        // -----------------------------------------------------------------------------------------
        {
            sb.AppendLine("--> Stored Procedures (SPs) --");
            server.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject");

            foreach (var sp in db.StoredProcedures.Cast<StoredProcedure>().Where(sp => !sp.IsSystemObject).OrderBy(x => x.Name))
            {
                sb.Append(ScriptObject(new[] { sp.Urn }, scripter));
            }

            sb.AppendLine("-- Stored Procedures (SPs) <--");
        }

        // -----------------------------------------------------------------------------------------
        protected void ScriptViews(Server server, Database db, Microsoft.SqlServer.Management.Smo.Scripter scripter, StringBuilder sb)
        // -----------------------------------------------------------------------------------------
        {
            sb.AppendLine("--> Views --");
            server.SetDefaultInitFields(typeof(View), "IsSystemObject");

            foreach (var view in db.Views.Cast<View>().Where(view => !view.IsSystemObject).OrderBy(x => x.Name))
            {
                sb.Append(ScriptObject(new[] { view.Urn }, scripter));
            }

            sb.AppendLine("-- Views <--");
        }

        // -----------------------------------------------------------------------------------------
        protected void ScriptTables(Server server, Database db, Microsoft.SqlServer.Management.Smo.Scripter scripter, StringBuilder sb)
        // -----------------------------------------------------------------------------------------
        {
            sb.AppendLine("--> Tables --");
            server.SetDefaultInitFields(typeof(Table), "IsSystemObject");

            foreach (var table in db.Tables.Cast<Table>().Where(table => !table.IsSystemObject).OrderBy(x => x.Name))
            {
                sb.Append(ScriptObject(new[] { table.Urn }, scripter));
            }

            sb.AppendLine("-- Tables <--");
        }

        // -----------------------------------------------------------------------------------------
        protected void ScriptTriggers(Server server, Database db, Microsoft.SqlServer.Management.Smo.Scripter scripter, StringBuilder sb)
        // -----------------------------------------------------------------------------------------
        {
            sb.AppendLine("--> Triggers --");
            server.SetDefaultInitFields(typeof(Trigger), "IsSystemObject");

            foreach (var trigger in db.Triggers.Cast<Trigger>().Where(trigger => !trigger.IsSystemObject).OrderBy(x => x.Name))
            {
                sb.Append(ScriptObject(new[] { trigger.Urn }, scripter));
            }

            sb.AppendLine("-- Triggers <--");
        }

        // -----------------------------------------------------------------------------------------
        protected string ScriptObject(Urn[] urns, Microsoft.SqlServer.Management.Smo.Scripter scripter)
        // -----------------------------------------------------------------------------------------
        {
            var sc = scripter.Script(urns);
            var sb = new StringBuilder();

            foreach (var str in sc)
            {
                sb.AppendFormat("{0}{1}GO{1}{1}", str, Environment.NewLine);
            }

            return sb.ToString();
        }


    }
}
