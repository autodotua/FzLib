using Avalonia.Controls;
using System.Threading.Tasks;
using System.Windows;
namespace FzLib.Avalonia
{
    public static class ControlExtension
    {
        /// <summary>
        /// 等待元素加载完成
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public async static Task WaitForLoadedAsync(this Control control)
        {
            if (control.IsLoaded)
            {
                return;
            }
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            control.Loaded += Control_Loaded;
            await tcs.Task;
            void Control_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
            {
                control.Loaded -= Control_Loaded;
                tcs.TrySetResult(0);
            }
        }


    }
}