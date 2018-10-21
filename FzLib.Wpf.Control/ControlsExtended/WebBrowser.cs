using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

namespace FzLib.Wpf.Control.ControlsExtended
{
    public enum WebBrowserEmulationVersion
    {
        IE7 = 7000,
        IE8 = 8888,
        IE9 = 9999,
        IE10 = 10001,
        IE11OrEdge = 11001,
    }
    [ComImport, TypeLibType(TypeLibTypeFlags.FHidden), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
    public interface DWebBrowserEvents2
    {
        [DispId(0x66)]
        void StatusTextChange([In] string text);
        [DispId(0x6c)]
        void ProgressChange([In] int progress, [In] int progressMax);
        [DispId(0x69)]
        void CommandStateChange([In] long command, [In] bool enable);
        [DispId(0x6a)]
        void DownloadBegin();
        [DispId(0x68)]
        void DownloadComplete();
        [DispId(0x71)]
        void TitleChange([In] string text);
        [DispId(0x70)]
        void PropertyChange([In] string szProperty);
        [DispId(250)]
        void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In, Out] ref bool cancel);
        [DispId(0xfb)]
        void NewWindow2([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object pDisp, [In, Out] ref bool cancel);
        [DispId(0xfc)]
        void NavigateComplete2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
        [DispId(0x103)]
        void DocumentComplete([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
        [DispId(0xfd)]
        void OnQuit();
        [DispId(0xfe)]
        void OnVisible([In] bool visible);
        [DispId(0xff)]
        void OnToolBar([In] bool toolBar);
        [DispId(0x100)]
        void OnMenuBar([In] bool menuBar);
        [DispId(0x101)]
        void OnStatusBar([In] bool statusBar);
        [DispId(0x102)]
        void OnFullScreen([In] bool fullScreen);
        [DispId(260)]
        void OnTheaterMode([In] bool theaterMode);
        [DispId(0x106)]
        void WindowSetResizable([In] bool resizable);
        [DispId(0x108)]
        void WindowSetLeft([In] int left);
        [DispId(0x109)]
        void WindowSetTop([In] int top);
        [DispId(0x10a)]
        void WindowSetWidth([In] int width);
        [DispId(0x10b)]
        void WindowSetHeight([In] int height);
        [DispId(0x107)]
        void WindowClosing([In] bool isChildWindow, [In, Out] ref bool cancel);
        [DispId(0x10c)]
        void ClientToHostWindow([In, Out] ref long cx, [In, Out] ref long cy);
        [DispId(0x10d)]
        void SetSecureLockIcon([In] int secureLockIcon);
        [DispId(270)]
        void FileDownload([In, Out] ref bool cancel);
        [DispId(0x10f)]
        void NavigateError([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In, Out] ref bool cancel);
        [DispId(0xe1)]
        void PrintTemplateInstantiation([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp);
        [DispId(0xe2)]
        void PrintTemplateTeardown([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp);
        [DispId(0xe3)]
        void UpdatePageStatus([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object nPage, [In] ref object fDone);
        [DispId(0x110)]
        void PrivacyImpactedStateChange([In] bool bImpacted);
    }

    internal static class ReflectionService
    {
        public readonly static BindingFlags BindingFlags =
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.FlattenHierarchy |
            BindingFlags.CreateInstance;

        public static object ReflectGetProperty(this object target, string propertyName)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("propertyName can not be null or whitespace", "propertyName");

            var propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags);
            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Can not find property '{0}' on '{1}'", propertyName, target.GetType()));
            return propertyInfo.GetValue(target, null);
        }

        public static object ReflectInvokeMethod(this object target, string methodName, Type[] argTypes, object[] parameters)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("methodName can not be null or whitespace", "methodName");

            var methodInfo = target.GetType().GetMethod(methodName, BindingFlags, null, argTypes, null);
            if (methodInfo == null)
                throw new ArgumentException(string.Format("Can not find method '{0}' on '{1}'", methodName, target.GetType()));
            return methodInfo.Invoke(target, parameters);
        }
    }


    public class WebBrowser
    {

        private System.Windows.Controls.WebBrowser _webBrowser;
        private object _cookie;

        public event CancelEventHandler NewWindow;

        public WebBrowser(System.Windows.Controls.WebBrowser webBrowser)
        {
            _webBrowser = webBrowser ?? throw new ArgumentNullException("webBrowser");
            _webBrowser.Dispatcher.BeginInvoke(new Action(Attach), DispatcherPriority.Loaded);
        }

        public void Disconnect()
        {
            if (_cookie != null)
            {
                _cookie.ReflectInvokeMethod("Disconnect", new Type[] { }, null);
                _cookie = null;
            }
        }

        private void Attach()
        {
            var axIWebBrowser2 = _webBrowser.ReflectGetProperty("AxIWebBrowser2");
            var webBrowserEvent = new WebBrowserEvent(this);
            var cookieType = typeof(System.Windows.Controls.WebBrowser).Assembly.GetType("MS.Internal.Controls.ConnectionPointCookie");
            _cookie = Activator.CreateInstance(
                cookieType,
                ReflectionService.BindingFlags,
                null,
                new[] { axIWebBrowser2, webBrowserEvent, typeof(DWebBrowserEvents2) },
                CultureInfo.CurrentUICulture);
        }

        private void OnNewWindow(ref bool cancel)
        {
            var eventArgs = new CancelEventArgs(cancel);
            if (NewWindow != null)
            {
                NewWindow(_webBrowser, eventArgs);
                cancel = eventArgs.Cancel;
            }
        }



        public static bool SetWebBrowserEmulationVersion(string programName, WebBrowserEmulationVersion version)
        {
            string tempPath = System.IO.Path.GetTempFileName();
            StringBuilder str = new StringBuilder();
            str.AppendLine("Windows Registry Editor Version 5.00");
            str.AppendLine();
            str.AppendLine(@"[HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION]");
            str.AppendLine("\"" + programName + "\"=dword:" + Convert.ToString(((int)version), 16));
            str.AppendLine();
            str.AppendLine(@"[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION]");
            str.AppendLine("\"" + programName + "\"=dword:" + Convert.ToString(((int)version), 16));
            File.WriteAllText(tempPath, str.ToString());

            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("regedit.exe", tempPath);
                p.Start();
                p.WaitForExit();

                return true;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return false;
            }

        }

        private void NewWindowOnItself(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            //dynamic browser = sender as System.Windows.Controls.WebBrowser;
            dynamic activeElement = ((dynamic)_webBrowser).Document.activeElement;
            string link = activeElement.ToString();
            _webBrowser.Navigate(link);
        }

        public void SetNewPageMode(bool openOnItself)
        {
            if (openOnItself)
            {
                NewWindow += NewWindowOnItself;

            }
            else
            {
                NewWindow -= NewWindowOnItself;
            }
        }
        //}

        /// <summary>  
        /// 设置浏览器静默，不弹错误提示框  
        /// </summary>  
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>  
        /// <param name="silent">是否静默</param>  
        public static void SetWebBrowserSilent(System.Windows.Controls.WebBrowser webBrowser, bool silent)
        {
            webBrowser.Navigating += (p1, p2) =>
              {
                  FieldInfo fi = typeof(System.Windows.Controls.WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                  if (fi != null)
                  {
                      object browser = fi.GetValue(webBrowser);
                      if (browser != null)
                          browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
                  }
              };

        }

        public static implicit operator WebBrowser(System.Windows.Controls.WebBrowser v)
        {
            throw new NotImplementedException();
        }
        private class WebBrowserEvent : StandardOleMarshalObject, DWebBrowserEvents2
        {
            private WebBrowser _helperInstance = null;

            public WebBrowserEvent(WebBrowser helperInstance)
            {
                _helperInstance = helperInstance;
            }

            #region DWebBrowserEvents2 Members

            public void StatusTextChange(string text)
            {

            }

            public void ProgressChange(int progress, int progressMax)
            {

            }

            public void CommandStateChange(long command, bool enable)
            {

            }

            public void DownloadBegin()
            {

            }

            public void DownloadComplete()
            {

            }

            public void TitleChange(string text)
            {

            }

            public void PropertyChange(string szProperty)
            {

            }

            public void BeforeNavigate2(object pDisp, ref object URL, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
            {

            }

            public void NewWindow2(ref object pDisp, ref bool cancel)
            {
                _helperInstance.OnNewWindow(ref cancel);
            }

            public void NavigateComplete2(object pDisp, ref object URL)
            {

            }

            public void DocumentComplete(object pDisp, ref object URL)
            {

            }

            public void OnQuit()
            {

            }

            public void OnVisible(bool visible)
            {

            }

            public void OnToolBar(bool toolBar)
            {

            }

            public void OnMenuBar(bool menuBar)
            {

            }

            public void OnStatusBar(bool statusBar)
            {

            }

            public void OnFullScreen(bool fullScreen)
            {

            }

            public void OnTheaterMode(bool theaterMode)
            {

            }

            public void WindowSetResizable(bool resizable)
            {

            }

            public void WindowSetLeft(int left)
            {

            }

            public void WindowSetTop(int top)
            {

            }

            public void WindowSetWidth(int width)
            {

            }

            public void WindowSetHeight(int height)
            {

            }

            public void WindowClosing(bool isChildWindow, ref bool cancel)
            {

            }

            public void ClientToHostWindow(ref long cx, ref long cy)
            {

            }

            public void SetSecureLockIcon(int secureLockIcon)
            {

            }

            public void FileDownload(ref bool cancel)
            {

            }

            public void NavigateError(object pDisp, ref object URL, ref object frame, ref object statusCode, ref bool cancel)
            {

            }

            public void PrintTemplateInstantiation(object pDisp)
            {

            }

            public void PrintTemplateTeardown(object pDisp)
            {

            }

            public void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
            {

            }

            public void PrivacyImpactedStateChange(bool bImpacted)
            {

            }

            #endregion
        }



    }
}
