using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace UiTools.Uno.My.Services.SimpleNavigation;

public class SimpleNavigator<TShellView, TShellViewModel>(IServiceProvider services) where TShellView : IShellView<TShellViewModel>
{
    
    public TShellView? ShellView { get; set; }

    /*
     *
     * - ALL views and viewmodels need to be registered (transient is fine) with DI. No paring between views and VMs (naming conventions are used)
     *
     * - Navigator itself need not be registered with DI. Use factory instead. Becauuse we want it to be a perfect singleton and .net is likely to mess it up
     *
     *     
     * - Views and viewmodels need to be in the same assembly
     *
     *
     * Naming conventions:
     *  - views are called ExamplePage
     *  - viewModels are called ExampleViewModel
     *  - viewModels are located in <Whatever>.ViewModels namespace
     *  - views are located in <SameAsViewModels>.Views namespace
     */


    private object? _currentViewModel;
    private IView? _currentView;

    
    public void NavigateToViewModel<TViewModel>() where TViewModel : notnull
    {
        if (ShellView == null) throw new Exception($"{nameof(ShellView)} is not assigned. Assign it from your shell view constructor");

        if (_currentViewModel?.GetType() == typeof(TViewModel)) return;
        
        


        // calculating view type using naming conventions
        var viewModelFullTypeName = typeof(TViewModel).FullName;
        var viewModelShortTypeName = typeof(TViewModel).Name;
        var viewShortTypeName = viewModelShortTypeName.Replace("viewmodel", "Page", StringComparison.OrdinalIgnoreCase);
        var viewTypeFullName = viewModelFullTypeName!.Replace("viewmodels." + viewModelShortTypeName, "Views." + viewShortTypeName, StringComparison.OrdinalIgnoreCase);
        var viewType = typeof(TViewModel).Assembly.GetType(viewTypeFullName);
        if (viewType == null) throw new Exception($"Unable to find view of type {viewTypeFullName} for viewModel of type {viewModelFullTypeName}. Check your naming");

        // cleaning up old viewmodel and view
        (_currentViewModel as INavigationAware)?.OnNavigatedFrom();
        // *** Avoid using IDisposable because it causes memory leak. a reference to the object remains in ServiceProvider's scope an it does not seem to want to clean up transient objects until the scope is closed, which it never is in a desktop app
        (_currentViewModel as IDisposable)?.Dispose();
        _currentView = null;
        _currentViewModel = null;

        // getting a new 
        var viewModel = services.GetRequiredService<TViewModel>();
        _currentViewModel = viewModel;

        var view = services.GetRequiredService(viewType);
        _currentView = view as IView;
        (view as IView<TViewModel>)!.Vm = viewModel;

        ShellView.NavigatorViewPort.VerticalContentAlignment = VerticalAlignment.Stretch;
        ShellView.NavigatorViewPort.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        ShellView.NavigatorViewPort.Content = _currentView;

        (viewModel as INavigationAware)?.OnNavigatedTo();

    }


}
