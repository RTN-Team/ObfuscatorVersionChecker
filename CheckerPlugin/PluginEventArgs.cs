using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPlugin
{
    public class PluginEventArgs : EventArgs
    {
        public PluginEventArgs(string message)
            : this(null,message,null)
        {
        }

        public PluginEventArgs(PluginProgress progress)
            : this(progress,null,null)
        {
        }

        public PluginEventArgs(Exception raisedException)
            : this(null, null, raisedException)
        {
        }

        public PluginEventArgs(PluginProgress progress, string message, Exception raisedException)
        {
            this.progress = progress;
            this.message = message;
            this.raisedException = raisedException;
        }

        private PluginProgress progress;
        private string message;
        private Exception raisedException;
        private bool cancel;

        public PluginProgress Progress
        {
            get { return progress; }
        }

        public string Message
        {
            get { return message; }
        }

        public Exception RaisedException
        {
            get { return raisedException; }
        }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }

    }
}
