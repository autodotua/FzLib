using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Program
{
   public static class Information
    {
        static Information()
        {
            ProgramFilePath = Process.GetCurrentProcess().MainModule.FileName;
            ProgramDirectoryPath = Path.GetDirectoryName(ProgramFilePath);
            ProgramName = AppDomain.CurrentDomain.FriendlyName.Split('.')[0];
        }
        public static string WorkingDirectory
        {
            get => Directory.GetCurrentDirectory();
            set => Directory.SetCurrentDirectory(value);
        }

        public static string ProgramFilePath { get; } 
        public static string ProgramDirectoryPath { get; } 

        public static string ProgramName { get; } 

    }
}
