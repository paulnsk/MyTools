using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using UiTools.Av.Demo.ViewModels;
using UiTools.Av.Demo.Views;
using Avalonia.Styling;
using FluentAvalonia.Styling;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using UiTools.Av.Demo.Di;


//todo ���� ��� ��
//todo ���� ��� ����������
//todo ���������� ��� ������������ �� ��
//todo ���� ������ �����?? CommonStyles

namespace UiTools.Av.Demo;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.AddDemoServices();
        var services = collection.BuildServiceProvider();
        var vm = services.GetRequiredService<MainViewModel>();


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
            desktop.MainWindow.WindowState = WindowState.Maximized;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
        //RequestedThemeVariant = ThemeVariant.Light;
        //RequestedThemeVariant = ThemeVariant.Dark;

    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}