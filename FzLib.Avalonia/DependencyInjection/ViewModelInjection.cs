using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FzLib.Avalonia.DependencyInjection
{
    public class ViewModelInjection
    {
        private static IServiceProvider serviceProvider;

        public static IServiceProvider ServiceProvider => serviceProvider;

        public static void Register(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ViewModelInjection.serviceProvider = serviceProvider;
        }

        public static readonly AttachedProperty<Type> ViewModelTypeProperty =
            AvaloniaProperty.RegisterAttached<ViewModelInjection, Control, Type>("ViewModelType", coerce: (d, v) =>
            {
                if (serviceProvider == null)
                {
                    throw new ArgumentNullException(nameof(serviceProvider));
                }

                if (d is Control c)
                {
                    c.DataContext = serviceProvider.GetRequiredService(v);
                }

                return v;
            });

        public static void SetViewModelType(Control obj, Type value) => obj.SetValue(ViewModelTypeProperty, value);
        public static Type GetViewModelType(Control obj) => obj.GetValue(ViewModelTypeProperty);
    }
}
