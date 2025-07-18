using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using System.Linq;

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
}