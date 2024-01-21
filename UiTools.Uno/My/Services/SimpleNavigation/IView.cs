namespace UiTools.Uno.My.Services.SimpleNavigation;

public interface IView
{
    
}


public interface IView<TViewModel> : IView
{
    TViewModel? Vm { get; set; }
}
