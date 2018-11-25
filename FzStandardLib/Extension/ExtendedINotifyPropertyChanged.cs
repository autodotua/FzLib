using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FzLib.Extension
{
    public abstract class ExtendedINotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(params string[] names)
        {
            foreach (var name in names)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void SetValueAndNotify<T>(ref T field,T value,params string[] names)
        {
            field = value;
            Notify(names);
        }
    }
}
