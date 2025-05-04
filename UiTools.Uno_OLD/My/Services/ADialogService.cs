using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UiTools.Uno.My.Views;

namespace UiTools.Uno.My.Services;

public abstract class ADialogService
{
    protected XamlRoot? ShellXamlRoot;

    protected void CheckInitialized()
    {
        if (ShellXamlRoot == null)
        {
            throw new Exception($"{nameof(ADialogService)} not initialized!");
        }
    }


    public void Initialize(XamlRoot shellXamlRoot)
    {
        ShellXamlRoot = shellXamlRoot;
    }


    public async Task<bool> Confirmed(string title, string message)
    {
        CheckInitialized();

        var contentDialog = new ContentDialog
        {
            Title = title,
            Content = message,
            //Content = new SampleControl(),
            PrimaryButtonText = "OK",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = ShellXamlRoot,
        };

        return await contentDialog.ShowAsync() == ContentDialogResult.Primary;
    }

    public async Task ShowErrorMessage(string title, string message)
    {
        CheckInitialized();
        var dialog = new ErrorDialog
        {
            Title = title,
            ErrorMessage = message,
            XamlRoot = ShellXamlRoot,
        };
        await dialog.ShowAsync();
    }

    public async Task ShowMessage(string title, string message)
    {
        CheckInitialized();
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            XamlRoot = ShellXamlRoot,
            PrimaryButtonText = "OK"
        };
        await dialog.ShowAsync();
    }


    public async Task<(bool success, string inputValue)> InputBox(string title, string prompt, string placeHolderText, string initialValue)
    {
        CheckInitialized();
        var dialog = new InputDialog
        {
            Title = title,
            Prompt = prompt,
            PlaceholderText = placeHolderText,
            Input = initialValue,
            XamlRoot = ShellXamlRoot,
        };
        var result = await dialog.ShowAsync();
        return (result == ContentDialogResult.Primary, dialog.Input);
    }
}