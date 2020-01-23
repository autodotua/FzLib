using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FzLib.Device
{
    public static class KeyboardHelper
    {
#pragma warning restore 618

        public static string GetLocalizedKeyString(Key key)
        {
            if (key >= Key.BrowserBack && key <= Key.LaunchApplication2)
            {
                return key.ToString();
            }

            var vkey = KeyInterop.VirtualKeyFromKey(key);
            return GetLocalizedKeyStringUnsafe(vkey) ?? key.ToString();
        }

#pragma warning disable 618
        public static string GetLocalizedKeyStringUnsafe(int key)
        {
            // strip any modifier keys
            long keyCode = key & 0xffff;

            var sb = new StringBuilder(256);

            long scanCode = NativeMethods.MapVirtualKey((uint)keyCode, NativeMethods.MapType.MAPVK_VK_TO_VSC);

            // shift the scancode to the high word
            scanCode = (scanCode << 16);
            if (keyCode == 45 ||
                keyCode == 46 ||
                keyCode == 144 ||
                (33 <= keyCode && keyCode <= 40))
            {
                // add the extended key flag
                scanCode |= 0x1000000;
            }

            var resultLength = UnsafeNativeMethods.GetKeyNameText((int)scanCode, sb, 256);
            return resultLength > 0 ? sb.ToString() : null;
        }
#pragma warning restore 618
        
        public static void SendString(string value)
        {
            global::System.Windows.Forms.SendKeys.SendWait( value);
        }
        
    }
}
