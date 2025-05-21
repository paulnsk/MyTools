using Avalonia.Controls;

namespace UiTools.Av.Services.SimpleNavigation;

public interface IShellView<TShellViewModel> : IView<TShellViewModel>
{
    ContentControl NavigatorViewPort { get; }
}
