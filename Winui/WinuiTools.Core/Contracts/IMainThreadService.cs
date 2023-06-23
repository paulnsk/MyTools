using System;
using System.Threading.Tasks;

namespace WinuiTools.Core.Contracts
{
    public interface IMainThreadService
    {
        void RunOnMainThread(Action action);

        Task EnqueueOnDispatcherQueueAsync(Func<Task> function);
    }
}
