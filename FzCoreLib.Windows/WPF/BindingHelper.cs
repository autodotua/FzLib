using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF
{
    public static class BindingHelper
    {
        public static void PauseBinding(this FrameworkElement obj, DependencyProperty property, Action action)
        {
            var binding = obj.GetBindingExpression(property);
            BindingOperations.ClearBinding(obj, property);
            action();
            obj.SetBinding(property, binding.ParentBindingBase);
        }

        public static async Task PauseBindingAsync(this FrameworkElement obj, DependencyProperty property, Func<Task> action)
        {
            var binding = obj.GetBindingExpression(property);
            BindingOperations.ClearBinding(obj, property);
            await action();
            obj.SetBinding(property, binding.ParentBindingBase);
        }
    }
}