using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPlugin
{
    public static class PluginSaveHelper
    {
        public static string AssemblyPath(System.Reflection.Assembly asm)
        {
            return Path.GetDirectoryName(asm.Location);
        }

        public static string BuildPath(PluginInformation info)
        {
            var path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), info.Name);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static string BuildSavePath(PluginInformation info, DateTime date, string optional = null)
        {
            return Path.Combine(BuildPath(info), BuildSaveName(info, date, optional));
        }

        public static string BuildSaveName(PluginInformation info, DateTime date, string optional)
        {
            return string.Format("{0} {1} Installer{2}", date.ToString("yyyy-MM-dd"), info.Name, string.IsNullOrWhiteSpace(optional) ? ".exe" : optional);
        }
    }
}
