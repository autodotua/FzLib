using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;


namespace FzLib.Avalonia.Dialogs
{
    public partial class InputDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private string watermark;

        [ObservableProperty]
        private bool multiLines;

        [ObservableProperty]
        private char passwordChar='\0';

        [ObservableProperty]
        private double minHeight;

        [ObservableProperty]
        private int maxLines;

        [ObservableProperty]
        private string text;
    }

    public partial class InputDialog : CommonDialogWindow
    {
        internal InputDialog(InputDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        private void DialogWindow_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            txt.Focus();
            if(!string.IsNullOrEmpty((DataContext as InputDialogViewModel).Text))
            {
                txt.SelectAll();
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            (Content as DialogWrapper).PrimaryButtonContent = DialogWrapper.OkButtonText;
            (Content as DialogWrapper).CloseButtonContent = DialogWrapper.CancelButtonText;

            base.OnApplyTemplate(e);
        }
    }
}