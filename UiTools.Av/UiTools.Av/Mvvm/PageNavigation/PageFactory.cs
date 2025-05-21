using System;
using Microsoft.Extensions.DependencyInjection;

namespace UiTools.Av.Mvvm.PageNavigation;

public class PageFactory(IServiceProvider serviceProvider) 
{
    public T CreatePage<T>() where T : PageViewModelBase => serviceProvider.GetRequiredService<T>();
}
