using FzLib.Control.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Program
{
    public static class Verification
    {
        public enum OprationMode
        {
            None,
            Shutdown,
            WarnAndShutdown,
        }
        public static bool CheckAvailableDeadlines(DateTime lastTime, OprationMode mode)
        {
            if(DateTime.Now<lastTime)
            {
                return true;
            }

            switch(mode)
            {
                case OprationMode.Shutdown:
                    Environment.Exit(0);
                    break;
                case OprationMode.WarnAndShutdown:
                    TaskDialog.ShowError(null, "此程序已过使用期限");
                    Environment.Exit(0);
                    break;
            }
            return false;
        }
    }
}
