using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ObfuscatorVersionChecker.CUI
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        private delegate bool ConsoleEventDelegate(int ctrlType);

        private static Checker c;
        static void Main(string[] args)
        {
            var handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            c = new Checker();
            c.Initialize();
            c.Start();

            Console.Read();


        }

        private static bool ConsoleEventCallback(int ctrlType)
        {
            c.Close();
            return true;
        }

        

    }
}
