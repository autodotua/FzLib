using Avalonia.Controls;
using System.Threading.Tasks;
using System.Windows;
namespace FzLib.Avalonia.Controls
{
    public static class ControlExtension
    {
        /// <summary>
        /// 等待元素加载完成
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Task WaitForLoadedAsync(this Control control)
        {
            if (control.IsLoaded)
            {
                return Task.CompletedTask;
            }
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            control.Loaded += Control_Loaded;
            return tcs.Task;
            void Control_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
            {
                control.Loaded -= Control_Loaded;
                tcs.TrySetResult(0);
            }
        }
    }
}