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
                        case CommonDialogMessage.CommonDialogType.ErrorRetry:
                            if (m.Exception == null)
                            {
                                result = await visual.ShowErrorDialogAsync(m.Title, m.Message, m.Detail, true);
                            }
                            else
                            {
                                result = await visual.ShowErrorDialogAsync(m.Title, m.Exception, true);
                            }

                            break;
                        case CommonDialogMessage.CommonDialogType.YesNo:
                            result = await visual.ShowYesNoDialogAsync(m.Title, m.Message, m.Detail);
                            break;
                        case CommonDialogMessage.CommonDialogType.Unknown:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    m.SetResult(result);
                }
                catch (Exception ex)
                {
                    m.SetException(ex);
                }
            });
        }

        public static void RegisterInputDialogMessage(this Visual visual)
        {
            WeakReferenceMessenger.Default.Register<InputDialogMessage>(visual, async (_, m) =>
            {
                try
                {
                    object result = m.Type switch
                    {
                        InputDialogMessage.InputDialogType.Text => await visual.ShowInputTextDialogAsync(m.Title,
                            m.Message, m.DefaultValue as string, m.Watermark, m.Validation),
                        InputDialogMessage.InputDialogType.Integer => await visual.ShowInputNumberDialogAsync(m.Title,
                            m.Message, (int)m.DefaultValue, m.Watermark),
                        InputDialogMessage.InputDialogType.Float => await visual.ShowInputNumberDialogAsync(m.Title,
                            m.Message, (double)m.DefaultValue, m.Watermark),
                        InputDialogMessage.InputDialogType.Password => await visual.ShowInputPasswordDialogAsync(
                            m.Title, m.Message, m.Watermark, m.Validation),
                        InputDialogMessage.InputDialogType.MultipleLinesText => await
                            visual.ShowInputMultiLinesTextDialogAsync(m.Title, m.Message, 3, 10,
                                m.DefaultValue as string, m.Watermark, m.Validation),
                        _ => throw new ArgumentOutOfRangeException()
                    };
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
                    var result = await m.Dialog.ShowDialog<object>(DialogContainerType.PopupPreferred,
                        TopLevel.GetTopLevel(visual));
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
            WeakReferenceMessenger.Default.Register<GetStorageProviderMessage>(visual,
                (_, m) => { m.StorageProvider = TopLevel.GetTopLevel(visual).StorageProvider; });
        }

        public static void RegisterGetClipboardMessage(this Visual visual)
        {
            WeakReferenceMessenger.Default.Register<GetClipboardMessage>(visual,
                (_, m) => { m.Clipboard = TopLevel.GetTopLevel(visual).Clipboard; });
        }
    }
}