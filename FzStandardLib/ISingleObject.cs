using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib
{
    public interface ISingleObject<T>
    {
        T SingleObject { get; set; }
    }
}