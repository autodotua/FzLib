using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using sys = System;

namespace FzLib.Wpf.Windows
{
   public class PerformanceInformation
    {
        #region 获得内存信息API
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

        //定义内存的信息结构
        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORY_INFO
        {
            public uint dwLength;                           //当前结构体大小
            public uint dwMemoryLoad;                       //当前内存使用率
            public ulong ullTotalPhys;                      //总计物理内存大小
            public ulong ullAvailPhys;                      //可用物理内存大小
            public ulong ullTotalPageFile;                  //总计交换文件大小
            public ulong ullAvailPageFile;                  //总计交换文件大小
            public ulong ullTotalVirtual;                   //总计虚拟内存大小
            public ulong ullAvailVirtual;                   //可用虚拟内存大小
            public ulong ullAvailExtendedVirtual;           //保留 这个值始终为0
        }
        private static MEMORY_INFO GetApiMemoryInfo()
        {
            MEMORY_INFO mi = new MEMORY_INFO();
            mi.dwLength = (uint)Marshal.SizeOf(mi);
            GlobalMemoryStatusEx(ref mi);
            return mi;
        }
        #endregion
        public static MemoryInfo GetMemoryInfo()
        {
            MEMORY_INFO apiMemoryInfo = GetApiMemoryInfo();
            MemoryInfo info = new MemoryInfo(
                (int)apiMemoryInfo.dwLength, 
                (long)apiMemoryInfo.ullTotalPhys, 
                (long)apiMemoryInfo.ullAvailPhys,
                (long)apiMemoryInfo.ullTotalPageFile, 
                (long)apiMemoryInfo.ullAvailPageFile, 
                (long)apiMemoryInfo.ullTotalVirtual, 
                (long)apiMemoryInfo.ullAvailVirtual);
            return info;
        }

        public class MemoryInfo
        {
            public int Bit { get;private set; }
            public long TotalPhysical { get; private set; }
            public long AvailablePhysical { get; private set; }
            public long TotalPageFile { get; private set; }
            public long AvailablePageFile { get; private set; }
            public long TotalVirtual { get; private set; }
            public long AvailableVirtual { get; private set; }

            public MemoryInfo(int bit, long totalPhysical, long availablePhysical, long totalPageFile, long availablePageFile, long totalVirtual, long availableVirtual)
            {
                Bit = bit;
                TotalPhysical = totalPhysical;
                AvailablePhysical = availablePhysical;
                TotalPageFile = totalPageFile;
                AvailablePageFile = availablePageFile;
                TotalVirtual = totalVirtual;
                AvailableVirtual = availableVirtual;
            }
        }
    }
}
