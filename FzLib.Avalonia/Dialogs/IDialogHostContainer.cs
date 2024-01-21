using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    public interface IDialogHostContainer
    {
        void Close();
        void Close(object result);
    }
    public interface IDialogHostContainer<TContainer> : IDialogHostContainer
    {
        Task ShowDialog(TContainer container, DialogHost dialogHost);
        Task<T> ShowDialog<T>(TContainer container, DialogHost dialogHost);
    }
}