using Microsoft.Win32;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace FzLib.Application;

/// <summary>
/// 用户和设备的唯一识别码（ChatGPT生成）
/// </summary>
public static class IdentityProvider
{
    private static readonly Lazy<string> combinedId = new Lazy<string>(InitCombinedId, true);

    private static readonly string FallbackFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "machine.id");

    // 缓存字段
    private static readonly Lazy<string> machineId = new Lazy<string>(InitMachineId, true);

    private static readonly Lazy<string> userId = new Lazy<string>(InitUserId, true);

    /// <summary>
    /// 获取机器码 + 用户码 的组合唯一ID
    /// </summary>
    public static string GetCombinedId() => combinedId.Value;

    /// <summary>
    /// 获取跨平台机器唯一ID（机器码）
    /// </summary>
    public static string GetMachineId() => machineId.Value;

    /// <summary>
    /// 获取跨平台用户唯一ID（用户码）
    /// </summary>
    public static string GetUserId() => userId.Value;
    #region 初始化方法（仅执行一次）

    private static string InitCombinedId()
    {
        string raw = GetMachineId() + "|" + GetUserId();
        return HashToHex(raw);
    }

    private static string InitMachineId()
    {
        string id = string.Empty;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            id = GetWindowsMachineGuid();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            id = GetLinuxMachineId();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            id = GetMacOSMachineId();
        }

        if (string.IsNullOrWhiteSpace(id))
        {
            id = GetMacAddress();
        }

        if (string.IsNullOrWhiteSpace(id))
        {
            id = GetOrCreateFallbackId();
        }

        return HashToHex(id);
    }

    private static string InitUserId()
    {
        string userName = Environment.UserName;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                string sid = identity.User?.Value ?? "UnknownSID";
                return $"{sid}:{userName}";
            }
            catch
            {
                return $"UnknownSID:{userName}";
            }
        }
        else
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "id",
                    Arguments = "-u",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                using var proc = Process.Start(psi);
                string uid = proc.StandardOutput.ReadToEnd().Trim();
                proc.WaitForExit();
                return $"{uid}:{userName}";
            }
            catch
            {
                return $"UnknownUID:{userName}";
            }
        }
    }
    #endregion

    #region 内部工具方法

    private static string GetLinuxMachineId()
    {
        try
        {
            if (File.Exists("/etc/machine-id"))
            {
                return File.ReadAllText("/etc/machine-id").Trim();
            }
            if (File.Exists("/var/lib/dbus/machine-id"))
            {
                return File.ReadAllText("/var/lib/dbus/machine-id").Trim();
            }
        }
        catch
        {
        }
        return string.Empty;
    }

    private static string GetMacAddress()
    {
        try
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic =>
                    nic.OperationalStatus == OperationalStatus.Up &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static string GetMacOSMachineId()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ioreg",
                Arguments = "-rd1 -c IOPlatformExpertDevice",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            using var proc = Process.Start(psi);
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            var marker = "IOPlatformUUID";
            int idx = output.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
            {
                int start = output.IndexOf('"', idx + marker.Length) + 1;
                int end = output.IndexOf('"', start);
                return output.Substring(start, end - start);
            }
        }
        catch
        {
        }
        return string.Empty;
    }

    private static string GetOrCreateFallbackId()
    {
        try
        {
            if (File.Exists(FallbackFilePath))
            {
                return File.ReadAllText(FallbackFilePath).Trim();
            }

            string newId = Guid.NewGuid().ToString();
            Directory.CreateDirectory(Path.GetDirectoryName(FallbackFilePath)!);
            File.WriteAllText(FallbackFilePath, newId);
            return newId;
        }
        catch
        {
            return Guid.NewGuid().ToString();
        }
    }

    [SupportedOSPlatform("windows")]
    private static string GetWindowsMachineGuid()
    {
        try
        {
            return Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography",
                "MachineGuid",
                string.Empty
            )?.ToString() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
    private static string HashToHex(string input)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hash);
    }

    #endregion
}
