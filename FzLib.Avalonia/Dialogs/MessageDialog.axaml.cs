﻿using Avalonia.Controls.Primitives;
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

    public partial class MessageDialog : DialogHost
    {
        internal static readonly string InfoIcon = "M925.928 337.299c-22.574-53.371-54.884-101.295-96.031-142.443-41.147-41.148-89.072-73.458-142.442-96.032C632.182 75.446 573.487 63.593 513 63.593S393.818 75.446 338.546 98.824c-53.371 22.574-101.295 54.883-142.443 96.031s-73.458 89.073-96.031 142.443C76.694 392.571 64.84 451.265 64.84 511.752c0 60.487 11.854 119.181 35.232 174.454 22.574 53.37 54.883 101.295 96.031 142.443 41.148 41.147 89.072 73.457 142.443 96.031 55.272 23.378 113.966 35.231 174.454 35.231s119.182-11.854 174.454-35.231c53.37-22.574 101.295-54.884 142.442-96.031 41.147-41.148 73.457-89.073 96.031-142.443 23.378-55.272 35.231-113.967 35.231-174.454 0.001-60.487-11.852-119.181-35.23-174.453zM562.707 774.763c0 22.227-5.283 39.015-15.85 50.394-10.567 11.379-23.978 17.084-40.234 17.084s-29.468-5.837-39.621-17.483c-10.168-11.645-15.244-28.315-15.244-49.995V457.368c0-21.946 5.076-38.468 15.244-49.581 10.153-11.113 23.365-16.67 39.621-16.67s29.668 5.557 40.234 16.67c10.567 11.113 15.85 26.557 15.85 46.33v320.646z m-16.256-449.677c-10.84 9.621-23.712 14.424-38.609 14.424-15.443 0-28.648-4.744-39.628-14.217-10.966-9.488-16.456-22.906-16.456-40.242 0-15.709 5.623-28.655 16.87-38.808 11.239-10.168 24.31-15.237 39.214-15.237 14.357 0 27.089 4.596 38.202 13.803 11.106 9.222 16.663 22.626 16.663 40.242 0 17.07-5.424 30.415-16.256 40.035z";
        internal static readonly string WarningIcon = "M955.7 856l-416-720c-6.2-10.7-16.9-16-27.7-16s-21.6 5.3-27.7 16l-416 720C56 877.4 71.4 904 96 904h832c24.6 0 40-26.6 27.7-48zM480 416c0-4.4 3.6-8 8-8h48c4.4 0 8 3.6 8 8v184c0 4.4-3.6 8-8 8h-48c-4.4 0-8-3.6-8-8V416z m32 352c-26.5 0-48-21.5-48-48s21.5-48 48-48 48 21.5 48 48-21.5 48-48 48z";
        internal static readonly string ErrorIcon = "M512 0C229.376 0 0 229.376 0 512s229.376 512 512 512 512-229.376 512-512S794.624 0 512 0z m218.624 672.256c15.872 15.872 15.872 41.984 0 57.856-8.192 8.192-18.432 11.776-29.184 11.776s-20.992-4.096-29.184-11.776L512 569.856l-160.256 160.256c-8.192 8.192-18.432 11.776-29.184 11.776s-20.992-4.096-29.184-11.776c-15.872-15.872-15.872-41.984 0-57.856L454.144 512 293.376 351.744c-15.872-15.872-15.872-41.984 0-57.856 15.872-15.872 41.984-15.872 57.856 0L512 454.144l160.256-160.256c15.872-15.872 41.984-15.872 57.856 0 15.872 15.872 15.872 41.984 0 57.856L569.856 512l160.768 160.256z";
        internal static readonly string QuestionIcon = "M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64z m0 708c-22.1 0-40-17.9-40-40s17.9-40 40-40 40 17.9 40 40-17.9 40-40 40z m62.9-219.5c-18.5 7.1-30.9 25.1-30.9 44.8V620c0 4.4-3.6 8-8 8h-48c-4.4 0-8-3.6-8-8v-21.5c0-23.1 6.7-45.9 19.9-64.9 12.9-18.6 30.9-32.8 52.1-40.9 34-13.1 56-41.6 56-72.7 0-44.1-43.1-80-96-80s-96 35.9-96 80v7.6c0 4.4-3.6 8-8 8h-48c-4.4 0-8-3.6-8-8V420c0-39.3 17.2-76 48.4-103.3C430.4 290.4 470 276 512 276s81.6 14.5 111.6 40.7C654.8 344 672 380.7 672 420c0 57.8-38.1 109.8-97.1 132.5z";
        private readonly MessageDialogButtonDefinition buttonDefinition;

        public MessageDialog(MessageDialogViewModel vm, MessageDialogButtonDefinition buttonDefinition)
        {
            Title = vm.Title;
            DataContext = vm;
            InitializeComponent();
            this.buttonDefinition = buttonDefinition;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            switch (buttonDefinition)
            {
                case MessageDialogButtonDefinition.OK:
                    CloseButtonContent = OkButtonText;
                    break;
                case MessageDialogButtonDefinition.YesNo:
                    PrimaryButtonContent = YesButtonText;
                    SecondaryButtonContent = NoButtonText;
                    break;
                case MessageDialogButtonDefinition.YesNoCancel:
                    PrimaryButtonContent = YesButtonText;
                    SecondaryButtonContent = NoButtonText;
                    CloseButtonContent = CancelButtonText;
                    break;
                case MessageDialogButtonDefinition.RetryCancel:
                    PrimaryButtonContent = RetryButtonText;
                    CloseButtonContent = CancelButtonText;
                    break;
            }

            base.OnApplyTemplate(e);
        }
        protected override void OnPrimaryButtonClick()
        {
            Close(true);
        }
        protected override void OnSecondaryButtonClick()
        {
            Close(false);
        }
        protected override void OnCloseButtonClick()
        {
            Close(null);
        }

        public enum MessageDialogButtonDefinition
        {
            OK,
            YesNo,
            YesNoCancel,
            RetryCancel,
        }
    }
}