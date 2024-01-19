using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;


namespace FzLib.Avalonia.Dialogs
{
    public partial class MessageDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string detail;

        [ObservableProperty]
        private string icon;

        [ObservableProperty]
        private IBrush iconBrush;
    }

    public partial class MessageDialog : CommonDialog
    {
        internal MessageDialog(string title, string message, string detail)
        {
            DataContext = new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail
            };
            InitializeComponent();
        }

    }
}