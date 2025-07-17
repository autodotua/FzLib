using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.Application.Runtime
{
    public class WPFUnhandledExceptionCatcher : UnhandledExceptionCatcher
    {
        public static WPFUnhandledExceptionCatcher RegistAll()
        {
            WPFUnhandledExceptionCatcher catcher = new WPFUnhandledExceptionCatcher();
            catcher.RegisterTaskCatcher();
            catcher.RegisterThreadsCatcher();
            catcher.RegisterUICatcher();
            return catcher;
        }

        public void RegisterUICatcher()
        {
            System.Windows.Application.Current.DispatcherUnhandledException += (p1, p2) =>//UI
            {
                base.RaiseEvent(p1, p2.Exception, (FzLib.Program.Runtime.ExceptionSource)FzLib.Program.Runtime.ExceptionSource.UI);
                p2.Handled = true;
            };
        }

    }

}