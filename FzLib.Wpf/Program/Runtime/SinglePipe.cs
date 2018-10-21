using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Wpf.Program.Runtime
{
    public abstract class SinglePipe
    {

        public class Clinet
        {
            public string PipeName { get; private set; }
            public Clinet(string pipeName)
            {
                if (string.IsNullOrEmpty(pipeName) )
                {
                    throw new Exception("名称为空");
                }
                PipeName = pipeName;



            }


            NamedPipeClientStream pipeStream;
            public void Start()
            {
                
                Task.Run(async () =>
                {
                    while (true)
                    {
                        pipeStream = new NamedPipeClientStream(PipeName);
                        await pipeStream.ConnectAsync();
                        using (StreamReader rdr = new StreamReader(pipeStream))
                        {
                            string result;
                         
                            result = await rdr.ReadLineAsync();
                           // Debug.WriteLine("1");
                            if (result == "\0")
                            {
                                Debug.Write("\\0");
                                break;
                            }
                            GotMessage.Invoke(this, new PipeMessageEventArgs(result));
                            //Debug.WriteLine("结果：" + result);

                        }
                    }

                });
            }

            public void Dispose()
            {
                try
                {

                    pipeStream.Dispose();
                }
                catch
                {
                    Debug.WriteLine("无法结束");
                }
            }

            public delegate void GotMessageHandler(object sender, PipeMessageEventArgs e);
            public event GotMessageHandler GotMessage;
        }

        public class Server
        {
            public string PipeName { get; private set; }
            StreamWriter writer;
            private NamedPipeServerStream pipeStream;
            public Server(string pipeName)
            {
            
                if (string.IsNullOrEmpty(pipeName))
                {
                    throw new Exception("名称为空");
                }
                PipeName = pipeName;


                pipeStream = new NamedPipeServerStream(pipeName);

            }

            public async Task SendMessageAsync(string message)
            {
                await pipeStream.WaitForConnectionAsync();
                writer = new StreamWriter(pipeStream);

                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
                pipeStream.Disconnect();

            }

            public async Task StopClinetAsync()
            {
                await SendMessageAsync("\0");
            }

            public void Dispose()
            {
                pipeStream.Dispose();
            }
        }



        public class PipeMessageEventArgs : EventArgs
        {
            public PipeMessageEventArgs(string message)
            {
                Message = message;
            }

            public string Message { get; set; }
        }



    }
}




// if(WpfCodes.Program.Startup.HaveAnotherInstance("F2I"))
//            {
//                WpfCodes.Program.Pipe pipe = new WpfCodes.Program.Pipe("F2I", WpfCodes.Program.Pipe.Type.Server);
//pipe.SendMessage("OpenWindow");
//                Application.Current.Shutdown();
//            }
//            else
//            {
//                WpfCodes.Program.Pipe pipe = new WpfCodes.Program.Pipe("F2I", WpfCodes.Program.Pipe.Type.Client);
//pipe.GotMessage += (p1, p2) =>
//                  {
//                      if (p2.Message == "OpenWindow")
//                      {
//                          Dispatcher.Invoke(() =>
//                          {
//                              MainWindow window = new MainWindow();
//window.Show();
//                              window.Activate();
//                          });
//                      }
//                  };
//            }
