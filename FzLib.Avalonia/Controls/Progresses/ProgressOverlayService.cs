using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FzLib.Avalonia.Controls
{
    public partial class ProgressOverlayService : IProgressOverlayService
    {
        private bool hasRegistered = false;
        private Action<bool> setCancelable;
        private Action<ICommand> setCancelCommand;
        private Action<TimeSpan> setDelay;
        private Action<string> setMessage;
        private Action<string> setTitle;
        private Action<bool> setVisible;
        public void Attach(ProgressRingOverlay overlay)
        {
            Register(
                p => overlay.IsActive = p,
                p => overlay.Delay = p,
                null,
                null, null, null);
        }

        public void Attach(ProgressRingBoxOverlay overlay)
        {
            Register(
                p => overlay.IsActive = p,
                p => overlay.Delay = p,
                p => overlay.Title = p,
                p => overlay.Message = p,
                p => overlay.CanCancel = p,
                p => overlay.CancelCommand = p);
        }

        public void Register(Action<bool> setVisible,
                                           Action<TimeSpan> setDelay,
                           Action<string> setTitle,
                           Action<string> setMessage,
                           Action<bool> setCancelable,
                           Action<ICommand> setCancelCommand)
        {
            if (hasRegistered)
            {
                throw new InvalidOperationException("ProgressOverlayService 只能注册一次");
            }

            this.setVisible = setVisible ?? throw new ArgumentNullException(nameof(setVisible));
            this.setTitle = setTitle;
            this.setDelay = setDelay;
            this.setMessage = setMessage;
            this.setCancelable = setCancelable;
            this.setCancelCommand = setCancelCommand;
            hasRegistered = true;
        }
        public void SetCancelable(bool cancelable)
        {
            CheckRegister();
            if (setCancelable == null)
            {
                throw new InvalidOperationException("注册的服务不支持取消操作");
            }
            setCancelable(cancelable);
        }

        public void SetCancelCommand(ICommand command)
        {
            CheckRegister();
            if (setCancelCommand == null)
            {
                throw new InvalidOperationException("注册的服务不支持取消操作");
            }
            setCancelCommand(command);
        }

        public void SetDelay(TimeSpan delay)
        {
            CheckRegister();
            if (setDelay == null)
            {
                throw new InvalidOperationException("注册的服务不支持设置延迟");
            }
            setDelay(delay);
        }

        public void SetMessage(string message)
        {
            CheckRegister();
            if (setMessage == null)
            {
                throw new InvalidOperationException("注册的服务不支持显示消息");
            }
            setMessage(message);
        }

        public void SetTitle(string title)
        {
            CheckRegister();
            if (setTitle == null)
            {
                throw new InvalidOperationException("注册的服务不支持显示标题");
            }
            setTitle(title);
        }

        public void SetVisible(bool visible)
        {
            CheckRegister();
            setVisible(visible);
        }

        public async Task WithOverlayAsync(Func<Task> task, Func<Exception, Task> onError = null, string initialMessage = null, TimeSpan? delay = null)
        {
            CheckRegister();

            try
            {
                setDelay?.Invoke(delay ?? TimeSpan.Zero);
                setMessage?.Invoke(initialMessage);
                setCancelable?.Invoke(false);
                setVisible(true);

                await task();
            }
            catch (Exception ex)
            {
                if (onError == null)
                {
                    throw;
                }
                await onError(ex);
            }
            finally
            {
                setVisible(false);
            }
        }

        public async Task WithOverlayAsync(Func<CancellationToken, Task> task, Func<Task> onCancel = null, Func<Exception, Task> onError = null, string initialMessage = null, TimeSpan? delay = null)
        {
            CheckRegister();
            if (setCancelable == null)
            {
                throw new InvalidOperationException("注册的服务不支持取消操作");
            }
            if (setCancelCommand == null)
            {
                throw new InvalidOperationException("注册的服务不支持取消操作");
            }
            try
            {
                setDelay?.Invoke(delay ?? TimeSpan.Zero);
                setMessage?.Invoke(initialMessage);
                setCancelable?.Invoke(false);
                setVisible(true);
                CancellationTokenSource cts = new CancellationTokenSource();
                setCancelable(true);
                setCancelCommand(new AsyncRelayCommand(cts.CancelAsync));
                await task(cts.Token);
            }
            catch (OperationCanceledException)
            {
                if (onCancel != null)
                {
                    await onCancel();
                }
            }
            catch (Exception ex)
            {
                if (onError == null)
                {
                    throw;
                }
                await onError(ex);
            }
            finally
            {
                setVisible(false);
            }

        }

        public async Task WithOverlayAsync(Func<Task> task, Func<Task> onCancel, Func<Exception, Task> onError = null, string initialMessage = null, TimeSpan? delay = null)
        {
            CheckRegister();
            if (setCancelable == null)
            {
                throw new InvalidOperationException("注册的服务不支持取消操作");
            }
            if (setCancelCommand == null)
            {
                throw new InvalidOperationException("注册的服务不支持取消操作");
            }
            try
            {
                setDelay?.Invoke(delay ?? TimeSpan.Zero);
                setMessage?.Invoke(initialMessage);
                setCancelable?.Invoke(false);
                setVisible(true);
                setCancelable(true);
                setCancelCommand(new AsyncRelayCommand(() =>
                {
                    return onCancel?.Invoke();
                }));
                await task();
            }
            catch (Exception ex)
            {
                if (onError == null)
                {
                    throw;
                }
                await onError(ex);
            }
            finally
            {
                setVisible(false);
            }
        }

        private void CheckRegister()
        {
            if (!hasRegistered)
            {
                throw new InvalidOperationException("请先调用 Attach 方法注册服务");
            }
        }
    }
}