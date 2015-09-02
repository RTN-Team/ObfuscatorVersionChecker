using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckerUtils.Http
{
    public class InstallerInfo
    {
        public DateTime CreationTime { get; set; }
        public long Length { get; set; }
        public string Version { get; set; }

        public override string ToString()
        {
            return string.Format("Version {0}, Date: {1}, Length: {2}", Version, CreationTime.ToShortDateString(), Length);
        }

    }
    public class HttpClient
    {
        public InstallerInfo LastModified(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var info = new InstallerInfo();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                info.CreationTime = response.LastModified;
                info.Length = response.ContentLength;
            }
            return info;
        }

        public string DownloadString(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            return new StreamReader(stream).ReadToEnd();
        }

    }

    
}
