using Avalonia.Controls;
using System;
using System.Collections.Generic;
using Avalonia;


namespace FzLib.Avalonia.Dialogs
{
    internal class CheckBoxDialogContent : ContentControl
    {
        public static readonly StyledProperty<IList<CheckDialogItem>> ItemsProperty =
            AvaloniaProperty.Register<CheckBoxDialogContent, IList<CheckDialogItem>>(nameof(Items));

        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<CheckBoxDialogContent, string>(nameof(Message));

        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<CheckBoxDialogContent, string>(nameof(Title));

        public IList<CheckDialogItem> Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        protected override Type StyleKeyOverride { get; } = typeof(CheckBoxDialogContent);
    }
}