using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPlugin
{
    public enum PluginExecuteResult
    {
        Successful = 1,
        Failed = 0,
        Exception = -1,
        Cancelled = 2
    }
}
