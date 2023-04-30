using System;

namespace WinuiTools.Contracts
{
    public interface IMainThreadService
    {
        void RunOnMainThread(Action action);
    }
}
