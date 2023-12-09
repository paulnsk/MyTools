using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UiTools.Uno.My.ViewModels
{

    
    public partial class SortableFieldVm : ObservableObject
    {


        public SortableFieldVm(string fieldName)
        {
            _fieldName = fieldName;
            //PropertyChanged += (s, e) => 
            //{
            //    if (e.PropertyName == nameof(IsDescending)) 
            //    {
            //        ArrowGlyph = IsDescending ? "D" : "A";
            //    }
            //};
        }

        
        public string ArrowToolTip
        {
            //getter only property does not want to be bindable
            get => IsDescending ? "Descending" : "Ascending";
            set { }
        }


        public string ArrowGlyph
        {
            //getter only property does not want to be bindable            
            get => IsDescending ? "↓" : "↑";            
            set { }
        }

        [ObservableProperty]
        private string _fieldName;

        [ObservableProperty]
        private string? _displayName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ArrowGlyph))]
        [NotifyPropertyChangedFor(nameof(ArrowToolTip))]
        private bool _isDescending;

        [ObservableProperty]
        private bool _isChecked;

        [ObservableProperty]
        private int _order;

        [RelayCommand]
        private void Uncheck()
        {
            IsChecked = false;  
        }


        partial void OnIsCheckedChanged(bool value)
        {
            IsCheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        partial void OnIsDescendingChanged(bool value)
        {
            IsDescendingChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? IsCheckedChanged;
        public event EventHandler? IsDescendingChanged;

    }


}