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
using UiTools.Av.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UiTools.Av.Extensions;

namespace UiTools.Av.Demo.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    public MainViewModel()
    {

        _sortFieldSelector = new SortFieldSelectorVm<PersonViewModel>
        (
            nameMappings: [(nameof(PersonViewModel.BirthDate), "Date of burth")],
            skipFields: [nameof(PersonViewModel.Id)]
        );
        _sortFieldSelector.Check(nameof(PersonViewModel.BirthDate), true);
        _sortFieldSelector.Check(nameof(PersonViewModel.Name), false);
        _sortFieldSelector.SortingConditionsChanged += _sortFieldSelector_SortingConditionsChanged; 



        _persons.QueueEmpty += (s, e) =>
        {
            //MainThreadService.Instance.EnqueueOnMainThread(() =>
            //{
                LogText += "Queue empty\n";
            //});
        };
    }

    private async void _sortFieldSelector_SortingConditionsChanged(object? sender, EventArgs e)
    {
        Kandishens =
            "conds: \n\n" + string.Join("\n", SortFieldSelector.SortingConditions) + "\n\n" +
            string.Join("; ", SortFieldSelector.SelectedFields.Select(x => x.DisplayName))
            ;
        await ApplySorting();
    }

    //todo ApplySorting()
    private async Task ApplySorting()
    {
        //LogText += "ApplySorting НЕ ЗАИМПЛЕМЕНЧЕНИНГ!!\n";
        await Persons.InplaceSort(SortFieldSelector.SortingConditions);

        //var sorted = Persons.MultiSort(SortFieldSelector.SortingConditions).ToList();
        //Persons = new ResponsiveCollection<PersonViewModel>(sorted);
    }

    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";


    [ObservableProperty]
    private SortFieldSelectorVm<PersonViewModel> _sortFieldSelector;

    [ObservableProperty]
    private string _kandishens = string.Empty;





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
            IconKind = MessageDialogIconKind.Error,
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


    [RelayCommand]
    private async Task ShowInfoDialog()
    {
        var infoContent = new MessageDialogContent
        {
            IconKind = MessageDialogIconKind.Info,
            Message = "Это информационное сообщение.\nДетали: некоторые важные данные для пользователя."
        };

        var dialog = new ContentDialog
        {
            Title = "Information",
            PrimaryButtonText = "ОК",
            Content = infoContent
        };

        await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowWarningDialog()
    {
        var warningContent = new MessageDialogContent
        {
            IconKind = MessageDialogIconKind.Warning,
            Message = "Это предупреждение!\nОбратите внимание, это важное сообщение для вашей работы."
        };

        var dialog = new ContentDialog
        {
            Title = "Warning",
            PrimaryButtonText = "ОК",
            Content = warningContent
        };

        await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowQuestionDialog()
    {
        var questionContent = new MessageDialogContent
        {
            IconKind = MessageDialogIconKind.Question,
            Message = "Вы уверены, что хотите продолжить?\nЭто действие нельзя отменить."
        };

        var dialog = new ContentDialog
        {
            Title = "Question",
            PrimaryButtonText = "Yes",
            SecondaryButtonText = "No",
            Content = questionContent
        };

        var result = await dialog.ShowAsync();
        LogText += $"question dialog result: {result}\n";
    }


    [RelayCommand]
    private async Task AskForInput()
    {
        var inputContent = new InputDialogContent
        {
            Prompt = "Enter something", 
            Input = string.Empty 
        };

        var dialog = new ContentDialog
        {
            Title = "Input Required",
            PrimaryButtonText = "OK",
            SecondaryButtonText = "Cancel",
            Content = inputContent
        };


        var result = await dialog.ShowAsync();
        LogText += $"input dialog result: {result}\nInput: {((InputDialogContent)dialog.Content).Input}";
    }


    // Для вызова через ADialogService
    [RelayCommand]
    private async Task ShowErrorDialogService()
    {
        var dialogService = new ADialogService();
        await dialogService.ShowErrorMessage("Error", "Это ошибка, которая приходит через сервис.");
    }

    [RelayCommand]
    private async Task ShowInfoDialogService()
    {
        var dialogService = new ADialogService();
        await dialogService.ShowMessage("Information", "Это информационное сообщение, которое приходит через сервис.");
    }

    [RelayCommand]
    private async Task ShowWarningDialogService()
    {
        var dialogService = new ADialogService();
        await dialogService.ShowMessage("Warning", "Это предупреждение, которое приходит через сервис.");
    }

    [RelayCommand]
    private async Task ShowQuestionDialogService()
    {
        var dialogService = new ADialogService();
        var result = await dialogService.Confirmed("Question", "Вы уверены, что хотите продолжить?");
        LogText += $"Question dialog result: {result}\n";
    }

    [RelayCommand]
    private async Task AskForInputService()
    {
        var dialogService = new ADialogService();
        var (success, input) = await dialogService.InputBox("Input Required", "Please enter something:", "Placeholder text", string.Empty);
        LogText += $"Input dialog result: {success}, Input: {input}\n";
    }



    [ObservableProperty] private MessageStack _messageStack = new();


    [RelayCommand]
    public void ClearStackedMessages()
    {
        MessageStack.Clear();
    }

    [RelayCommand]
    public async Task AddRandomStackedMessages()
    {
        var random = new Random();
        var types = Enum.GetValues(typeof(StackableMessageSeverity)).Cast<StackableMessageSeverity>().ToArray();

        var count = random.Next(5, 11);
        var generatedIds = new List<string>();

        for (var i = 0; i < count; i++)
        {
            var severity = types[random.Next(types.Length)];
            var id = Guid.NewGuid().ToString();
            generatedIds.Add(id);

            double? ttl = random.NextDouble() < 0.5 ? random.Next(1000, 5001) : null;

            var title = $"{severity} message #{i + 1}";
            var msg = "This is a randomly generated";
            if (random.NextDouble() < 0.1)
                msg =
                    "Метакогнитивный анализ сущностных противоречий в режиме позднего капитализма демонстрирует поливалентность институционализированной эксплуатации как детерминанты циклических конъюнктурных колебаний, обеспечивающих перманентную концентрацию капитала в условиях регрессивной трансформации трудовой стоимости. При этом интероперабельность идеологических гегемоний с технократическим регулированием производства порождает парадигмальную инверсию социальной мобилизации, фетишизируя эквивалентную ценность товарного обмена как ключевой компонент институциональной легитимации рыночного порядка.\n\nСинергетическая ретроспекция марксистской методологии позволяет экстраполировать перспективу эволюционного преобразования экономической формации через деконструкцию механизмов отчуждения труда и стратегическую реорганизацию репрессивных моделей экономического доминирования. В этом контексте концептуальное переосмысление производства и распределения ресурсов открывает горизонты квазисистемного перехода от капиталистического детерминизма к пост-революционной фазе общественной самоорганизации.";
            
            var message = (ttl != null ? $" ---------------------- TTL = {ttl.Value} ms --------------- " : string.Empty) + $"{msg} {severity.ToString().ToLower()}.";

            switch (severity)
            {
                case StackableMessageSeverity.Error:
                    MessageStack.AddError(title, message, id, ttl);
                    break;
                case StackableMessageSeverity.Warning:
                    MessageStack.AddWarning(title, message, id, ttl);
                    break;
                case StackableMessageSeverity.Success:
                    MessageStack.AddSuccess(title, message, id, ttl);
                    break;
                default:
                    MessageStack.AddInfo(title, message, id, ttl);
                    break;
            }
        }
    }


}
