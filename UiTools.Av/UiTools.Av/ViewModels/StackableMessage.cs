using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace UiTools.Av.ViewModels;

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

    private double? _ttlMilliseconds;
    public double? TtlMilliseconds
    {
        get => _ttlMilliseconds;
        set
        {
            _ttlMilliseconds = value;
            if (value > 0)
            {
                _ = AutoCloseAsync(value.Value);
            }
        }
    }

    private async Task AutoCloseAsync(double ms)
    {
        try
        {
            await Task.Delay((int)ms);
            IsOpen = false;
        }
        catch
        {
            // Ignore (e.g. app shutting down)
        }
    }
}