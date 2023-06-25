using System;
using System.Collections.Generic;

namespace UiTools.Uno.Models
{
    public class SortingConditionsChangedEventArgs : EventArgs
    {
        public IEnumerable<SortingCondition> SortingConditions { get; set; }

        public SortingConditionsChangedEventArgs(IEnumerable<SortingCondition> sortingConditions)
        {
            SortingConditions = sortingConditions;
        }

    }
}