namespace UiTools.Av.Services.SimpleNavigation;

public interface INavigationAware
{
    
    /// <summary>
    /// Occurs when the model is navigated to. Do your initialization here
    /// </summary>
    void OnNavigatedTo();

    /// <summary>
    /// Occurrs when another model is navigated to by the same navigator. DISPOSE your resources here instead of implementing IDisposable
    /// </summary>
    void OnNavigatedFrom();
}
