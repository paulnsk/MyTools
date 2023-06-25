
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace UiTools.Uno.ViewModels
{

    //todo как конвертить СортКондишен в SortableFieldViewModel? это посути одно и то же

    public partial class SortableFieldViewModel : ObservableObject
    {


        public SortableFieldViewModel(string fieldName)
        {
            _fieldName = fieldName;            
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
            get => IsDescending ? "\uE74B" : "\uE74A";
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