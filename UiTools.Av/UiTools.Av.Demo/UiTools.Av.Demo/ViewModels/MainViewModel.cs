using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyTools;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using UiTools.Av.Mvvm;
using UiTools.Av.Services;
using UiTools.Av.Views;

namespace UiTools.Av.Demo.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    public MainViewModel()
    {
        _persons.QueueEmpty += (s, e) =>
        {
            //MainThreadService.Instance.EnqueueOnMainThread(() =>
            //{
                LogText += "Queue empty\n";
            //});
        };
    }

    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";


    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private bool _isOption1Selected = true; // Default selection

    [ObservableProperty]
    private bool _isOption2Selected = false;

    [ObservableProperty]
    private bool _isOption3Selected = false;

    
    [RelayCommand]
    private void ButtonClick()
    {
        InputText = $"Button Clicked at {DateTime.Now}";
    }

    [RelayCommand]
    private void CloseButtonClick()
    {
        // Example action: Log or update InputText
        InputText = $"Closebutton Clicked at {DateTime.Now}";
    }
    


    [ObservableProperty]
    private string _threadDemoResult = "No task run yet";

    [RelayCommand]
    private async Task RunBackgroundTask()
    {
        ThreadDemoResult = "Starting background task...";

        await Task.Run(async () =>
        {

            // Simulate work in background thread
            await Task.Delay(1000);

            // Update UI via MainThreadService (sync)
            MainThreadService.Instance.EnqueueOnMainThread(() =>
            {
                ThreadDemoResult = "Updated from background (sync) at " + DateTime.Now;
            });

            // Simulate async work
            await Task.Delay(1000);

            // Update UI via MainThreadService (async)
            await MainThreadService.Instance.EnqueueOnMainThreadAndWait(async () =>
            {
                await Task.Delay(500); // Simulate async UI work
                ThreadDemoResult = "Updated from background (async) at " + DateTime.Now;
            });
        });
    }



    [ObservableProperty]
    private ResponsiveCollection<PersonViewModel> _persons = new();

    [ObservableProperty]
    private string _logText = string.Empty;


    [ObservableProperty]
    private int _numPersonsToAdd = 100;


    [RelayCommand]
    private async Task AddPersons()
    {
        LogText += $"Adding {NumPersonsToAdd} persons...\n";
        var newPersons = Enumerable.Range(1, NumPersonsToAdd)
            .Select(i => new PersonViewModel
            {
                Name = RandomDataGenerator.RandomName(3, 8, RandomDataGenerator.CapMode.FirstCap),
                LastName = RandomDataGenerator.RandomName(4, 10, RandomDataGenerator.CapMode.FirstCap),
                Age = RandomDataGenerator.RandomNumber(2).Select(c => c - '0').Sum() + 18,
                Email = $"{RandomDataGenerator.RandomName(5, 8, RandomDataGenerator.CapMode.NoCaps)}@{RandomDataGenerator.RandomName(3, 8, RandomDataGenerator.CapMode.NoCaps)}.{RandomDomain()}",
                Phone = RandomPhone(),
                BirthDate = RandomDataGenerator.RandomDate(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-18)),
                Department = RandomDataGenerator.RandomName(5, 10, RandomDataGenerator.CapMode.FirstCap),
                Salary = RandomDataGenerator.RandomNumber(5).Select(c => c - '0').Sum() * 1000 + 30000
            })
            .ToList();
        await Persons.AddRangeQueued(newPersons);
    }

    private static string RandomDomain()
    {
        var domains = new[] { "com", "net", "ru", "org" };
        return domains[new Random().Next(domains.Length)];
    }

    private static string RandomPhone()
    {
        var random = new Random();
        var countryCodes = new[] { "+1", "+44", "+7", "+81" };
        var cityCodes = new[] { "495", "212", "813", "650" };
        return $"{countryCodes[random.Next(countryCodes.Length)]}-{cityCodes[random.Next(cityCodes.Length)]}-{RandomDataGenerator.RandomNumber(3)}-{RandomDataGenerator.RandomNumber(4)}";
    }


    [RelayCommand]
    private async Task ClearPersons()
    {
        LogText += "Clearing persons...\n";
        await Persons.Enqueue(() => Persons.Clear());
    }


    [RelayCommand]

    private async Task ShowErrorDialog()
    {
        var errorContent = new MessageDialogContent
        {
            IconKind = MessageDialogIconKind.Question,
            Message = "Это еррор, детка.\nВ рамках многомерной квантовой топологии ключевым аспектом анализа\n остаётся взаимодействие нелокальных когерентных структур,\n детерминирующих динамику флуктуационных модулей. Это особенно важно при рассмотрении дисперсионных свойств гиперболической метафизической среды, способной индуцировать спонтанную трансцендентную синхронизацию спектральных компонент."
        };

        var dialog = new ContentDialog
        {
            Title = "Error",
            PrimaryButtonText = "ОК",
            Content = errorContent
        };

        await dialog.ShowAsync();
    }
}
