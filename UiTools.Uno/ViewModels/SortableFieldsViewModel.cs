using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UiTools.Uno.Models;

namespace UiTools.Uno.ViewModels
{
    /// <summary>
    /// Implements viewmodel for list of sortable fields to be bound to SortByDropdown flyout or similar colelction view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class SortableFieldsViewModel<T> : ObservableObject
    {

        public SortableFieldsViewModel((string fieldName, string displayName)[] nameMappings, string[] skipFields)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!skipFields.Contains(prop.Name)) AddField(new SortableFieldViewModel(prop.Name)
                {                    
                    DisplayName = nameMappings.FirstOrDefault(x => x.fieldName == prop.Name).displayName ?? prop.Name,
                });
            }
        }

        [ObservableProperty]
        private ObservableCollection<SortableFieldViewModel> _sortableFields = new ObservableCollection<SortableFieldViewModel>();

        private void ReorderFieldCollection()
        {
            var ordered = SortableFields.OrderBy(x => x.IsChecked ? x.Order : int.MaxValue).ThenBy(x => x.DisplayName).ToList();
            SortableFields = new ObservableCollection<SortableFieldViewModel>(ordered);
        }

        private void AddField(SortableFieldViewModel sfvm)
        {            
            sfvm.IsCheckedChanged += Sfvm_IsCheckedChanged;
            sfvm.IsDescendingChanged += Sfvm_IsDescendingChanged;
            SortableFields.Add(sfvm);
        }


        private void Sfvm_IsCheckedChanged(object? sender, EventArgs e)
        {
            if (sender is not SortableFieldViewModel sfvm) return;

            var previouslyCheckedFields = SortableFields.Where(x => x.IsChecked && x != sfvm).OrderBy(x => x.Order).ToList();

            //removing gaps in order sequence due to unchecking of items
            for (int i = 0; i < previouslyCheckedFields.Count; i++)
            {
                previouslyCheckedFields[i].Order = i + 1;
            }

            if (sfvm.IsChecked)
            {

                //assigning newly checked item an order
                var maxOrder = previouslyCheckedFields.Select(x => x.Order).DefaultIfEmpty(0).Max();
                sfvm.Order = maxOrder + 1;
            }

            ReorderFieldCollection();
            FireSortingConditionsChanged();
        }

        private void Sfvm_IsDescendingChanged(object? sender, EventArgs e)
        {
            if (sender is not SortableFieldViewModel sfvm) return;
            if (!sfvm.IsChecked) return;
            FireSortingConditionsChanged();
        }

        private void FireSortingConditionsChanged()
        {
            SortingConditionsChanged?.Invoke(this, new SortingConditionsChangedEventArgs(SortableFieldsAsSortingConditions()));
        }

        private List<SortingCondition> SortableFieldsAsSortingConditions()
        {
            return SortableFields.Where(x => x.IsChecked).OrderBy(x => x.Order).Select(x => new SortingCondition(x.FieldName) { IsDescending = x.IsDescending }).ToList();
        }

        public event EventHandler<SortingConditionsChangedEventArgs>? SortingConditionsChanged;

    }
}