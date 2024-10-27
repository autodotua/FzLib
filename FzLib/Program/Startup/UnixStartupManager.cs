using System;
using System.IO;

namespace FzLib.Program.Startup
{
    public class UnixStartupManager : IStartupManager
    {
        private string GetAutostartFilePath(string appName)
        {
            string autostartPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "autostart");
            return Path.Combine(autostartPath, $"{appName}.desktop");
        }

        public void EnableStartup(string appName, string executablePath, string arguments = "")
        {
            string desktopFilePath = GetAutostartFilePath(appName);

            string desktopFileContent = $"[Desktop Entry]\n" +
                                        "Type=Application\n" +
                                        $"Name={appName}\n" +
                                        $"Exec=\"{executablePath}\" {arguments}\n" +
                                        "X-GNOME-Autostart-enabled=true\n";

            Directory.CreateDirectory(Path.GetDirectoryName(desktopFilePath));
            File.WriteAllText(desktopFilePath, desktopFileContent);
        }

        public void DisableStartup(string appName)
        {
            string desktopFilePath = GetAutostartFilePath(appName);
            if (File.Exists(desktopFilePath))
            {
                File.Delete(desktopFilePath);
            }
        }

        public bool IsStartupEnabled(string appName)
        {
            string desktopFilePath = GetAutostartFilePath(appName);
            return File.Exists(desktopFilePath);
        }
    }
}