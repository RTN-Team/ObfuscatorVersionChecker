using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CheckerPlugin;
using CheckerUtils;
using CheckerUtils.Http;
using CheckerUtils.Settings;

namespace AgileNET.Plugin
{
    public class Checker : IPlugin
    {
        #region "Events"
        public event OnStart EventStart;

        public event OnEnd EventEnd;

        public event OnProcessing EventProcessing;

        public event OnUpdate EventUpdate;

        public event OnExceptionOccurred EventExceptionOccurred;
        #endregion

        #region "Variables"
        private CancellationTokenSource source;
        private PluginInformation info;
        private PluginSiteInformation siteInfo;
        private HttpClient http;
        private Downloader down;
        private IniFile settings;
        private InstallerInfo latest;
        #endregion

        public Checker()
        {
            info = new PluginInformation()
            {
                Name = "Agile.NET",
                Description = "Version Checker for Agile.NET",
                Publisher = "li0nsar3c00l",
                Version = new Version(1, 0, 0, 0),
                Intervall = new TimeSpan(0, 1, 0)
            };
            siteInfo = new PluginSiteInformation()
            {
                BaseUrl = "http://secureteam.net",
                InstallerUrl = "http://secureteam.net/AgileDotNetInstaller.exe"
            };
                        
        }

        #region "Properties"
        public string LatestVersion
        {
            get { return string.Format("Version: {0}, Time: {1}, Size: {2}", latest.Version, latest.CreationTime, latest.Length); }
        }

        public PluginInformation Info
        {
            get { return info; }
        }

        public PluginSiteInformation SiteInfo
        {
            get { return SiteInfo; }
        }
        #endregion

        public PluginExecuteResult Load()
        {
            http = new HttpClient();
            down = new Downloader();
            down.ResponseHandler += SetResponseInfo;
            down.ProgressHandler += Progress;
            down.DoneHandler += Done;

            settings = IniFile.FromFile(Default.Name);
            latest = new InstallerInfo();
            latest.Version = settings[info.Name]["Version"] ?? "-";
            long ticks;
            if (long.TryParse(settings[info.Name]["Time"], out ticks))
            {
                latest.CreationTime = new DateTime(ticks);
            }
            long length;
            if (long.TryParse(settings[info.Name]["Length"], out length))
            {
                latest.Length = length;
            }
            if (latest.CreationTime != DateTime.MinValue || latest.Length != 0)
                EventStart(this, new PluginEventArgs(string.Format("Version: {0} (Date: {1}, Length: {2})", latest.Version, latest.CreationTime, latest.Length)));
            return PluginExecuteResult.Successful;
        }

        public void Start()
        {
            EventStart(this, new PluginEventArgs(string.Format("Starting {0} Checker v{1} by {2} at {3}", info.Name,info.Version,info.Publisher,info.Intervall)));
            source = new CancellationTokenSource();
            var t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    while (true)
                    {
                        if (source.IsCancellationRequested)
                            return;

                        var result = Execute();

                        Task.Delay(info.Intervall).Wait();
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Canceled!");
                }
                catch(Exception ex)
                {
                    EventExceptionOccurred(this, new PluginEventArgs(ex));
                }
            }));
            t.Start();
            
        }

        public PluginExecuteResult Execute()
        {
            EventProcessing(this, new PluginEventArgs(string.Format("Checking for {0}...", this)));

            bool newVersion = false;
            InstallerInfo installerInfo = http.LastModified(siteInfo.InstallerUrl);
            if(installerInfo.CreationTime.Date > latest.CreationTime.Date)
            {
                newVersion = true;
            }
            else if(installerInfo.Length != latest.Length)
            {
                newVersion = true;
            }
            if (newVersion)
            {
                EventUpdate(this, new PluginEventArgs("New version found!"));
                NewLatestVersion(installerInfo);
                latest = installerInfo;
                string path = PluginSaveHelper.BuildSavePath(info, latest.CreationTime);
                if(!File.Exists(path))
                    down.FileAsync(siteInfo.InstallerUrl);
                else
                {
                    EventProcessing(this, new PluginEventArgs("File already exists already"));
                    BuildVersion(path);
                }

            }


            return PluginExecuteResult.Failed;
        }

        private void SetResponseInfo(string statusDescr, string contentLength)
        {
            //EventProcessing(this, new PluginEventArgs(statusDescr));
        }

        private void Progress(int totalBytes, double pctComplete, double transferRate)
        {
            PluginProgress process = new PluginProgress(totalBytes,pctComplete,transferRate);
            EventProcessing(this, new PluginEventArgs(process));
        }

        private void Done(RequestState state)
        {
            string path = PluginSaveHelper.BuildSavePath(info, latest.CreationTime);
            File.WriteAllBytes(path, state.ResponseContent.ToArray());

            BuildVersion(path);
            EventProcessing(this, new PluginEventArgs(latest.ToString()));
        }

        private void BuildVersion(string path)
        {
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(path);
            var a = new Version(fileVersion.FileVersion);
            var b = new Version(fileVersion.ProductVersion);
            latest.Version = a.HigherThan(b) ? a.ToString() : b.ToString();
            settings[info.Name]["Version"] = latest.Version;
            settings.Save(Default.Name);
        }

        private void NewLatestVersion(InstallerInfo installerInfo)
        {
            settings[info.Name]["Version"] = installerInfo.Version;
            settings[info.Name]["Time"] = installerInfo.CreationTime.Ticks.ToString();
            settings[info.Name]["Length"] = installerInfo.Length.ToString();
            settings.Save(Default.Name);
        }

        public bool Cancel()
        {
            if (source != null)
            {
                source.Cancel();
                EventEnd(this, null);
                return source.IsCancellationRequested;
            }
            else
                return false;
        }

        public PluginExecuteResult Close()
        {
            bool result = Cancel();
            settings.Save(Default.Name);
            return result ? PluginExecuteResult.Successful : PluginExecuteResult.Failed;
        }

        public override string ToString()
        {
            return info.Name;
        }
        
    }
}
