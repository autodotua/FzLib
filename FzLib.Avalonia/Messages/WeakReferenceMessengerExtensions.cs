using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using FzLib.Avalonia.Dialogs;
using System;
using System.ComponentModel;

namespace FzLib.Avalonia.Messages
{
    public static class WeakReferenceMessengerExtensions
    {
        public static void RegisterDialogHostMessage(this Visual visual)
        {
            WeakReferenceMessenger.Default.Register(visual, async (object _, DialogHostMessage m) =>
            {
                try
                {
                    var result = await m.Dialog.ShowDialog<object>(DialogContainerType.PopupPreferred,
                        global::Avalonia.Controls.TopLevel.GetTopLevel(visual));
                    m.SetResult(result);
                }
                catch (Exception ex)
                {
                    m.SetException(ex);
                }
            });
        }
    }
}