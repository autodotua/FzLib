using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FzLib.Application
{
    public static class UnhandledExceptionCatcher
    {
        public enum ExceptionSource
        {
            Task,
            Thread,
            UI,
        }

        class UnhandledExceptionEventArgs(Exception exception, ExceptionSource source) : EventArgs
        {
            public Exception Exception { get; } = exception ?? throw new ArgumentNullException(nameof(exception));
            public ExceptionSource Source { get; } = source;
        }

        public class ExceptionCatcherBuilder
        {
            private readonly Action action;
            private Action<Exception, ExceptionSource> catchHandler;
            private Action finallyHandler;

            private readonly UnhandledExceptionRegistrar registrar = new();

            internal ExceptionCatcherBuilder(Action action)
            {
                this.action = action;
            }

            public ExceptionCatcherBuilder Catch(Action<Exception, ExceptionSource> handler)
            {
                catchHandler = handler;
                return this;
            }

            public ExceptionCatcherBuilder Finally(Action handler)
            {
                finallyHandler = handler;
                return this;
            }

            public void Run()
            {
                if (Debugger.IsAttached)
                {
                    action?.Invoke();
                    return;
                }

                registrar.UnhandledExceptionCaught += (s, e) => { catchHandler?.Invoke(e.Exception, e.Source); };

                registrar.RegisterTaskCatcher();
                registrar.RegisterThreadsCatcher();

                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    catchHandler?.Invoke(ex, ExceptionSource.UI);
                }
                finally
                {
                    finallyHandler?.Invoke();
                }
            }
        }

        class UnhandledExceptionRegistrar
        {
            public event EventHandler<UnhandledExceptionEventArgs> UnhandledExceptionCaught;

            public void RegisterTaskCatcher()
            {
                TaskScheduler.UnobservedTaskException += (s, e) =>
                {
                    if (e.Exception.InnerException is SocketException)
                    {
                        //来自TcpSingleInstanceHelper的异常，不处理
                        return;
                    }
                    if (!e.Observed)
                    {
                        RaiseEvent(s, e.Exception.InnerException ?? e.Exception, ExceptionSource.Task);
                        try
                        {
                            e.SetObserved();
                        }
                        catch
                        {
                        }
                    }
                };
            }

            public void RegisterThreadsCatcher()
            {
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    RaiseEvent(s, e.ExceptionObject as Exception ?? new Exception("未知异常"),
                        ExceptionSource.Thread);
                };
            }

            private void RaiseEvent(object sender, Exception exception, ExceptionSource source)
            {
                var args = new UnhandledExceptionEventArgs(exception, source);
                UnhandledExceptionCaught?.Invoke(sender, args);
            }
        }

        public static ExceptionCatcherBuilder WithCatcher(Action action)
        {
            return new ExceptionCatcherBuilder(action);
        }
    }
}