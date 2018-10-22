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

        public static bool AutoShowMessage { get; set; }

        public static string AppName { get; set; }

        public static void RegistAll(bool autoShowMessage = true, bool notWorkInDebugMode = true)
        {
            RegistAll(Information.ProgramName,autoShowMessage,notWorkInDebugMode);
        }
        public static void RegistAll(string appName, bool autoShowMessage = true, bool notWorkInDebugMode = true)
        {
            AppName = appName;
            AutoShowMessage = autoShowMessage;
            if (notWorkInDebugMode)
            {
#if!DEBUG
                Regist();
#endif
            }
            else
            {
                Regist();
            }
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
                //Application.Current.Dispatcher.Invoke(() => MessageBox.Show("程序发生了未捕获的错误，类型" + e.Source.ToString() + Environment.NewLine + Environment.NewLine + e.Exception.ToString(), AppName, MessageBoxButton.OK, MessageBoxImage.Error));
                Notify.TaskDialog.ShowWithDetail(null, e.Exception.Message, "程序发生了未捕获的错误","来源："+e.Source.ToString()+Environment.NewLine+ e.Exception.ToString(), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogStandardIcon.Error, false, "查看详细错误");
                File.AppendAllText(Information.ProgramDirectoryPath+ "\\UnhandledException.log", Environment.NewLine + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + e.Exception.ToString());
            }
            catch (Exception ex)
            {
                Notify.TaskDialog.ShowWithDetail(null, e.Exception.Message, "错误信息无法写入磁盘",   ex.ToString(), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogStandardIcon.Error, false, "查看详细错误");

            }
            finally
            {
                Environment.Exit(-1);
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
