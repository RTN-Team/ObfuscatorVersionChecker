using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPlugin
{
    public delegate void OnStart(object sender, PluginEventArgs e);
    public delegate void OnEnd(object sender, PluginEventArgs e);
    public delegate void OnProcessing(object sender, PluginEventArgs e);
    public delegate void OnUpdate(object sender, PluginEventArgs e);
    public delegate void OnExceptionOccurred(object sender, PluginEventArgs e);
}
