using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using UiTools.Av.Services;

namespace UiTools.Av.Mvvm;

public class ResponsiveCollection<T>: ObservableCollection<T>
{

    /*
     *
     * On Wasm collection views such as ListView are slow to the point of being unusable.
     *
     * The two particularly annoying issues are:
     *  - UI is frozen when items are added or updated in bulk (a single item make take .1 sec to update)
     *  - User clicks on buttons faster than UI can respond so, for example, sorting command is issued before the collection is fully populated
     *
     * This custom observable collection class does 2 things:
     *
     *  - adds a background queue for holding UI-updating requests.
     *
     *  - wraps each UI-updating request in a call to Community Toolkit's DispatcherQueue.EnqueueAsync() extension method.
     *    Because MaxDegreeOfParallelism is 1, next request won't run until current request is processed.
     *    The current request is considered processed when it has actually finished running on the DispatcherQueue, NOT when it was enqueued,
     *    because of the way EnqueueAsync() is implemented.
     *    These awaits between requests are enough for UNO WASM to do whatever it is doing under the hood to update the UI.
     *    (Simply enqueing tasks on dispatcher queue one by one does not do the trick, UI is still updated only when all tasks are done)
     *
     *
     *
     *
     * *** The consumer should ALWAYS use Enqueue() to make changes to the collection. ***
     *
     *
     *
     */

    //somewhat inspired by https://stackoverflow.com/questions/13302933/how-to-avoid-firing-observablecollection-collectionchanged-multiple-times-when-r

    //todo android performance when adding items 1 by 1. Use BatchBlock
        
    public ResponsiveCollection()
    {
        _taskQueue = new ActionBlock<Func<Task>>
        (
            async t =>
            {
                await MainThreadService.Instance.EnqueueOnMainThreadAndWait(t);
                //MainThreadService.Instance.EnqueueOnMainThread(async ()=>
                //{
                //    await t();
                //});
                if (_taskQueue!.InputCount == 0) { QueueEmpty?.Invoke(this, EventArgs.Empty); }
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 }
        );
    }


    public Task Enqueue(Func<Task> f) => _taskQueue.SendAsync(f);
    public Task Enqueue(Action a) => _taskQueue.SendAsync(() =>
    {
        a();
        return Task.CompletedTask;
    });


    /// <summary>
    /// A replacement for ctor(Ienumerable<T>) which would recreate a new instance of task queue each time the collection is repopulated.
    /// Instead we populate it without recreating it, while keeping UI responsive
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public async Task AddRangeQueued(IEnumerable<T> collection)
    {
        foreach (var t in collection)
        {
            await Enqueue(() => Add(t));
        }
    }

    public Task AddItemQueued(T item)
    {
        return Enqueue(() => Add(item));
    }

    public void InsertSorted<TKey>(T item, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
    {
        Insert(FindInsertionIndex(this, item, keySelector), item);
    }

    private static int FindInsertionIndex<T, TKey>(IReadOnlyList<T> sortedList, T newItem, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        var index = 0;

        while (index < sortedList.Count && keySelector(sortedList[index]).CompareTo(keySelector(newItem)) < 0)
        {
            index++;
        }

        return index;
    }

    private readonly ActionBlock<Func<Task>> _taskQueue;

    public event EventHandler? QueueEmpty;

}