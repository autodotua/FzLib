using FzLib.Program;
using FzLib.UI.Dialog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.UI.Program
{
    public static class UnhandledException
    {
        public delegate void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e);

        public static event UnhandledExceptionEventHandler UnhandledExceptionCatched;

        public static bool ExitAfterHandle { get; set; } = false;
        public static bool AutoShowMessage { get; set; }

        public static string AppName { get; set; }

        public static void RegistAll(bool autoShowMessage = true)
        {
            RegistAll(App.ProgramName, autoShowMessage);
        }
        public static void RegistAll(string appName, bool autoShowMessage = true)
        {
            AppName = appName;
            AutoShowMessage = autoShowMessage;
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
            if (AutoShowMessage)
            {
                ShowMessage(e);
            }
        }

        private static void ShowMessage(UnhandledExceptionEventArgs e)
        {
            try
            {
                ShowErrorMessage(e.Exception.Message, "程序发生了未捕获的错误", "来源：" + e.Source.ToString() + Environment.NewLine + e.Exception.ToString());
            }
            catch
            {

            }
            try
            {
                File.AppendAllText(App.ProgramDirectoryPath + "\\UnhandledException.log", Environment.NewLine + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + e.Exception.ToString());

                //Application.Current.Dispatcher.Invoke(() => MessageBox.Show("程序发生了未捕获的错误，类型" + e.Source.ToString() + Environment.NewLine + Environment.NewLine + e.Exception.ToString(), AppName, MessageBoxButton.OK, MessageBoxImage.Error));
            }
            catch (Exception ex)
            {
                ShowErrorMessage(e.Exception.Message, "错误信息无法写入磁盘", ex.ToString());
            }
            finally
            {
                if (ExitAfterHandle)
                {
                    Environment.Exit(-1);
                }
            }
        }

        private static void ShowErrorMessage(string t1, string t2, string t3)
        {
            try
            {
                TaskDialog.ShowWithDetail(t1, t2, t3, Microsoft.WindowsAPICodePack.Dialogs.TaskDialogStandardIcon.Error, false, true, "查看详细错误");
            }
            catch
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TaskDialog.ShowWithDetail(t1, t2, t3, Microsoft.WindowsAPICodePack.Dialogs.TaskDialogStandardIcon.Error, false, true, "查看详细错误");
                    });

                }
                catch
                {

                }
            }
        }

        public class UnhandledExceptionEventArgs : EventArgs
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
}
