using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObfuscatorVersionChecker
{
    public class Logger
    {
        ListView listViewLog;
        public Logger(ListView listview)
        {
            this.listViewLog = listview;
        }

        public void Log(string format)
        {
            Log("Main", format);
        }

        public void Log(string format, object args)
        {
            Log("Main",string.Format(format, args));
        }
        public void Log(object sender, string format)
        {
            Log(sender.ToString(), format);
        }

        public void Log(object sender, string format, object args)
        {
            Log(sender.ToString(), string.Format(format, args));
        }

        public void Log(string thread, string format, object args)
        {
            Log(thread, string.Format(format, args));
        }

        public void Log(string thread, string msg)
        {
            Log(new ListViewItem(new string[]
            {
                string.Format("{0:HH:mm:ss}", DateTime.Now),
                thread,
                msg
            }));
        }

        public void Log(ListViewItem item)
        {
            if (listViewLog.InvokeRequired)
                listViewLog.Invoke(new MethodInvoker(() => { listViewLog.Items.Add(item); }));
            else
                listViewLog.Items.Add(item);
        }
    }
}
