using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static void AddViewAndViewModel<TView,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TViewModel>(
        this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TView : Control, new()
        where TViewModel : class
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TViewModel>();
                    services.AddSingleton(s => new TView()
                    { DataContext = s.GetRequiredService<TViewModel>() });

                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient<TViewModel>();
                    services.AddTransient(s => new TView()
                    { DataContext = s.GetRequiredService<TViewModel>() });

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

    }
}
