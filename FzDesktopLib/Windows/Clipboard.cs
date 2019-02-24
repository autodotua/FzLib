using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FzLib.Windows
{
    public static class Clipboard
    {

        public static IDataObject currentData;

        static Clipboard()
        {
            ClipboardMonitor c = new ClipboardMonitor();
            c.ClipboardUpdate += (p1, p2) =>
              {
                  currentData = global::System.Windows.Clipboard.GetDataObject();
                  ClipboardChanged?.Invoke(p1, new ClipboardEventArgs(currentData));
              };
        }

        public static string[] GetContainedFormats()
        {
            return global::System.Windows.Clipboard.GetDataObject().GetFormats();
        }

        public static bool IsFormatContained(string format)
        {
            return global::System.Windows.Clipboard.GetDataObject().GetDataPresent(format);
        }
        public static T GetData<T>(string dataFormat)
        {
            return GetData<T>(dataFormat, currentData);
        }

        public static T GetData<T>(string dataFormat, IDataObject dataObj)
        {
            try
            {
                object data = dataObj.GetData(dataFormat);
                if (data is T)
                {
                    return (T)data;
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
        public static T GetData<T>()
        {
            return (GetData<T>(currentData));
        }
        public static T GetData<T>(IDataObject dataObj)
        {
            try
            {
                object data = dataObj.GetData(typeof(T));
                if (data is T)
                {
                    return (T)data;
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        //public static string[] ContainsDataFormats
        //{
        //    get
        //    {
        //        string[] formats = new string[]
        //    {
        //        DataFormats.Text,
        //        DataFormats.XamlPackage,
        //        DataFormats.Xaml,
        //        DataFormats.Serializable,
        //        DataFormats.StringFormat,
        //        DataFormats.CommaSeparatedValue,
        //        DataFormats.Rtf,
        //        DataFormats.Html,
        //        DataFormats.Locale,
        //        DataFormats.FileDrop,
        //        DataFormats.WaveAudio,
        //        DataFormats.Riff,
        //        DataFormats.Palette,
        //        DataFormats.OemText,
        //        DataFormats.Tiff,
        //        DataFormats.Dif,
        //        DataFormats.SymbolicLink,
        //        DataFormats.MetafilePicture,
        //        DataFormats.EnhancedMetafile,
        //        DataFormats.Bitmap,
        //        DataFormats.Dib,
        //        DataFormats.UnicodeText,
        //        DataFormats.PenData,
        //    };
        //        List<string> containsFormats = new List<string>();
        //        foreach (var format in formats)
        //        {
        //            if (System.Windows.Clipboard.ContainsData(format))
        //            {
        //                containsFormats.Add(format);
        //            }
        //        }
        //        return containsFormats.ToArray();
        //    }
        //}

        public delegate void ClipboardEventHandler(object sender, ClipboardEventArgs e);
        public static event ClipboardEventHandler ClipboardChanged;



        public class ClipboardMonitor
        {
            private const int WM_CLIPBOARDUPDATE = 0x031D;

            private IntPtr windowHandle;

            /// <summary>
            /// Event for clipboard update notification.
            /// </summary>
            public event EventHandler ClipboardUpdate;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="window">Main window of the application.</param>
            /// <param name="start">Enable clipboard notification on startup or not.</param>
            public ClipboardMonitor(Window window = null, bool start = true)
            {
                if (window == null)
                {
                    window = new Window();
                }
                windowHandle = new WindowInteropHelper(window).EnsureHandle();
                HwndSource.FromHwnd(windowHandle)?.AddHook(HwndHandler);
                if (start) Start();
            }

            /// <summary>
            /// Enable clipboard notification.
            /// </summary>
            public void Start()
            {
                NativeMethods.AddClipboardFormatListener(windowHandle);
            }

            /// <summary>
            /// Disable clipboard notification.
            /// </summary>
            public void Stop()
            {
                NativeMethods.RemoveClipboardFormatListener(windowHandle);
            }

            private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
            {
                if (msg == WM_CLIPBOARDUPDATE)
                {
                    this.ClipboardUpdate?.Invoke(this, new EventArgs());
                }
                handled = false;
                return IntPtr.Zero;
            }

            private static class NativeMethods
            {
                [DllImport("user32.dll", SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool AddClipboardFormatListener(IntPtr hwnd);

                [DllImport("user32.dll", SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);
            }
        }

    }

    public class ClipboardEventArgs : EventArgs
    {
        public IDataObject Data { get; private set; }

        public ClipboardEventArgs(IDataObject data)
        {
            Data = data ?? throw new NullReferenceException();
        }

        public string[] ContainedDataFormats => Data.GetFormats();

        public string Text
        {
            get
            {
                if (Data.GetDataPresent(DataFormats.UnicodeText))
                {
                    return Clipboard.GetData<string>(DataFormats.UnicodeText, Data);
                }
                else if (Data.GetDataPresent(DataFormats.Text))
                {
                    return Clipboard.GetData<string>(DataFormats.Text, Data);
                }
                else if (Data.GetDataPresent(DataFormats.OemText))
                {
                    return Clipboard.GetData<string>(DataFormats.OemText, Data);
                }
                return null;
            }
        }

        public string[] Files
        {
            get
            {
                if (Data.GetDataPresent(DataFormats.FileDrop))
                {
                    return Clipboard.GetData<string[]>(DataFormats.FileDrop, Data);
                }
                return null;
            }
        }

        public Bitmap Image
        {
            get
            {
                if (Data.GetDataPresent(DataFormats.Bitmap))
                {
                    return Clipboard.GetData<Bitmap>(Data);
                }
                return null;
            }
        }

        public string Html
        {
            get
            {
                if (Data.GetDataPresent(DataFormats.Html))
                {
                    return Clipboard.GetData<string>(DataFormats.Html, Data);
                }
                return null;
            }
        }

        public string Rtf
        {
            get
            {
                if (Data.GetDataPresent(DataFormats.Rtf))
                {
                    return Clipboard.GetData<string>(DataFormats.Rtf, Data);
                }
                return null;
            }
        }

        public string Csv
        {
            get
            {
                if (Data.GetDataPresent(DataFormats.CommaSeparatedValue))
                {
                    return Clipboard.GetData<string>(DataFormats.CommaSeparatedValue, Data);
                }
                return null;
            }
        }
    }
}
