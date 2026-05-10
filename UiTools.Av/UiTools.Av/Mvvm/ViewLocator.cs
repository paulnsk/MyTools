using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Diagnostics;
using System.Linq;

namespace UiTools.Av.Mvvm;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);


        var stopwatch = Stopwatch.StartNew();

        // Search all assemblies, not just the one ViewLocator lives in. This takes about .1 ms so as long as we don't use this for repeated searches we are ok
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .Select(a => a.GetType(name))
            .FirstOrDefault(t => t != null);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is LocatableViewModelBase;
    }
}