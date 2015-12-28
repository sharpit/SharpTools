using System;
using System.IO;
using System.Reflection;

namespace SharpIT.Tools.Sql.Scripter.Commons.Helpers
{
    public static class IoHelper
    {
        public static string GetThisAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
