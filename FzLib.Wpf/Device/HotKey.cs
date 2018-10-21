using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using static FzLib.Wpf.Device.KeyboardHelper;

namespace FzLib.Wpf.Device
{
    /////<summary>
    ///// 直接构造类实例即可注册
    ///// 自动完成注销
    ///// 注意注册时会抛出异常
    ///// 注册系统热键类
    ///// 热键会随着程序结束自动解除,不会写入注册表
    /////</summary>
    //public class HotKey
    //{
    //    #region Member

    //    private int keyId;         //热键编号
    //    private IntPtr handle;     //窗体句柄
    //    private Window window;     //热键所在窗体
    //    private uint controlKey;   //热键控制键
    //    private uint key;          //热键主键

    //    public delegate void OnHotKeyEventHandler();     //热键事件委托
    //    public event OnHotKeyEventHandler OnHotKey = null;  //热键事件

    //    static Hashtable KeyPair = new Hashtable();         //热键哈希表
    //    private const int WM_HOTKEY = 0x0312;       // 热键消息编号

    //    public enum KeyFlags    //控制键编码
    //    {
    //        MOD_NONE = 0x0,
    //        MOD_ALT = 0x1,
    //        MOD_CONTROL = 0x2,
    //        MOD_SHIFT = 0x4,
    //        MOD_WIN = 0x8
    //    }

    //    #endregion

    //    public HotKey(Window win, KeyFlags control, Key key):this(win, control, (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(key))
    //    {
    //    }


    //    ///<summary>
    //    /// 构造函数
    //    ///</summary>
    //    ///<param name="win">注册窗体</param>
    //    ///<param name="control">控制键</param>
    //    ///<param name="key">主键</param>
    //    public HotKey(Window win, KeyFlags control, System.Windows.Forms.Keys key)
    //    {
    //        handle = new WindowInteropHelper(win).Handle;
    //        window = win;
    //        controlKey = (uint)control;
    //        this.key = (uint)key;
    //        keyId = (int)controlKey + (int)this.key * 10;

    //        if (KeyPair.ContainsKey(keyId))
    //        {
    //            throw new Exception("热键已经被注册!");
    //        }

    //        //注册热键
    //        if (false == RegisterHotKey(handle, keyId, controlKey, this.key))
    //        {
    //            throw new Exception("热键注册失败!");
    //        }

    //        //消息挂钩只能连接一次!!
    //        if (KeyPair.Count == 0)
    //        {
    //            if (false == InstallHotKeyHook(this))
    //            {
    //                throw new Exception("消息挂钩连接失败!");
    //            }
    //        }

    //        //添加这个热键索引
    //        KeyPair.Add(keyId, this);
    //    }

    //    //析构函数,解除热键
    //    //~HotKey()
    //    //{
    //    //    HotKey.UnregisterHotKey(Handle, KeyId);
    //    //}

    //    #region core

    //    [System.Runtime.InteropServices.DllImport("user32")]
    //    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint controlKey, uint virtualKey);

    //    [System.Runtime.InteropServices.DllImport("user32")]
    //    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    //    //安装热键处理挂钩
    //    static private bool InstallHotKeyHook(HotKey hk)
    //    {
    //        if (hk.window == null || hk.handle == IntPtr.Zero)
    //        {
    //            return false;
    //        }

    //        //获得消息源
    //        System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(hk.handle);
    //        if (source == null)
    //        {
    //            return false;
    //        }

    //        //挂接事件
    //        source.AddHook(HotKeyHook);
    //        return true;
    //    }

    //    //热键处理过程
    //    static private IntPtr HotKeyHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    //    {
    //        if (msg == WM_HOTKEY)
    //        {
    //            HotKey hk = (HotKey)KeyPair[(int)wParam];
    //            if (hk.OnHotKey != null)
    //            {
    //                hk.OnHotKey();
    //            }
    //        }
    //        return IntPtr.Zero;
    //    }

    //    #endregion
    //}


    /// <summary>
    /// Setups system-wide hot keys and provides possibility to react on their events.
    /// </summary>
    public class HotKey : IDisposable
    {
        private readonly HwndSource windowHandleSource;

        private readonly Dictionary<HotKeyInfo, int> registered = new Dictionary<HotKeyInfo, int>();

        public HotKeyInfo[] RegisteredKeys => registered.Keys.ToArray();

        /// <summary>
        /// Occurs when registered system-wide hot key is pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey"/> class.
        /// </summary>
        public HotKey()
        {
            windowHandleSource = new HwndSource(new HwndSourceParameters());
            windowHandleSource.AddHook(MessagesHandler);

        }

