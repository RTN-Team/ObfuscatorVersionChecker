using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckerPlugin;
using System.Reflection;

namespace ObfuscatorVersionChecker
{
    public partial class frmMain : Form
    {
        List<IPlugin> plugins;
        Logger logger;
        uint counter = 0;

        public frmMain()
        {
            InitializeComponent();
            logger = new Logger(listView1);
        }

        private void Initialize()
        {
            plugins = new List<IPlugin>();
            string[] files = Directory.GetFiles(Application.StartupPath, "*.Plugin.dll");
            foreach (string file in files)
            {
                Assembly asm = Assembly.LoadFrom(file);
                foreach (Type type in asm.GetTypes())
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
            logger.Log("{0} Plugins detected", plugins.Count);

            foreach (IPlugin plugin in plugins)
                plugin.Load();



            timerGraph.Start();

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var asmName = Assembly.GetExecutingAssembly().GetName();
            this.Text = string.Format("{0} v{1}", asmName.Name, asmName.Version);
            logger.Log(this.Text);
            Initialize();
        }

        #region "Events"
        void EventProcessing(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
        }
        void EventUpdate(object sender, PluginEventArgs e)
        {
            counter++;
            RefreshProgress(sender, e);
        }

        void EventExceptionOccurred(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
            MessageBox.Show(e.RaisedException.ToString(), "Error executing the task!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void EventStart(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
        }

        void EventEnd(object sender, PluginEventArgs e)
        {
            RefreshProgress(sender, e);
        }
        #endregion

        void RefreshProgress(object sender, PluginEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { RefreshProgress(sender, e); }));
            }
            else
            {
                if (e != null)
                {
                    if(e.RaisedException != null)
                    {
                        logger.Log(sender, e.RaisedException.Message);
                    }
                    if (e.Progress != null)
                    {
                        itemProgress.Value = (int)e.Progress.PctComplete;
                        lineGraphSpeed.AddDataPoint((float)e.Progress.TransferRate);
                        
                    }
                    else
                    {
                        itemProgress.Value = 0;
                    }

                    if (!string.IsNullOrEmpty(e.Message))
                    {

                        logger.Log(sender, e.Message);
                    }

                    itemProgress.Refresh();
                    Application.DoEvents();
                }
            }
        }

        private void timerGraph_Tick(object sender, EventArgs e)
        {
            lineGraphCheck.AddDataPoint(counter);
            counter = 0;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerGraph.Stop();
            foreach (IPlugin plugin in plugins)
                plugin.Close();
        }

        private void buttonStartAll_Click(object sender, EventArgs e)
        {
            foreach (IPlugin plugin in plugins)
                plugin.Start();
        }

    }
}
