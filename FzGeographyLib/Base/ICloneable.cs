using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Geography.Base
{
    interface ICloneable<T>
    {
        T Clone();
    }
}
