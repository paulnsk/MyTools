using System.Threading.Tasks;
using UiTools.Av.Interface;

namespace UiTools.Av.Mvvm.PageNavigation;

public partial class PageViewModelBase : LocatableViewModelBase, INavigationAware
{
    public virtual Task OnNavigatedTo()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }
}