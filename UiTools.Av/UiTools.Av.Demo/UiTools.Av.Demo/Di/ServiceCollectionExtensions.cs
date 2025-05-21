using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiTools.Av.Demo.ViewModels;
using UiTools.Av.Services;

namespace UiTools.Av.Demo.Di;

public static class ServiceCollectionExtensions
{
    public static void AddDemoServices(this IServiceCollection collection)
    {
        collection.AddTransient<ADialogService>();
        collection.AddTransient<MainViewModel>();
    }
}