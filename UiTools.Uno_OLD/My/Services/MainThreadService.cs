using System;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;



namespace UiTools.Uno.My.Services;


//originally from Smarter project, heavily modified
public class MainThreadService
{

    /// <summary>
    /// For non-DI environments. Must still be initialized (e.g. from MainPage's ctor) by calling "_ = new MainThreadService();"
    /// When not initialized properly, a stub instance will be created for the purpose of throwing a "not initialized" error rather than a nullreference.
    /// </summary>
    public static MainThreadService Instance = new();

    /// <summary>
    /// Must be called from the main ui thread which is done when the service is registered as singleton from App() ctor.
    /// Call it explicetly: services.AddSingleton(new MainThreadService());
    /// </summary>
    public MainThreadService()
    {
        Instance = this;
        _mainThreadDispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }


    public void EnqueueOnMainThread(DispatcherQueueHandler action)
    {
        if (!MainThreadDispatcherQueue.TryEnqueue(action)) throw new Exception($"{nameof(MainThreadService)}: Unable to enqueue!");
    }

    public Task EnqueueOnMainThreadAndWait(Func<Task> function)
    {
        //despite the name, Toolkit's EnqueueAsync() returns a task which represent the execution of the actual function being "enqueued"
        //I.e. awaiting this won't return until function() returns
        return MainThreadDispatcherQueue.EnqueueAsync(function);
    }

    private readonly DispatcherQueue _mainThreadDispatcherQueue;


    private DispatcherQueue MainThreadDispatcherQueue
    {
        get
        {
            if (_mainThreadDispatcherQueue == default) throw new Exception(nameof(MainThreadService) + " not initialized!");
            return _mainThreadDispatcherQueue;
        }
    }


}
