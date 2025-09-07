using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FzLib.Avalonia.Controls
{
    public interface IProgressOverlayService
    {
        void Attach(ProgressRingBoxOverlay overlay);
        void Attach(ProgressRingOverlay overlay);
        void Register(Action<bool> setVisible, Action<TimeSpan> setDelay, Action<string> setTitle, Action<string> setMessage, Action<bool> setCancelable, Action<ICommand> setCancelCommand);
        void SetCancelable(bool cancelable);
        void SetCancelCommand(ICommand command);
        void SetDelay(TimeSpan delay);
        void SetMessage(string message);
        void SetTitle(string title);
        void SetVisible(bool visible);
        Task WithOverlayAsync(Func<Task> task, Func<Exception, Task> onError = null, string initialMessage = null, TimeSpan? delay = null);
        Task WithOverlayAsync(Func<CancellationToken, Task> task, Func<Task> onCancel = null, Func<Exception, Task> onError=null, string initialMessage = null, TimeSpan? delay = null);
        Task WithOverlayAsync(Func<Task> task, Func<Task> onCancel, Func<Exception, Task> onError = null, string initialMessage = null, TimeSpan? delay = null);
    }
}