using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace UiTools.Uno.My.Mvvm
{
    public class ResponsiveCollection<T>: ObservableCollection<T>
    {
        public ResponsiveCollection()
        {
            _stopwatch = new Stopwatch();
            _taskQueue = new ActionBlock<Func<Task>>(t => t(), new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
            _stopwatch.Start();
        }





        private readonly Stopwatch _stopwatch;
        
        /// <summary>
        /// To be called periodically after each iteration of a potentially slow ui blocking loop
        /// </summary>
        /// <returns></returns>
        public async Task EnsureResponsiveness()
        {
            if (_stopwatch.ElapsedMilliseconds > 30)
            {
                await Task.Delay(1);
                _stopwatch.Restart();
                UpdateUi?.Invoke(this, EventArgs.Empty);
            }
        }

        public Task AddResponsively(T item)
        {
            Add(item);
            return EnsureResponsiveness();
        }

        public Task Enque(Func<Task> f) => _taskQueue.SendAsync(f);

        public event EventHandler UpdateUi;

        private ActionBlock<Func<Task>> _taskQueue;

        //public void Test()
        //{

        //}

        //public void EnqueueTask(Task t)
        //{
        //    lock (_queueLock)
        //    {
        //        _taskQueue.Enqueue(t);
        //    }
        //}

        //private object _queueLock = new();

        //private Queue<Task> _taskQueue = new();


    }
}
