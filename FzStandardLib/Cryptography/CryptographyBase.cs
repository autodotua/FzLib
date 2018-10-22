using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Cryptography
{
   public abstract class CryptographyBase:IDisposable
    {
        public  Encoding StringEncoding { get; set; } = Encoding.UTF8;

        public  int BufferLength { get; set; } = 1024 * 1024;

        public abstract void Dispose();
    }
}
