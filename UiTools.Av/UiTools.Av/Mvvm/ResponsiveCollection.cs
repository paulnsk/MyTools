﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using UiTools.Av.Extensions;
using UiTools.Av.Models;
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

    //todo_ (uno) android performance when adding items 1 by 1. Use BatchBlock

    private readonly ActionBlock<(ResponsiveCollection<T>, Func<Task>)> _taskQueue;
    private bool _antiClogIgnore = false;

    public ResponsiveCollection()
    {
        _taskQueue = new ActionBlock<(ResponsiveCollection<T>, Func<Task>)>
        (
            async ((ResponsiveCollection<T> r,Func<Task> t) tupol) =>
            {
                //skipping the actual action if ignore flag is on, only allowing it to dequeue
                if (!tupol.r._antiClogIgnore) await MainThreadService.Instance.EnqueueOnMainThreadAndWait(tupol.t);
                
                if (_taskQueue!.InputCount == 0)
                {
                    //reset anticlog ignore flag on queue empty
                    tupol.r._antiClogIgnore = false;
                    QueueEmpty?.Invoke(this, EventArgs.Empty);
                }
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 }
        );
        
    }
    //todo в номер версии добавить минуты

    /// <summary>
    /// When enabled and when number of items in queue reached <see cref="AntiClogMaxItemCount"/>, further items are ignored and a flag is set to prevent existing items from executing ths
    /// making existing items also ignored. Once queue is cleared, the flag is reset.
    /// </summary>
    public bool AntiClogProtectionEnabled { get; set; } = false;
    public int AntiClogMaxItemCount { get; set; } = 500;

    private void CheckAntiClog()
    {
        if (!AntiClogProtectionEnabled) return;
        if (_taskQueue.InputCount > AntiClogMaxItemCount) _antiClogIgnore = true;
    }

    public Task Enqueue(Func<Task> f)
    {
        CheckAntiClog();
        return _taskQueue.SendAsync((this, f));
    }

    public Task Enqueue(Action a)
    {
        CheckAntiClog();
        return _taskQueue.SendAsync((this, () =>
        {
            a();
            return Task.CompletedTask;
        }));
    }


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


    public event EventHandler? QueueEmpty;




    /// <summary>
    /// Sorts the collection in place by replacing items one by one using the provided sorting conditions.
    /// Each replacement is enqueued to ensure UI responsiveness and consistency with other collection operations.
    /// </summary>
    /// <param name="conditions">The sorting conditions to apply.</param>
    /// <returns>A task representing the completion of the sorting operation.</returns>
    public async Task InplaceSort(IEnumerable<SortingCondition> conditions)
    {
        var sortingConditions = conditions as SortingCondition[] ?? conditions.ToArray();
        if (!sortingConditions.Any())
        {
            return;
        }

        // Create a sorted copy of the collection
        var sortedList = this.MultiSort(sortingConditions).ToList();
        

        for (var i = 0; i < Count; i++)
        {
            var index = i; // Capture i for lambda
            if (!EqualityComparer<T>.Default.Equals(this[index], sortedList[index]))
            {
                await Enqueue(() =>
                {
                        this[index] = sortedList[index];
                });
            }
        }
        
    }

}