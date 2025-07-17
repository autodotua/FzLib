using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Programming
{
    public interface ISingleObject<T>
    {
        T SingleObject { get; set; }
    }
}