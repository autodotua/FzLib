using ModernWpf.FzExtension.CommonDialog;
using System;
using System.Windows.Input;
using System.Threading.Tasks;

namespace FzLib.WpfDemo
{
    public abstract class PanelButtonCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool canExecute = true;

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public abstract void Execute(object parameter);

        protected async void Do(Action action)
        {
            try
            {
                canExecute = false;
                CanExecuteChanged?.Invoke(this, new EventArgs());
                await Task.Run(() =>
                 {
                     action();
                 });
                canExecute = true;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                await CommonDialog.ShowErrorDialogAsync(ex);
            }
        }

        protected async void Do(Func<Task> task)
        {
            try
            {
                canExecute = false;
                CanExecuteChanged?.Invoke(this, new EventArgs());
                await task();
                canExecute = true;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                await CommonDialog.ShowErrorDialogAsync(ex);
            }
        }
    }
}