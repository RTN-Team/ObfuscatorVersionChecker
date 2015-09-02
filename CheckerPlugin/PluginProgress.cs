using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPlugin
{
    public class PluginProgress
    {
        private int totalBytes;
        private double pctComplete;
        private double transferRate;
        public PluginProgress(int totalBytes, double pctComplete, double transferRate)
        {

            this.totalBytes = totalBytes;
            this.pctComplete = pctComplete;
            this.transferRate = transferRate;

        }
        public int TotalBytes
        {
            get { return totalBytes; }
            set { totalBytes = value; }
        }

        public double PctComplete
        {
            get { return pctComplete; }
            set { pctComplete = value; }
        }

        public double TransferRate
        {
            get { return transferRate; }
            set { transferRate = value; }
        }

    }
}
