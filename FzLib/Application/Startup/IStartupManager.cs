namespace FzLib.Application.Startup
{
    public interface IStartupManager
    {
        void EnableStartup(string appName, string executablePath, string arguments = "");
        void DisableStartup(string appName);
        bool IsStartupEnabled(string appName);
    }
}