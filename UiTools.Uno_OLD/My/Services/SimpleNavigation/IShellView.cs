using Microsoft.UI.Xaml.Controls;

namespace UiTools.Uno.My.Services.SimpleNavigation;

public interface IShellView<TShellViewModel> : IView<TShellViewModel>
{
    ContentControl NavigatorViewPort { get; }
}
