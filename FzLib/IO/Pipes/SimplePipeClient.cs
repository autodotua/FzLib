using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace FzLib.IO.Pipes
{
    public class SimplePipeClient : IDisposable
    {
        private CancellationTokenSource cts;
        public SimplePipeClient(string pipeName)
        {
            PipeName = pipeName ?? throw new ArgumentNullException(nameof(pipeName));
        }

        public event EventHandler<PipeMessageEventArgs> MessageReceived;

        public string PipeName { get; }
        public void Dispose() => cts?.Cancel();

        public async Task ReceiveMessagesAsync()
        {
            while (!cts.IsCancellationRequested)
            {
                using (var pipeStream = new NamedPipeClientStream(PipeName))
                {
                    await pipeStream.ConnectAsync(cts.Token);
                    using (var reader = new StreamReader(pipeStream))
                    {
                        var message = await reader.ReadLineAsync();
                        if (message == "\0") break;
                        MessageReceived?.Invoke(this, new PipeMessageEventArgs(message));
                    }
                }
            }
        }

        public void Start() => cts = new CancellationTokenSource();
    }
}