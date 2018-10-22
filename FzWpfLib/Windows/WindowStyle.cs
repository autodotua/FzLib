using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace FzLib.Windows
{
    public class WindowStyle
    {
        public WindowStyle(Window win)
        {
            winHandle = new WindowInteropHelper(win).Handle;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        public const int GWL_EXSTYLE = (-20);
        private readonly IntPtr winHandle;
        public void SetStickOnDesktop(bool overDesktopIcons)
        {
            if (overDesktopIcons)
            {
                IntPtr pWnd = FindWindow("Progman", null);
                pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
                pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
                SetParent(winHandle, pWnd);
            }
            else
            {
                SetParent(winHandle, GetWorkerWindow());
            }
        }

        public bool SetToMouseThrough()
        {
            return Set(winHandle, GWL_EXSTYLE, WindowModes.Transparent | WindowModes.ToolWindow);
        }
        public bool SetToNormal()
        {
            return Set(winHandle, GWL_EXSTYLE, 0);
        }
        public static bool Set(IntPtr handle, int type, int style)
        {
            return SetWindowLong(handle, type, style) != 0;
        }
        public static bool Set(IntPtr handle, int type, WindowModes style)
        {
            return Set(handle, type, (int)style);
        }
        public bool Set(int type, int style)
        {
            return Set(winHandle, type, style);
        }
        public bool Set(int style)
        {
            return Set(winHandle, GWL_EXSTYLE, style);
        }
        public bool Set(WindowModes style)
        {
            return Set(winHandle, GWL_EXSTYLE, style);
        }
        public bool Reset()
        {
            return Set(winHandle, GWL_EXSTYLE, 0);
        }

        public static IntPtr GetWorkerWindow()
        {
            // 获取
            IntPtr windowHandle = FindWindow("Progman", null);

            IntPtr zero;
            // 重要消息 生成一个WorkerW 顶级窗口 桌面列表会随之搬家
            SendMessageTimeout(windowHandle, 0x52c, new IntPtr(0), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out zero);
            IntPtr workerw = IntPtr.Zero;
            // 消息会生成两个WorkerW 顶级窗口 所以要枚举不包含“SHELLDLL_DefView”这个的 WorkerW 窗口 隐藏掉。
            EnumWindows(delegate (IntPtr tophandle, IntPtr topparamhandle)
            {
                if (FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    workerw = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                }
                return true;
            }, IntPtr.Zero);
            ShowWindow(workerw, SW_HIDE);
            return windowHandle;
        }

        /// <summary>
        /// 枚举窗口
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);


        /// <summary>
        /// 显示窗口异步
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">显示方式</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        public const int SW_SHOW = 5;
        public const int SW_HIDE = 0;
        public const int WS_SHOWNORMAL = 1;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_ABORTIFHUNG = 2,
            SMTO_BLOCK = 1,
            SMTO_ERRORONEXIT = 0x20,
            SMTO_NORMAL = 0,
            SMTO_NOTIMEOUTIFNOTHUNG = 8
        }


        [Flags]
        public enum WindowModes
        {
            Normal = 0x00000000,
            /// <summary>
            /// 总在最上
            /// </summary>
            TopMost = 0x00000008,
            /// <summary>
            /// 透明显示，鼠标穿透
            /// </summary>
            Transparent = 0x00000020,
            /// <summary>
            /// 工具窗口
            /// </summary>
            ToolWindow = 0x00000080,
            /// <summary>
            /// 从右到左
            /// </summary>
            RightToLeft = 0x00001000,
            /// <summary>
            /// 也是从右到左
            /// </summary>
            RightDing = 0x00002000,
            /// <summary>
            /// 滚动条在左
            /// </summary>
            LeftScroll = 0x00004000,
            /// <summary>
            /// 有一个最大化按钮。 不能与WS_EX_CONTEXTHELP样式组合。 还必须指定WS_SYSMENU样式。
            /// </summary>
            MaximizeBox = 0x00010000,
            /// <summary>
            /// 设置一组窗口的第一个
            /// </summary>
            Group = 0x00020000,
            /// <summary>
            /// 拥有调整尺寸的边框
            /// </summary>
            SizeBox = 0x00040000,
            /// <summary>
            /// 拥有窗口菜单
            /// </summary>
            SystemMenu = 0x00080000,
            /// <summary>
            /// 拥有水平滚动条
            /// </summary>
            HorizontalScroll = 0x00100000,
            /// <summary>
            /// 拥有垂直滚动条
            /// </summary>
            VerticalScroll = 0x00200000,
            /// <summary>
            /// 对话框样式，无标题栏
            /// </summary>
            DialogFrame = 0x00400000,
            /// <summary>
            /// 细线边框
            /// </summary>
            Border = 0x00800000,
            /// <summary>
            /// 有一个标题栏
            /// </summary>
            Caption = Border | DialogFrame,
            /// <summary>
            /// 用于父窗口，不绘制子窗口
            /// </summary>
            ClipChildren = 0x02000000,
            /// <summary>
            /// 用于父窗口，不绘制子窗口重叠部分
            /// </summary>
            ClipSibLings = 0x04000000,
            /// <summary>
            /// 禁用
            /// </summary>
            Disable = 0x08000000,
            /// <summary>
            /// 最大化
            /// </summary>
            Maximize = 0x01000000,
            /// <summary>
            /// 最小化
            /// </summary>
            Minimize = 0x20000000,
            /// <summary>
            /// 子窗口。 这种风格的窗口不能有菜单栏。 该样式不能与WS_POPUP样式一起使用
            /// </summary>
            Child = 0x40000000,
        }
    }
}
