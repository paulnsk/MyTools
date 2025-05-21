using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiTools.Av.Interface;

namespace UiTools.Av.Services;

public class YesDesignTime : IDesignTimeDetector
{
    public bool IsDesignTime => true;
}

public class NoDesignTime : IDesignTimeDetector
{
    public bool IsDesignTime => false;
}



public static class DesignTimeDetectorExtensions
{

    public static IServiceCollection AddDesignTimeDetector(this IServiceCollection services)
    {
        if (Design.IsDesignMode) services.AddSingleton<IDesignTimeDetector, YesDesignTime>();
        else services.AddSingleton<IDesignTimeDetector, NoDesignTime>();
        return services;
    }
}