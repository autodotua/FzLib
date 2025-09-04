using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FzLib.Avalonia.Controls
{
    public interface IProgressOverlayService
    {
        void Register(Action<bool> setVisible, Action<TimeSpan> setDelay, Action<string> setTitle, Action<string> setMessage, Action<bool> setCancelable, Action<ICommand> setCancelCommand);
        void Register(ProgressRingBoxOverlay overlay);
        void Register(ProgressRingOverlay overlay);
        void SetCancelable(bool cancelable);
        void SetCancelCommand(ICommand command);
        void SetDelay(TimeSpan delay);
        void SetMessage(string message);
        void SetTitle(string title);
        void SetVisible(bool visible);
        Task WithOverlayAsync(Func<Task> task, string initialMessage = null, TimeSpan? delay = null);
        Task<bool> WithOverlayAsync(Func<CancellationToken, Task> task, string initialMessage = null, TimeSpan? delay = null);
        Task WithOverlayAsync(Func<Task> task, Func<Task> cancelRequest, string initialMessage = null, TimeSpan? delay = null);
    }
}