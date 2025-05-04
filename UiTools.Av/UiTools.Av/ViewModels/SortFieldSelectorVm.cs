using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using UiTools.Av.Models;

namespace UiTools.Av.ViewModels
{
    /// <summary>
    /// Implements viewmodel for list of sortable fields to be bound to SortByDropdown flyout or similar colelction view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class SortFieldSelectorVm<T> : ObservableObject
    {

        public SortFieldSelectorVm((string fieldName, string displayName)[] nameMappings = default, string[] skipFields = default)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (skipFields?.Contains(prop.Name) != true) AddField(new SortableFieldVm(prop.Name)
                {                    
                    DisplayName = nameMappings?.FirstOrDefault(x => x.fieldName == prop.Name).displayName ?? prop.Name,
                });
            }
        }

        [ObservableProperty]
        private ObservableCollection<SortableFieldVm> _sortableFields = new();

        [ObservableProperty]
        private ObservableCollection<SortableFieldVm> _selectedFields = new();

        public IEnumerable<SortingCondition> SortingConditions => SortableFieldsAsSortingConditions();


        private void ReorderFieldCollection()
        {
            var ordered = SortableFields.OrderBy(x => x.IsChecked ? x.Order : int.MaxValue).ThenBy(x => x.DisplayName).ToList();
            SortableFields = new ObservableCollection<SortableFieldVm>(ordered);
        }

        private void AddField(SortableFieldVm sfvm)
        {            
            sfvm.IsCheckedChanged += Sfvm_IsCheckedChanged;
            sfvm.IsDescendingChanged += Sfvm_IsDescendingChanged;
            SortableFields.Add(sfvm);
        }


        private List<SortableFieldVm> CheckedFields(SortableFieldVm except = default)
        {
            return SortableFields.Where(x => x.IsChecked && x != except).OrderBy(x => x.Order).ToList();
        }

        private void Sfvm_IsCheckedChanged(object? sender, EventArgs e)
        {
            if (sender is not SortableFieldVm sfvm) return;

            var previouslyCheckedFields = CheckedFields(except: sfvm);

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
            
            SelectedFields = new ObservableCollection<SortableFieldVm>(CheckedFields());

            //todo поднять это над  SelectedFields =
            FireSortingConditionsChanged();

        }

        private void Sfvm_IsDescendingChanged(object? sender, EventArgs e)
        {
            if (sender is not SortableFieldVm sfvm) return;
            if (!sfvm.IsChecked) return;
            FireSortingConditionsChanged();
        }

        private void FireSortingConditionsChanged()
        {
            SortingConditionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private List<SortingCondition> SortableFieldsAsSortingConditions()
        {
            return SortableFields.Where(x => x.IsChecked).OrderBy(x => x.Order).Select(x => new SortingCondition(x.FieldName) { IsDescending = x.IsDescending }).ToList();
        }

        public event EventHandler? SortingConditionsChanged;

        public void Check(string fieldName, bool isDescending)
        {
            var field = SortableFields.FirstOrDefault(x => x.FieldName == fieldName);
            if (field == null) return;
            field.IsDescending = isDescending;
            field.IsChecked = true;
        }

        public void SetSortingConditions(IEnumerable<SortingCondition>? conditions)
        {
            if (conditions == null) return;
            foreach (var condition in conditions)
            {
                Check(condition.FieldName, condition.IsDescending);
            }
        }
    }
}