namespace UiTools.Av.Services.SimpleNavigation;

public interface IView
{
    
}


public interface IView<TViewModel> : IView
{
    TViewModel? Vm { get; set; }
}
