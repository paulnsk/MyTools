using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace UiTools.Av.Demo.ViewModels;

public partial class PersonViewModel : ObservableObject
{
    private static int _idCounter = 0;

    public PersonViewModel()
    {
        Id = ++_idCounter;
    }

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private int _age;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private DateTime _birthDate;

    [ObservableProperty]
    private string _department = string.Empty;

    [ObservableProperty]
    private int _salary;
}