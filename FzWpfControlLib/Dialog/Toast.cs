using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace FzLib.Control.Dialog
{
    internal enum STGM : long
    {
        STGM_READ = 0x00000000L,
        STGM_WRITE = 0x00000001L,
        STGM_READWRITE = 0x00000002L,
        STGM_SHARE_DENY_NONE = 0x00000040L,
        STGM_SHARE_DENY_READ = 0x00000030L,
        STGM_SHARE_DENY_WRITE = 0x00000020L,
        STGM_SHARE_EXCLUSIVE = 0x00000010L,
        STGM_PRIORITY = 0x00040000L,
        STGM_CREATE = 0x00001000L,
        STGM_CONVERT = 0x00020000L,
        STGM_FAILIFTHERE = 0x00000000L,
        STGM_DIRECT = 0x00000000L,
        STGM_TRANSACTED = 0x00010000L,
        STGM_NOSCRATCH = 0x00100000L,
        STGM_NOSNAPSHOT = 0x00200000L,
        STGM_SIMPLE = 0x08000000L,
        STGM_DIRECT_SWMR = 0x00400000L,
        STGM_DELETEONRELEASE = 0x04000000L,
    }

    internal static class ShellIIDGuid
    {
        internal const string IShellLinkW = "000214F9-0000-0000-C000-000000000046";
        internal const string CShellLink = "00021401-0000-0000-C000-000000000046";
        internal const string IPersistFile = "0000010b-0000-0000-C000-000000000046";
        internal const string IPropertyStore = "886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99";
    }

    [ComImport,
    Guid(ShellIIDGuid.IShellLinkW),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellLinkW
    {
        UInt32 GetPath(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
            int cchMaxPath,
            //ref _WIN32_FIND_DATAW pfd,
            IntPtr pfd,
            uint fFlags);
        UInt32 GetIDList(out IntPtr ppidl);
        UInt32 SetIDList(IntPtr pidl);
        UInt32 GetDescription(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
            int cchMaxName);
        UInt32 SetDescription(
            [MarshalAs(UnmanagedType.LPWStr)] string pszName);
        UInt32 GetWorkingDirectory(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
            int cchMaxPath
            );
        UInt32 SetWorkingDirectory(
            [MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        UInt32 GetArguments(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs,
            int cchMaxPath);
        UInt32 SetArguments(
            [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        UInt32 GetHotKey(out short wHotKey);
        UInt32 SetHotKey(short wHotKey);
        UInt32 GetShowCmd(out uint iShowCmd);
        UInt32 SetShowCmd(uint iShowCmd);
        UInt32 GetIconLocation(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszIconPath,
            int cchIconPath,
            out int iIcon);
        UInt32 SetIconLocation(
            [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath,
            int iIcon);
        UInt32 SetRelativePath(
            [MarshalAs(UnmanagedType.LPWStr)] string pszPathRel,
            uint dwReserved);
        UInt32 Resolve(IntPtr hwnd, uint fFlags);
        UInt32 SetPath(
            [MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [ComImport,
    Guid(ShellIIDGuid.IPersistFile),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPersistFile
    {
        UInt32 GetCurFile(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile
        );
        UInt32 IsDirty();
        UInt32 Load(
            [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
            [MarshalAs(UnmanagedType.U4)] STGM dwMode);
        UInt32 Save(
            [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
            bool fRemember);
        UInt32 SaveCompleted(
            [MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
    }
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyStore)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStore
    {
        UInt32 GetCount([Out] out uint propertyCount);
        UInt32 GetAt([In] uint propertyIndex, out PropertyKey key);
        UInt32 GetValue([In] ref PropertyKey key, [Out] PropVariant pv);
        UInt32 SetValue([In] ref PropertyKey key, [In] PropVariant pv);
        UInt32 Commit();
    }


    [ComImport,
    Guid(ShellIIDGuid.CShellLink),
    ClassInterface(ClassInterfaceType.None)]
    internal class CShellLink { }

    // static class ErrorHelper
    //{
    //    public static void VerifySucceeded(UInt32 hresult)
    //    {
    //        if (hresult > 1)
    //        {
    //            throw new Exception("Failed with HRESULT: " + hresult.ToString("X"));
    //        }
    //    }
    //}








    public class Toast
    {

        public Toast(string appName)
        {
            AppName = appName;
            TryCreateShortcut();

        }
        public static void VerifySucceeded(UInt32 hresult)
        {
            if (hresult > 1)
            {
                throw new Exception("HRESULT失败： " + hresult.ToString("X"));
            }
        }


        private string GetShortCutPath()
        {
          return  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\Microsoft\\Windows\\Start Menu\\Programs\\{AppName}.lnk";
        }
        private bool TryCreateShortcut()
        {
            String shortcutPath = GetShortCutPath();
            if (!File.Exists(shortcutPath))
            {
                InstallShortcut(shortcutPath);
                return true;
            }
            return false;
        }

        private void InstallShortcut(String shortcutPath)
        {
            // Find the path to the current executable
            String exePath = Process.GetCurrentProcess().MainModule.FileName;
            IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

            // Create a shortcut to the exe
            VerifySucceeded(newShortcut.SetPath(exePath));
            VerifySucceeded(newShortcut.SetArguments(""));

            // Open the shortcut property store, set the AppUserModelId property
            IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

            using (PropVariant appId = new PropVariant(AppName))
            {
                VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appId));
                VerifySucceeded(newShortcutProperties.Commit());
            }

            // Commit the shortcut to disk
            IPersistFile newShortcutSave = (IPersistFile)newShortcut;

           VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
        }

        public void Show(string text)

        {
            TextLines = new List<string>() { text };
            Show();
        }
        public void Show(string text1, string text2)

        {
            TextLines = new List<string>() { text1, text2 };
            Show();
        }
        public void Show(string text1, string text2, string text3)

        {
            TextLines = new List<string>() { text1, text2, text3 };
            Show();
        }

        public void Show()

        {
            // Get a toast XML template
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(TemplateType);

            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            if (TextLines == null)
            {
                throw new Exception("还未设置显示内容");
            }
            for (int i = 0; i < stringElements.Length; i++)
            {
                //stringElements[i].AppendChild(toastXml.CreateTextNode("Line " + i));
                stringElements[i].AppendChild(toastXml.CreateTextNode(TextLines[i]));
            }

            // Specify the absolute path to an image
            //   String imagePath = "file:///" + Path.GetFullPath("toastImageAndText.png");
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            //       imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;
            if (ImagePath != null)
            {
                imageElements[0].Attributes.GetNamedItem("src").NodeValue = ImagePath;
            }
            // Create the toast and attach event listeners
            Show(toastXml);
        }

        public void Show(XmlDocument xml)
        {
            ToastNotification toast = new ToastNotification(xml);
            toast.Activated += (p1, p2) => ToastActivated?.Invoke(p1, p2);
            toast.Dismissed += (p1, p2) => ToastDismissed?.Invoke(p1, p2);
            toast.Failed += (p1, p2) => ToastFailed?.Invoke(p1, p2);
            Show(toast);
            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
        }

        public void Show(ToastNotification toast)
        {
            ToastNotificationManager.CreateToastNotifier(AppName).Show(toast);

        }

        public bool RemoveShortcut()
        {
            string path = GetShortCutPath();
            if(!File.Exists(path))
            {
                return false;
            }
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public delegate void ToastFailedEventHandler(ToastNotification sender, ToastFailedEventArgs args);
        public delegate void ToastDismissedEventHandler(ToastNotification sender, ToastDismissedEventArgs args);
        public delegate void ToastActivatedEventHandler(ToastNotification sender, object args);

        public event ToastFailedEventHandler ToastFailed;
        public event ToastDismissedEventHandler ToastDismissed;
        public event ToastActivatedEventHandler ToastActivated;

        private string imagePath;
        private string appName;
        private IList<string> textLines;
        private ToastTemplateType templateType = ToastTemplateType.ToastImageAndText04;

        public string ImagePath { get => imagePath; set => imagePath = value; }
        public string AppName
        {
            get
            {
                if (appName == null)
                {
                    throw new NullReferenceException();
                }
                return appName;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }
                if (value.Replace(" ", "") == "")
                {
                    throw new Exception("值为空");
                }
                appName = value;
            }
        }
        public IList<string> TextLines
        {
            get => textLines;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (value.Count == 0)
                {
                    throw new Exception("空列表");
                }
                List<string> lines = new List<string>();
                if (value.Count == 1)
                {
                    lines.Add(value[0]);
                    lines.Add("");
                    lines.Add("");
                }
                else if (value.Count == 2)
                {
                    lines.Add(value[0]);
                    lines.Add(value[1]);
                    lines.Add("");

                }
                else if (value.Count == 3)
                {
                    lines.Add(value[0]);
                    lines.Add(value[1]);
                    lines.Add(value[2]);
                }
                else
                {
                    throw new Exception("列表项过多");
                }
                textLines = lines;
            }
        }

        public ToastTemplateType TemplateType { get => templateType; set => templateType = value; }
    }
}
