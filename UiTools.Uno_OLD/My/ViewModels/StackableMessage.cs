using CommunityToolkit.Mvvm.ComponentModel;

namespace UiTools.Uno.My.ViewModels
{
    public partial class StackableMessage : ObservableObject
    {
        [ObservableProperty]
        private string? _messageText;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private StackableMessageSeverity _severity;

        [ObservableProperty]
        private bool _isOpen;


        public string? Id { get; set; }
        //public bool IsClosed { get; set; }

        //[RelayCommand]
        //private void CloseMe()
        //{
        //    IsClosed = true;
        //}
    }
}
