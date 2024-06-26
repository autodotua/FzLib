using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using FzLib.Avalonia.Dialogs;
using System;

namespace FzLib.Avalonia.Messages
{
    public static class WeakReferenceMessengerExtensions
    {
        public static void RegisterCommonDialogMessage(this Visual visual)
        {
            WeakReferenceMessenger.Default.Register<CommonDialogMessage>(visual, async (_, m) =>
            {
                try
                {
                    object result = null;
                    switch (m.Type)
                    {
                        case CommonDialogMessage.CommonDialogType.Ok:
                            await visual.ShowOkDialogAsync(m.Title, m.Message, m.Detail);
                            break;
                        case CommonDialogMessage.CommonDialogType.Error:
                            if (m.Exception == null)
                            {
                                result = await visual.ShowErrorDialogAsync(m.Title, m.Message, m.Detail);
                            }
                            else
                            {
                                result = await visual.ShowErrorDialogAsync(m.Title, m.Exception);
                            }
                            break;
                        case CommonDialogMessage.CommonDialogType.YesNo:
                            result = await visual.ShowYesNoDialogAsync(m.Title, m.Message, m.Detail);
                            break;
                    }
                    m.SetResult(result);
                }
                catch (Exception ex)
                {
                    m.SetException(ex);
                }
            });
        }

        public static void RegisterDialogHostMessage(this Visual visual)
        {
            WeakReferenceMessenger.Default.Register<DialogHostMessage>(visual, async (_, m) =>
            {
                try
                {
                    var result = await m.Dialog.ShowDialog<object>(DialogContainerType.PopupPreferred, TopLevel.GetTopLevel(visual));
                    m.SetResult(result);
                }
                catch (Exception ex)
                {
                    m.SetException(ex);
                }
            });
        }

        public static void RegisterGetStorageProviderMessage(this Visual visual)
        {
            WeakReferenceMessenger.Default.Register<GetStorageProviderMessage>(visual, (_, m) =>
            {
                m.StorageProvider = TopLevel.GetTopLevel(visual).StorageProvider;
            });
        }

        public static void RegisterGetClipboardMessage(this Visual visual)
        {
            WeakReferenceMessenger.Default.Register<GetClipboardMessage>(visual, (_, m) =>
            {
                m.Clipboard = TopLevel.GetTopLevel(visual).Clipboard;
            });
        }
    }
}
