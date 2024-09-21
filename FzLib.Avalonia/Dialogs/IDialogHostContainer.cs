using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using System.Linq;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    public static class DialogHostContainerExtension
    {
        public static Border FindThumb<TContainer>(this TContainer visual) where TContainer : Visual, IDialogHostContainer
        {
            return visual.GetVisualDescendants()
                .FirstOrDefault(p => p.Name == "PART_Thumb") as Border;
        }
    }
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