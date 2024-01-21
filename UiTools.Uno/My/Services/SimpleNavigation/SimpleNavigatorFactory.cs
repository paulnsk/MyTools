using System;
using System.Collections.Generic;

namespace UiTools.Uno.My.Services.SimpleNavigation;

public class SimpleNavigatorFactory(IServiceProvider services)
{

    private static readonly Dictionary<Type, object> Navigators = new();

    //To ensure true singleton
    public SimpleNavigator<TShellView, TShellViewModel> GetNavigator<TShellView, TShellViewModel>() where TShellView : IShellView<TShellViewModel>
    {
        if (Navigators.TryGetValue(typeof(TShellView), out var navigatorObj))
        {
            return (SimpleNavigator<TShellView, TShellViewModel>)navigatorObj;
        }

        var navigator = new SimpleNavigator<TShellView, TShellViewModel>(services);
        Navigators[typeof(TShellView)] = navigator;
        return navigator;
    }

}
