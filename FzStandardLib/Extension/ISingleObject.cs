using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Extension
{
   public interface ISingleObject<T>
    {
        T SingleObject { get; set; }
    }
}
