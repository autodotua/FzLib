using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.Program.Runtime
{
    public class SingleInstance : IDisposable
    {
        public string Name { get; private set; }
        public bool ExistAnotherInstance { get; private set; }
        private static Mutex mutex;

        public SingleInstance(string name)
        {
            Name = name;
            mutex = new Mutex(false, name, out bool isSingle);
            ExistAnotherInstance = !isSingle;
        }

        ///// <summary>
        ///// 检查是否有其他实例，若有其他实例则打开其他实例的窗口
        ///// </summary>
        ///// <param name="programName"></param>
        ///// <param name="app"></param>
        ///// <returns>若无其他实例返回True，若有其他实例但无法打开窗口返回False</returns>
        //public static bool CheckAnotherInstanceAndShutdown( Application app)
        //{
        //    string programName = Information.ProgramName;
        //    mutex = new Mutex(false, programName, out bool isSingle);
        //    if (!isSingle)
        //    {
        //        //FindAndOpenWindow(windowName);
        //        Process currProc = Process.GetCurrentProcess();

        //        Process[] runningProc = Process.GetProcesses();

        //        var searchResult = runningProc.Where(p => p.ProcessName == currProc.ProcessName);

        //        //选出和当前进程进程名相同，但是id不同的那个进程
        //        Process firstProc = searchResult.FirstOrDefault(a => a.Id != currProc.Id);
        //        if (firstProc == null)
        //        {
        //            return false;
        //        }
        //        IntPtr firstProcWindow = firstProc.MainWindowHandle;
        //        SetForegroundWindow(firstProcWindow);

        //        app.Shutdown();

        //    }
        //    return true;
        //}

        //[DllImport("user32.dll")]

        //private static extern int SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 检查是否有另一个实例，若没有则开启客户端用于接受信息，若有则打开服务器连接第一个实例的客户端并发送指令使其打开窗口，随机关闭当前实例
        /// </summary>
        /// <typeparam name="T">需要打开的窗口的类</typeparam>
        /// <param name="programName">名称</param>
        /// <param name="app">Application实例</param>
        /// <returns>是否存在另一个实例</returns>
        public async Task<bool> CheckAndOpenWindow<T>(Application app) where T : Window, new()
        {
            if (ExistAnotherInstance)
            {
                await SendOpenWindowMessage();
                return true;
            }
            else
            {
                RegistClient<T>(app);
                return false;
            }
        }

        public async Task<bool> CheckAndOpenWindow<T>(Application app, ISingleObject<T> obj) where T : Window, new()
        {
            if (ExistAnotherInstance)
            {
                await SendOpenWindowMessage();
                return true;
            }
            else
            {
                RegistClient<T>(app, obj);
                return false;
            }
        }

        private async Task SendOpenWindowMessage()
        {
            SimplePipe.Server pipe = new SimplePipe.Server(Name + "_Mutex");
            await pipe.SendMessageAsync("OpenWindow");
            //await pipe.StopClinetAsync();
            pipe.Dispose();
            Environment.Exit(-1);
        }

        private void RegistClient<T>(Application app) where T : Window, new()
        {
            SimplePipe.Clinet pipe = new SimplePipe.Clinet(Name + "_Mutex");
            pipe.Start();
            pipe.GotMessage += (p1, p2) =>
            {
                if (p2.Message == "OpenWindow")
                {
                    app.Dispatcher.Invoke(() =>
                    {
                        if (app.MainWindow == null)
                        {
                            app.MainWindow = new T();
                        }
                        try
                        {
                            app.MainWindow.Show();
                        }
                        catch (InvalidOperationException)
                        {
                            app.MainWindow = new T();
                            app.MainWindow.Show();
                        }
                        if (app.MainWindow.Visibility == Visibility.Hidden)
                        {
                            app.MainWindow.Visibility = Visibility.Collapsed;
                        }
                        if (app.MainWindow.Visibility != Visibility.Visible)
                        {
                            app.MainWindow.Visibility = Visibility.Visible;
                        }
                        if (app.MainWindow.WindowState == WindowState.Minimized)
                        {
                            app.MainWindow.WindowState = WindowState.Normal;
                        }
                        app.MainWindow.Activate();
                        //pipe.Dispose();
                        //pipe.Start();
                        //SetForegroundWindow(new WindowInteropHelper(app.MainWindow).Handle);
                    });
                }
            };
        }

        private void RegistClient<T>(Application app, ISingleObject<T> obj) where T : Window, new()
        {
            SimplePipe.Clinet pipe = new SimplePipe.Clinet(Name + "_Mutex");
            pipe.Start();
            pipe.GotMessage += (p1, p2) =>
            {
                if (p2.Message == "OpenWindow")
                {
                    app.Dispatcher.Invoke(() =>
                    {
                        if (obj.SingleObject == null)
                        {
                            obj.SingleObject = new T();
                        }
                        try
                        {
                            obj.SingleObject.Show();
                        }
                        catch (InvalidOperationException)
                        {
                            obj.SingleObject = new T();
                            obj.SingleObject.Show();
                        }
                        if (obj.SingleObject.Visibility == Visibility.Hidden)
                        {
                            obj.SingleObject.Visibility = Visibility.Collapsed;
                        }
                        if (obj.SingleObject.Visibility != Visibility.Visible)
                        {
                            obj.SingleObject.Visibility = Visibility.Visible;
                        }
                        if (obj.SingleObject.WindowState == WindowState.Minimized)
                        {
                            obj.SingleObject.WindowState = WindowState.Normal;
                        }
                        obj.SingleObject.Activate();
                        //pipe.Dispose();
                        //pipe.Start();
                        //SetForegroundWindow(new WindowInteropHelper(app.MainWindow).Handle);
                    });
                }
            };
        }

        public void Dispose()
        {
            mutex.Dispose();
        }
    }
}