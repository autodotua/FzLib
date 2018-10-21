using ControlzEx.Native;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using static FzLib.Wpf.Device.KeyboardHelper;

namespace FzLib.Wpf.Device
{
    public class KeyboardHook
    {
        private enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public struct KBDLLHOOKSTRUCT
        {
            public UInt32 vkCode;
            public UInt32 scanCode;
            public UInt32 flags;
            public UInt32 time;
            public IntPtr extraInfo;
        }

        private readonly int WM_KEYDOWN = 0x100;
        private readonly int WM_KEYUP = 0x101;

        private readonly int WM_SYSKEYDOWN = 0x0104;
        private readonly int WM_SYSKEYUP = 0x105;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain.
        /// </summary>
        /// <param name="idHook">The type of hook procedure to be installed.</param>
        /// <param name="lpfn">Reference to the hook callback method.</param>
        /// <param name="hMod">A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated with the current process.</param>
        /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated. If this parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread.</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure. If the function fails, the return value is NULL. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(HookType idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">A handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hhk">This parameter is ignored.</param>
        /// <param name="nCode">The hook code passed to the current hook procedure. The next hook procedure uses this code to determine how to process the hook information.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.</param>
        /// <returns>This value is returned by the next hook procedure in the chain. The current hook procedure must also return this value. The meaning of the return value depends on the hook type. For more information, see the descriptions of the individual hook procedures.</returns>
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        /// <summary>
        /// An application-defined or library-defined callback function used with the SetWindowsHookEx function. The system calls this function whenever an application calls the GetMessage or PeekMessage function and there is a keyboard message (WM_KEYUP or WM_KEYDOWN) to be processed.
        /// </summary>
        /// <param name="code">A code the hook procedure uses to determine how to process the message. If code is less than zero, the hook procedure must pass the message to the CallNextHookEx function without further processing and should return the value returned by CallNextHookEx.</param>
        /// <param name="wParam">The virtual-key code of the key that generated the keystroke message.</param>
        /// <param name="lParam">The repeat count, scan code, extended-key flag, context code, previous key-state flag, and transition-state flag. For more information about the lParam parameter, see Keystroke Message Flags.</param>
        /// <returns>If code is less than zero, the hook procedure must return the value returned by CallNextHookEx. If code is greater than or equal to zero, and the hook procedure did not process the message, it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise bad stuff.</returns>
        private delegate int HookProc(int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        public string Name { get; private set; }


        public bool IsPaused
        {
            get
            {
                return ispaused;
            }
            set
            {
                if (value != ispaused && value == true)
                    StopHook();

                if (value != ispaused && value == false)
                    StartHook();

                ispaused = value;
            }
        }
        private bool ispaused = false;

        public delegate void KeyDownEventDelegate(object sender, KeyboardHookEventArgs e);
        public KeyDownEventDelegate KeyDown;

        public delegate void KeyUpEventDelegate(object sender, KeyboardHookEventArgs e);
        public KeyUpEventDelegate KeyUp;

        HookProc hookproc;
        IntPtr hhook;


        public KeyboardHook(string name)
        {
            Name = name;
            StartHook();
        }

        private void StartHook()
        {
            Trace.WriteLine(string.Format("Starting hook '{0}'...", Name), string.Format("Hook.StartHook [{0}]", Thread.CurrentThread.Name));

            hookproc = new HookProc(HookCallback);
            hhook = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, hookproc, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
            if (hhook == null || hhook == IntPtr.Zero)
            {
                Win32Exception LastError = new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private void StopHook()
        {
            Trace.WriteLine(string.Format("Stopping hook '{0}'...", Name), string.Format("Hook.StartHook [{0}]", Thread.CurrentThread.Name));

            UnhookWindowsHookEx(hhook);
        }

        private int HookCallback(int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            int result = 0;

            try
            {
                if (!IsPaused && code >= 0)
                {
                    if (wParam.ToInt32() == WM_SYSKEYDOWN || wParam.ToInt32() == WM_KEYDOWN)
                        KeyDown?.Invoke(this, new KeyboardHookEventArgs(lParam));

                    if (wParam.ToInt32() == WM_SYSKEYUP || wParam.ToInt32() == WM_KEYUP)
                        KeyUp?.Invoke(this, new KeyboardHookEventArgs(lParam));
                }
            }
            finally
            {
                result = CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);
            }

            return result;
        }

        ~KeyboardHook()
        {
            StopHook();
        }


        public class KeyboardHookEventArgs : EventArgs
        {
            [DllImport("user32.dll")]
            static extern short GetKeyState(VirtualKeyStates nVirtKey);

            private enum VirtualKeyStates : int
            {
                VK_LWIN = 0x5B,
                VK_RWIN = 0x5C,
                VK_LSHIFT = 0xA0,
                VK_RSHIFT = 0xA1,
                VK_LCONTROL = 0xA2,
                VK_RCONTROL = 0xA3,
                VK_LALT = 0xA4, //aka VK_LMENU
                VK_RALT = 0xA5, //aka VK_RMENU
            }

            private const int KEY_PRESSED = 0x8000;

            public Keys WinFormKey { get; private set; }
            public Key Key { get; private set; }

            public bool IsAltPressed => IsLeftAltPressed || IsRightAltPressed;
            public bool IsLeftAltPressed { get; private set; }
            public bool IsRightAltPressed { get; private set; }

            public bool IsCtrlPressed => IsLeftCtrlPressed || IsRightCtrlPressed;
            public bool IsLeftCtrlPressed { get; private set; }
            public bool IsRightCtrlPressed { get; private set; }

            public bool IsShiftPressed => IsLeftShiftPressed || IsRightShiftPressed;
            public bool IsLeftShiftPressed { get; private set; }
            public bool IsRightShiftPressed { get; private set; }

            public bool IsWinPressed => IsLeftWinPressed || IsRightWinPressed;
            public bool IsLeftWinPressed { get; private set; }
            public bool IsRightWinPressed { get; private set; }

            internal KeyboardHookEventArgs(KBDLLHOOKSTRUCT para)
            {
                WinFormKey = (Keys)para.vkCode;
                Key = KeyInterop.KeyFromVirtualKey((int)WinFormKey);

                //Control.ModifierKeys doesn't capture alt/win, and doesn't have r/l granularity
                IsLeftAltPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LALT) & KEY_PRESSED) || WinFormKey == Keys.LMenu;
                IsRightAltPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_RALT) & KEY_PRESSED) || WinFormKey == Keys.RMenu;

                IsLeftCtrlPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LCONTROL) & KEY_PRESSED) || WinFormKey == Keys.LControlKey;
                IsRightCtrlPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_RCONTROL) & KEY_PRESSED) || WinFormKey == Keys.RControlKey;

                IsLeftShiftPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LSHIFT) & KEY_PRESSED) || WinFormKey == Keys.LShiftKey;
                IsRightShiftPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_RSHIFT) & KEY_PRESSED) || WinFormKey == Keys.RShiftKey;

                IsLeftWinPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LWIN) & KEY_PRESSED) || WinFormKey == Keys.LWin;
                IsRightWinPressed = Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_RWIN) & KEY_PRESSED) || this.WinFormKey == Keys.RWin;

                if (new[] { Keys.LMenu, Keys.RMenu, Keys.LControlKey, Keys.RControlKey, Keys.LShiftKey, Keys.RShiftKey, Keys.LWin, Keys.RWin }.Contains(this.WinFormKey))
                    this.WinFormKey = Keys.None;
            }

            public override string ToString()
            {
                {
                    var sb = new StringBuilder();
                    if (IsAltPressed)
                    {
                        sb.Append(GetLocalizedKeyStringUnsafe(Constants.VK_MENU));
                        sb.Append("+");
                    }
                    if (IsCtrlPressed)
                    {
                        sb.Append(GetLocalizedKeyStringUnsafe(Constants.VK_CONTROL));
                        sb.Append("+");
                    }
                    if (IsShiftPressed)
                    {
                        sb.Append(GetLocalizedKeyStringUnsafe(Constants.VK_SHIFT));
                        sb.Append("+");
                    }
                    if (IsWinPressed)
                    {
                        sb.Append("Windows+");
                    }
                    if(Key!=Key.LWin && Key != Key.RWin
                        && Key != Key.LeftCtrl && Key != Key.RightCtrl
                        && Key != Key.LeftShift && Key != Key.RightShift
                        && Key != Key.LeftAlt && Key != Key.RightAlt)
                    {
                        sb.Append(GetLocalizedKeyString(Key));
                    }
                    else
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    return sb.ToString();
                }
            }

        }

    }
}
