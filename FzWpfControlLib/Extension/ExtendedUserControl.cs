using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FzLib.Control.Extension
{
  public  class ExtendedUserControl : UserControl, INotifyPropertyChanged
    {
        public ExtendedUserControl()
        {
            DataContext = this;
        }

        protected void Notify(params string[] names)
        {
            foreach (var name in names)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void SetValueAndNotify<T>(ref T field, T value, params string[] names)
        {
            field = value;
            Notify(names);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
