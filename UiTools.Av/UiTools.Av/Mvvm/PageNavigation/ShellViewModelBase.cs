using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace UiTools.Av.Mvvm.PageNavigation;

public partial class ShellViewModelBase : LocatableViewModelBase
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected ShellViewModelBase()
    {
        
    }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected ShellViewModelBase(PageFactory pageFactory)
    {
        _pageFactory = pageFactory;
    }

    private readonly PageFactory? _pageFactory;


    [ObservableProperty]
    private PageViewModelBase? _currentPage;

    protected async Task GotoPage<TPageViewModel>() where TPageViewModel : PageViewModelBase
    {
        if (CurrentPage is TPageViewModel) return;
        if (_pageFactory == null) return; //design time
        if (CurrentPage != null) await CurrentPage.OnNavigatedFrom();
        CurrentPage = _pageFactory.CreatePage<TPageViewModel>();
        await CurrentPage.OnNavigatedTo();
    }

    //proteted partial virtual void OnCurrentPageChanged(PageViewModelBase? oldValue, PageViewModelBase? newValue)
    //{
    //    //UpdateSelectedStates(newValue);
    //}

    partial void OnCurrentPageChanged(PageViewModelBase? value)
    {
        NavigatedToPage(value);
    }

    protected virtual void NavigatedToPage(PageViewModelBase? newPage){}
}