using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace FzLib.IO.Pipes
{
    public class SimplePipeServer : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly NamedPipeServerStream pipeStream;
        public SimplePipeServer(string pipeName)
        {
            PipeName = pipeName ?? throw new ArgumentNullException(nameof(pipeName));
            pipeStream = new NamedPipeServerStream(pipeName);
        }

        public string PipeName { get; }
        public void Dispose()
        {
            cts.Cancel();
            pipeStream.Dispose();
        }

        public async Task SendAsync(string message)
        {
            await pipeStream.WaitForConnectionAsync(cts.Token);
            try
            {
                using (var writer = new NonClosingStreamWriter(pipeStream))
                {
                    await writer.WriteLineAsync(message);
                    await writer.FlushAsync();
                }
            }
            finally
            {
                pipeStream.Disconnect();
            }
        }

        private class NonClosingStreamWriter : StreamWriter
        {
            public NonClosingStreamWriter(Stream stream) : base(stream) { }

            protected override void Dispose(bool disposing)
            {
                base.Flush();  // 确保数据刷新
                               // 不调用 base.Dispose() 以避免关闭底层流
            }
        }
    }
}