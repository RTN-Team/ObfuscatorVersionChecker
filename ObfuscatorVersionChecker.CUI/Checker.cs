using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CheckerPlugin;

namespace ObfuscatorVersionChecker.CUI
{
    public class Checker
    {
        private List<IPlugin> plugins;
        private Logger logger;
        private Dictionary<IPlugin, int> pctDict;
        private string text;

        public Checker()
        {
            logger = new Logger();
            plugins = new List<IPlugin>();
            pctDict = new Dictionary<IPlugin, int>();

        }

        public void Initialize()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            Assembly asm = Assembly.GetExecutingAssembly();
            Console.Title = text = string.Format("{0} v{1}", asm.GetName().Name, asm.GetName().Version);
            logger.Log(text);

            logger.Log("Loading Plugins...");
            string[] files = Directory.GetFiles(Path.GetDirectoryName(asm.Location), "*.Plugin.dll");
            foreach (string file in files)
            {
                Assembly asmTmp = Assembly.LoadFrom(file);
                foreach (Type type in asmTmp.GetTypes())
                {
                    if (type.IsInterface || type.IsAbstract)
                    {
                        continue;
                    }
                    else if (typeof(IPlugin).IsAssignableFrom(type))
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                        plugin.EventStart += EventStart;
                        plugin.EventEnd += EventEnd;
                        plugin.EventProcessing += EventProcessing;
                        plugin.EventExceptionOccurred += EventExceptionOccurred;
                        plugin.EventUpdate += EventUpdate;
                        plugins.Add(plugin);
                    }
                }
            }
            logger.Log(string.Format("{0} Plugin{1} detected", plugins.Count, (plugins.Count == 1) ? string.Empty : "s"));
            Console.ResetColor();
            foreach (IPlugin plugin in plugins)
                plugin.Load();
        }

        #region "Events"
        private void EventProcessing(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
        }
        private void EventUpdate(object sender, PluginEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            RefreshProgress(sender, e);
            Console.ResetColor();
        }

        private void EventExceptionOccurred(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
        }

        private void EventStart(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
        }

        private void EventEnd(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
        }
        #endregion

        void RefreshProgress(object sender, PluginEventArgs e)
        {
            if (e != null)
            {
                if (e.RaisedException != null)
                {
                    logger.Log(sender, "Error:");
                    logger.Log(sender, e.RaisedException.Message);
                }
                if (e.Progress != null)
                {
                    IPlugin plugin = (IPlugin)sender;
                    if (plugin != null)
                    {
                        int pct = (int)e.Progress.PctComplete;
                        if (pct == 100)
                        {
                            pctDict.Remove((IPlugin)sender);
                        }
                        else if (pctDict.ContainsKey(plugin))
                        {
                            pctDict[plugin] = pct;
                        }
                        else
                        {
                            pctDict.Add(plugin, pct);
                        }
                        DisplayProgress();
                    }
                }

                if (!string.IsNullOrEmpty(e.Message))
                {

                    logger.Log(sender, e.Message);
                }
            }
        }

        private void DisplayProgress()
        {
            if (pctDict.Keys.Count != 0)
            {
                int pct = 0;
                foreach (var plugin in pctDict.Keys)
                {
                    pct += pctDict[plugin];
                }
                pct /= pctDict.Keys.Count;
                Console.Title = string.Format("{0} - Downloading [{1}%]", text, pct);
            }
            else
            {
                Console.Title = text;
            }
        }

        public void Start()
        {
            foreach (IPlugin plugin in plugins)
                plugin.Start();
        }

        public void Close()
        {
            foreach (IPlugin plugin in plugins)
                plugin.Close();
        }

    }
}
