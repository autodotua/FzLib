using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using FzLib.Application;

namespace FzLib.Programming;

public static class TcpSingleInstanceHelper
{
    public const string ActivateCommand = "ACTIVATE";

    private static CancellationTokenSource cts;

    private static TcpListener listener;

    static TcpSingleInstanceHelper()
    {
        TcpPort = CalculatePortFromAssemblyName();
    }

    public static TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(0.2);

    public static int TcpPort { get; private set; }

    public static void Dispose()
    {
        try
        {
            cts?.Cancel();
            listener?.Stop();
            listener = null;
            cts?.Dispose();
        }
        catch
        {
            // 忽略清理时的错误
        }
    }

    public static bool EnsureSingleInstance(Func<Task> onActivated = null)
    {
        if (TryNotifyExistingInstance(TcpPort))
        {
            return false; // 已有实例，退出
        }

        StartListener(TcpPort, onActivated);
        return true; // 是主实例
    }

    public static async Task<bool> EnsureSingleInstanceAsync(Func<Task> onActivated = null)
    {
        if (await TryNotifyExistingInstanceAsync(TcpPort))
        {
            return false; // 已有实例，退出
        }

        StartListener(TcpPort, onActivated);
        return true; // 是主实例
    }

    private static int CalculatePortFromAssemblyName()
    {
        string name = ApplicationInfo.ProgramFilePath;

        byte[] hash = SHA1.HashData(Encoding.UTF8.GetBytes(name));
        int value = BitConverter.ToInt32(hash, 0);
        value = Math.Abs(value); // 去符号
        int port = 10000 + (value % (65535 - 10000)); // 保证端口 > 10000 且 < 65535
        return port;
    }

    private static void StartListener(int port, Func<Task> onActivated)
    {
        cts = new CancellationTokenSource();
        listener = new TcpListener(IPAddress.Loopback, port);
        listener.Start();

        _ = Task.Run(async () =>
        {
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var client = await listener.AcceptTcpClientAsync(cts.Token);
                    using var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
                    string line = await reader.ReadLineAsync();
                    if (line == ActivateCommand)
                    {
                        if (onActivated != null)
                        {
                            await onActivated();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    // 忽略单次错误，继续监听
                }
            }
        }, cts.Token);
    }

    private static bool TryNotifyExistingInstance(int port)
    {
        try
        {
            using var tcpClient = new TcpClient();
            var task = tcpClient.ConnectAsync("127.0.0.1", port);
            if (!task.Wait(ConnectTimeout))
            {
                return false; // 超时
            }
            using var writer = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
            writer.WriteLine(ActivateCommand);
            writer.Flush();
            return true;
        }
        catch
        {
            return false;
        }
    }


    private static async Task<bool> TryNotifyExistingInstanceAsync(int port)
    {
        try
        {
            using var tcpClient = new TcpClient();
            var connectTask = tcpClient.ConnectAsync("127.0.0.1", port);
            if (await Task.WhenAny(connectTask, Task.Delay(ConnectTimeout)) != connectTask)
            {
                return false; // 超时
            }
            await connectTask;

            using var writer = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
            await writer.WriteLineAsync(ActivateCommand);
            await writer.FlushAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

}