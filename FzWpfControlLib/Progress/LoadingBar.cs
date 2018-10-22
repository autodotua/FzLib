using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Control.Progress
{
  public  class LoadingBar : MahApps.Metro.Controls.MetroProgressBar
    {
        public LoadingBar()
        {
            IsIndeterminate = true;
        }
    }
}