        /// <summary>
        /// Registers the system-wide hot key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        /// <returns>The registered <see cref="HotKeyInfo"/>.</returns>
        public HotKeyInfo Register(Key key, ModifierKeys modifiers)
        {
            var hotKey = new HotKeyInfo(key, modifiers);
            Register(hotKey);
            return hotKey;
        }

        /// <summary>
        /// Registers the system-wide hot key.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        public void Register(HotKeyInfo hotKey)
        {
            // Check if specified hot key is already registered.
            if (registered.ContainsKey(hotKey))
            {
                throw new ArgumentException("热键已经被注册");
            }

            // Register new hot key.
            var id = GetFreeKeyId();
            if (!WinApi.RegisterHotKey(windowHandleSource.Handle, id, hotKey.Key, hotKey.Modifiers))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "无法注册热键.");
            }

            registered.Add(hotKey, id);
        }

        /// <summary>
        /// Unregisters previously registered hot key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        public void Unregister(Key key, ModifierKeys modifiers)
        {
            var hotKey = new HotKeyInfo(key, modifiers);
            Unregister(hotKey);
        }

        /// <summary>
        /// Unregisters previously registered hot key.
        /// </summary>
        /// <param name="hotKey">The registered hot key.</param>
        public void Unregister(HotKeyInfo hotKey)
        {
            if (registered.TryGetValue(hotKey, out int id))
            {
                WinApi.UnregisterHotKey(windowHandleSource.Handle, id);
                registered.Remove(hotKey);
            }
        }

        public void UnregisterAll()
        {
            foreach (var key in registered)
            {
                WinApi.UnregisterHotKey(windowHandleSource.Handle, key.Value);
            }
            registered.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Unregister hot keys.
            foreach (var hotKey in registered)
            {
                WinApi.UnregisterHotKey(windowHandleSource.Handle, hotKey.Value);
            }

            windowHandleSource.RemoveHook(MessagesHandler);
            windowHandleSource.Dispose();
        }

        private int GetFreeKeyId()
        {
            return registered.Any() ? registered.Values.Max() + 1 : 0;
        }

        private IntPtr MessagesHandler(IntPtr handle, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (message == WinApi.WmHotKey)
            {
                // Extract key and modifiers from the message.
                var key = KeyInterop.KeyFromVirtualKey(((int)lParam >> 16) & 0xFFFF);
                var modifiers = (ModifierKeys)((int)lParam & 0xFFFF);

                var hotKey = new HotKeyInfo(key, modifiers);
                OnKeyPressed(new KeyPressedEventArgs(hotKey));

                handled = true;
                return new IntPtr(1);
            }

            return IntPtr.Zero;
        }

        private void OnKeyPressed(KeyPressedEventArgs e)
        {
            var handler = KeyPressed;
            if (handler != null)
                handler(this, e);
        }

        ///// <summary>
        ///// Represents system-wide hot key.
        ///// </summary>
        //public class HotKeyInfo
        //{
        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="HotKeyInfo"/> class.
        //    /// </summary>
        //    /// <param name="key">The key.</param>
        //    /// <param name="modifiers">The key modifiers.</param>
        //    public HotKeyInfo(Key key, ModifierKeys modifiers)
        //    {
        //        Key = key;
        //        Modifiers = modifiers;
        //    }

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="HotKeyInfo"/> class.
        //    /// </summary>
        //    public HotKeyInfo()
        //    {
        //    }

        //    /// <summary>
        //    /// Gets or sets the key.
        //    /// </summary>
        //    /// <value>
        //    /// The key.
        //    /// </value>
        //    public Key Key { get; set; }

        //    /// <summary>
        //    /// Gets or sets the key modifiers.
        //    /// </summary>
        //    /// <value>
        //    /// The key modifiers.
        //    /// </value>
        //    public ModifierKeys Modifiers { get; set; }

        //    #region Equality members

        //    /// <summary>
        //    /// Determines whether the specified <see cref="HotKeyInfo"/> is equal to this instance.
        //    /// </summary>
        //    /// <param name="other">The <see cref="HotKeyInfo"/> to compare with this instance.</param>
        //    /// <returns>
        //    /// <c>true</c> if the specified <see cref="HotKeyInfo"/> is equal to this instance; otherwise, <c>false</c>.
        //    /// </returns>
        //    public bool Equals(HotKeyInfo other)
        //    {
        //        if (ReferenceEquals(null, other))
        //            return false;

        //        if (ReferenceEquals(this, other))
        //            return true;

        //        return Equals(other.Key, Key) && Equals(other.Modifiers, Modifiers);
        //    }

        //    /// <summary>
        //    /// Determines whether the specified <see cref="object"/> is equal to this instance.
        //    /// </summary>
        //    /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        //    /// <returns>
        //    /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        //    /// </returns>
        //    public override bool Equals(object obj)
        //    {
        //        if (ReferenceEquals(null, obj))
        //            return false;

        //        if (ReferenceEquals(this, obj))
        //            return true;

        //        return obj.GetType() == typeof(HotKeyInfo) && Equals((HotKeyInfo)obj);
        //    }

        //    /// <summary>
        //    /// Returns a hash code for this instance.
        //    /// </summary>
        //    /// <returns>
        //    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        //    /// </returns>
        //    public override int GetHashCode()
        //    {
        //        unchecked
        //        {
        //            return (Key.GetHashCode() * 397) ^ Modifiers.GetHashCode();
        //        }
        //    }

        //    public override string ToString()
        //    {
        //        var sb = new StringBuilder();
        //        if (Modifiers .HasFlag( ModifierKeys.Control))
        //        {
        //            sb.Append("Ctrl + ");
        //        }
        //        if (Modifiers.HasFlag(ModifierKeys.Shift))
        //        {
        //            sb.Append("Shift + ");
        //        }

        //        if (Modifiers.HasFlag(ModifierKeys.Alt))
        //        {
        //            sb.Append("Alt + ");
        //        }
        //        if ((Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
        //        {
        //            sb.Append("Win + ");
        //        }
        //        sb.Append(Key.ToString());
        //        return sb.ToString();
        //    }



        //    #endregion
        //}
        /// <summary>
        /// Arguments for key pressed event which contain information about pressed hot key.
        /// </summary>
        public class KeyPressedEventArgs : EventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeyPressedEventArgs"/> class.
            /// </summary>
            /// <param name="hotKey">The hot key.</param>
            public KeyPressedEventArgs(HotKeyInfo hotKey)
            {
                HotKey = hotKey;
            }

            /// <summary>
            /// Gets the hot key.
            /// </summary>
            public HotKeyInfo HotKey { get; private set; }
        }
        /// <summary>
        /// Contains imported Win32 API functions, constants and helper methods.
        /// </summary>
        class WinApi
        {
            /// <summary>
            /// Registers a system-wide hot key.
            /// </summary>
            /// <param name="handle">The handle of the window that will process hot key messages.</param>
            /// <param name="id">The hot key ID.</param>
            /// <param name="key">The key.</param>
            /// <param name="modifiers">The key modifiers.</param>
            /// <returns><c>true</c> if hot key was registered; otherwise, <c>false</c>.</returns>
            public static bool RegisterHotKey(IntPtr handle, int id, Key key, ModifierKeys modifiers)
            {
                var virtualCode = KeyInterop.VirtualKeyFromKey(key);
                return RegisterHotKey(handle, id, (uint)modifiers, (uint)virtualCode);
            }

            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool RegisterHotKey(IntPtr handle, int id, uint modifiers, uint virtualCode);

            /// <summary>
            /// Unregisters previously registered system-wide hot key.
            /// </summary>
            /// <param name="handle">The window handle.</param>
            /// <param name="id">The hot key ID.</param>
            /// <returns></returns>
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool UnregisterHotKey(IntPtr handle, int id);

            /// <summary>
            /// The message code posted when the user presses a hot key.
            /// </summary>
            public static int WmHotKey = 0x0312;
        }
        public class HotKeyInfo : IEquatable<HotKeyInfo>
        {
            public HotKeyInfo(Key key, ModifierKeys modifierKeys = ModifierKeys.None)
            {
                Key = key;
                Modifiers = modifierKeys;
            }
            public HotKeyInfo()
            {
            }
            public Key Key { get; set; }

            public ModifierKeys Modifiers { get; set; }

            public override bool Equals(object obj)
            {
                return obj is HotKey && Equals((HotKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((int)Key * 397) ^ (int)Modifiers;
                }
            }

            public bool Equals(HotKeyInfo other)
            {
                return Key == other.Key && Modifiers == other.Modifiers;
            }

#pragma warning disable 618
            public override string ToString()
            {
                var sb = new StringBuilder();
                if ((Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                {
                    sb.Append(GetLocalizedKeyStringUnsafe(Constants.VK_MENU));
                    sb.Append("+");
                }
                if ((Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    sb.Append(GetLocalizedKeyStringUnsafe(Constants.VK_CONTROL));
                    sb.Append("+");
                }
                if ((Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    sb.Append(GetLocalizedKeyStringUnsafe(Constants.VK_SHIFT));
                    sb.Append("+");
                }
                if ((Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
                {
                    sb.Append("Windows+");
                }
                sb.Append(GetLocalizedKeyString(Key));
                return sb.ToString();
            }

        }

    }
}
