using System.ComponentModel;
using System.Windows.Controls;

namespace FzLib.WPF.Bases
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