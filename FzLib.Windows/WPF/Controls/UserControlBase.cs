using System.ComponentModel;
using System.Windows.Controls;

namespace FzLib.WPF.Controls
{
    public abstract class UserControlBase : UserControl, INotifyPropertyChanged
    {
        public UserControlBase()
        {
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}