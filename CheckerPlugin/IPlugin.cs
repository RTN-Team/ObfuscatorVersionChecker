using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPlugin
{
    public interface IPlugin
    {
        event OnStart EventStart;
        event OnEnd EventEnd;
        event OnProcessing EventProcessing;
        event OnUpdate EventUpdate;
        event OnExceptionOccurred EventExceptionOccurred;
        string LatestVersion { get; }
        PluginInformation Info { get; }
        PluginSiteInformation SiteInfo { get; }
        PluginExecuteResult Load();
        void Start();
        PluginExecuteResult Execute();
        bool Cancel();
        PluginExecuteResult Close();

    }
}
