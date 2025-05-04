using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiTools.Av.Views;

namespace UiTools.Av.Services;

public class ADialogService
{
    public async Task<bool> Confirmed(string title, string message)
    {
        var contentDialog = new ContentDialog
        {
            Title = title,
            Content = new MessageDialogContent
            {
                IconKind = MessageDialogIconKind.Question,
                Message = message
            },
            PrimaryButtonText = "Yes",
            SecondaryButtonText = "No"
        };

        var result = await contentDialog.ShowAsync();
        return result == ContentDialogResult.Primary;
    }

    public async Task ShowErrorMessage(string title, string message)
    {
        var contentDialog = new ContentDialog
        {
            Title = title,
            Content = new MessageDialogContent
            {
                IconKind = MessageDialogIconKind.Error,
                Message = message
            },
            PrimaryButtonText = "OK"
        };

        await contentDialog.ShowAsync();
    }

    public async Task ShowMessage(string title, string message)
    {
        var contentDialog = new ContentDialog
        {
            Title = title,
            Content = new MessageDialogContent
            {
                IconKind = MessageDialogIconKind.Info,
                Message = message
            },
            PrimaryButtonText = "OK"
        };

        await contentDialog.ShowAsync();
    }

    public async Task<(bool success, string inputValue)> InputBox(string title, string prompt, string placeholderText, string initialValue)
    {
        var contentDialog = new ContentDialog
        {
            Title = title,
            Content = new InputDialogContent
            {
                Prompt = prompt,
                Input = initialValue
            },
            PrimaryButtonText = "OK",
            SecondaryButtonText = "Cancel"
        };

        var result = await contentDialog.ShowAsync();
        var inputValue = ((InputDialogContent)contentDialog.Content).Input;

        return (result == ContentDialogResult.Primary, inputValue);
    }
}
