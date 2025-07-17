using System;

namespace FzLib.IO.Pipes
{
    public class PipeMessageEventArgs : EventArgs
    {
        public PipeMessageEventArgs(string message) => Message = message;

        public string Message { get; }
    }
}