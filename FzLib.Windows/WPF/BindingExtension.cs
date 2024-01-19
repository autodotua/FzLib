using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF
{
    public static class BindingExtension
    {
        /// <summary>
        /// 暂时取消绑定并完成操作
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="action"></param>
        public static void PauseBinding(this FrameworkElement obj, DependencyProperty property, Action action)
        {
            var binding = obj.GetBindingExpression(property);
            BindingOperations.ClearBinding(obj, property);
            action();
            obj.SetBinding(property, binding.ParentBindingBase);
        }

        /// <summary>
        /// 暂时取消绑定并完成操作
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task PauseBindingAsync(this FrameworkElement obj, DependencyProperty property, Func<Task> action)
        {
            var binding = obj.GetBindingExpression(property);
            BindingOperations.ClearBinding(obj, property);
            await action();
            obj.SetBinding(property, binding.ParentBindingBase);
        }
    }
}