﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;

namespace FzLib.Avalonia.Dialogs
{
    public abstract class CommonDialogWindow : Window
    {
        internal CommonDialogWindow()
        {
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = global::Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
            ExtendClientAreaTitleBarHeightHint = -1;
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            MinHeight = 120;
            MinWidth = 320;
            MaxWidth = 800;
            MaxHeight = 800;
            Padding = new Thickness(16);
            Loaded += CommonDialogWindow_Loaded;
        }

        public bool CloseOnButtonClick { get; set; } = true;

        private void CommonDialogWindow_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Content is DialogWrapper dw)
            {
                PrimaryButton = dw.PrimaryButton;
                SecondaryButton = dw.SecondaryButton;
                CloseButton = dw.CloseButton;
            }
            else
            {
                throw new Exception($"{nameof(CommonDialogWindow)}的{nameof(Content)}必须为{nameof(DialogWrapper)}");
            }
            PrimaryButton.Click += Button_Click;
            SecondaryButton.Click += Button_Click;
            CloseButton.Click += Button_Click;
        }

        private void Button_Click(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (CloseOnButtonClick)
            {
                if (sender == PrimaryButton)
                {
                    Close(CommonDialogButtonType.Primary);
                }
                else if(sender == SecondaryButton)
                {
                    Close(CommonDialogButtonType.Secondary);
                }
                else if (sender == CloseButton)
                {
                    Close(CommonDialogButtonType.Close);
                }
            }
        }


        private Button PrimaryButton;
        private Button SecondaryButton;
        private Button CloseButton;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
        }
    }
}