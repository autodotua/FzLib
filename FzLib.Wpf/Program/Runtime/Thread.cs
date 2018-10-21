using System;
using System.Security.Permissions;
using System.Windows.Threading;


namespace FzLib.Wpf.Program.Runtime
{
    public class Thread
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            try
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback((p1) => { ((DispatcherFrame)frame).Continue = false; return null; }), frame);
                Dispatcher.PushFrame(frame);
            }
            catch (InvalidOperationException) { }
        }
        
    }
}
