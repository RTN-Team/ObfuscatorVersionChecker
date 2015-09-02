using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObfuscatorVersionChecker.CUI
{
    public class Logger
    {
        public void Log(string format)
        {
            Log("Main", format);
        }

        public void Log(string format, object args)
        {
            Log("Main", string.Format(format, args));
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
            Console.WriteLine(string.Format("[{0:HH:mm:ss}] - {1}: {2}", DateTime.Now, thread, msg));
        }
    }
}
