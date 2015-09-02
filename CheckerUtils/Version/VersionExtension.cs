using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerUtils
{
    public static class VersionExtension
    {
        public static bool HigherThan(this Version a, Version b)
        {
            if(a==null)
                return false;
            if (b == null)
                return true;
            if (a.Major > b.Major)
                return true;
            if (a.Minor > b.Minor)
                return true;
            if (a.Build > b.Build)
                return true;
            if (a.Revision > b.Revision)
                return true;
            else
                return false;
        }
    }
}
