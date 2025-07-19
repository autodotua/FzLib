using CommunityToolkit.Mvvm.ComponentModel;
using FzLib;
using System;
using System.ComponentModel;

namespace FzLib.Avalonia.Dialogs
{
    public abstract partial class DialogItem : ObservableObject
    {
        public object Tag { get; set; }
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string detail;


        public DialogItem(string title, string detail = null)
        {
            Title = title;
            Detail = detail;
        }

        public DialogItem()
        {
        }
    }

    public partial class SelectDialogItem : DialogItem
    {
        public SelectDialogItem(string title, string detail, Action selectAction) : base(title, detail)
        {
            SelectAction = selectAction;
        }

        public SelectDialogItem(string title, string detail = null) : base(title, detail)
        {
        }

        [ObservableProperty]
        private Action selectAction;
    }

    public partial class CheckDialogItem : DialogItem
    {
        public CheckDialogItem(string title, string detail = null) : base(title, detail)
        {
        }

        public CheckDialogItem(string title, string detail, bool isEnabled, bool isChecked) : base(title, detail)
        {
            IsEnabled = isEnabled;
            IsChecked = isChecked;
        }

        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private bool isEnabled = true;

    }
}