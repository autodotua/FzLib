using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace FzLib.Application.Startup
{
    public class WindowsStartupManager : IStartupManager
    {
        private const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public void EnableStartup(string appName, string executablePath, string arguments = "")
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);
            key?.SetValue(appName, $"\"{executablePath}\" {arguments}");
        }

        public void DisableStartup(string appName)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);
            key?.DeleteValue(appName, false);
        }

        public bool IsStartupEnabled(string appName)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, false);
            return key?.GetValue(appName) != null;
        }
    }
}