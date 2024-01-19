using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.WPF.Dialog
{
    public class WindowOwner
    {
        public WindowOwner(bool auto = true)
        {
            Auto = auto;
        }

        public WindowOwner(Window window)
        {
            owner = window;
        }

        public bool Auto { get; }
        private Window owner;

        public Window Owner
        {
            get
            {
                if (Auto)
                {
                    return Application.Current?.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                }
                else
                {
                    return owner;
                }
            }
        }
    }
}