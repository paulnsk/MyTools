using CommunityToolkit.Mvvm.ComponentModel;

namespace UiTools.Av.Mvvm;

/// <summary>
/// Base clas for models handled by the ViewLocator.
/// "non-locatable" viewmodels can be inherited directly from observableobject
/// </summary>
public abstract class LocatableViewModelBase : ObservableObject
{
}
