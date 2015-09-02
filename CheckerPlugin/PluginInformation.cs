using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPlugin
{
    public class PluginInformation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public Version Version { get; set; }
        public TimeSpan Intervall { get; set; }
    }
}
