using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace UiTools.Av.Services;


// Originally from Smarter project, heavily modified; ported from uitools.uno to avalonia
public class MainThreadService
{
    /// <summary>
    /// For non-DI environments.
    /// </summary>
    public static MainThreadService Instance = new();

    /// <summary>
    /// For uno this had to be called explicitly: services.AddSingleton(new MainThreadService()); for Avalonia, it no longer matters.
    /// </summary>
    public MainThreadService()
    {
        Instance = this;
        _mainThreadDispatcher = Dispatcher.UIThread;
    }

    public void EnqueueOnMainThread(Action action)
    {
        if (!_mainThreadDispatcher.CheckAccess())
        {
            _mainThreadDispatcher.Post(action, DispatcherPriority.Normal);
        }
        else
        {
            action();
        }
    }

    public Task EnqueueOnMainThreadAndWait(Func<Task> function)
    {
        return _mainThreadDispatcher.InvokeAsync(function, DispatcherPriority.Normal);
    }

    private readonly Dispatcher _mainThreadDispatcher;
    
}