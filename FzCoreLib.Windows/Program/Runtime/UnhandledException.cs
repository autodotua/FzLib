using FzLib.Program;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.Program.Runtime
{
    public static class UnhandledException
    {
        public delegate void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e);

        public static event UnhandledExceptionEventHandler UnhandledExceptionCatched;

        public static bool ExitAfterHandle { get; set; } = false;

        public static void RegistAll()
        {
            Regist();
        }

        private static void Regist()
        {
            TaskScheduler.UnobservedTaskException += (p1, p2) => //Task
            {
                if (!p2.Observed)
                {
                    RaiseEvent(p1, p2.Exception.InnerException, ExceptionSource.TaskScheduler);
                    try
                    {
                        p2.SetObserved();
                    }
                    catch
                    {

                    }
                }
            };
            AppDomain.CurrentDomain.UnhandledException += (p1, p2) =>//Thread
            {
                RaiseEvent(p1, (Exception)p2.ExceptionObject, ExceptionSource.AppDomain);
            };
            Application.Current.DispatcherUnhandledException += (p1, p2) =>//UI
            {
                RaiseEvent(p1, p2.Exception, ExceptionSource.Application);
                p2.Handled = true;
            };
        }

        private static void RaiseEvent(object sender, Exception ex, ExceptionSource source)
        {
            var e = new UnhandledExceptionEventArgs(ex, source);
            UnhandledExceptionCatched?.Invoke(sender, e);
        }

   
    }     public class UnhandledExceptionEventArgs : EventArgs
        {
            public UnhandledExceptionEventArgs(Exception exception, ExceptionSource source)
            {
                Exception = exception ?? throw new ArgumentNullException(nameof(exception));
                Source = source;
            }

            public Exception Exception { get; private set; }
            public ExceptionSource Source { get; private set; }


        }
        public enum ExceptionSource
        {
            TaskScheduler,
            AppDomain,
            Application,
        }
}
