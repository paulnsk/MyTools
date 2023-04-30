using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using WinuiTools.Xamls;

namespace WinuiTools
{
    public static class Dialogs
    {

        //todo ** перетащить диалогсервис сюда, и диспетчерсервис заодно. Добавить в диспетчерсервис еще и ссылку на хамлрут главного окна
        

        //todo save dialog
        public static async Task<string> ShowOpenFileDialog(this Window @this, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail, IList<string> fileTypeFilter = default)
        {
            var openPicker = new FileOpenPicker();
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(@this);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            
            fileTypeFilter ??= new List<string> { "*" };
            foreach (var s in fileTypeFilter)
            {
                openPicker.FileTypeFilter.Add(s);
            }

            var file = await openPicker.PickSingleFileAsync();

            return file?.Path;
        }

        //todo Confirmed()
        
        //todo_ this works for background threads but if there is already a content dialog open the one initiated by bg thread is ignored.
        public static async Task ShowMessage(this Window @this, string message)
        {
            if (@this == null) return;
            if (@this.Content == null) return;

            await @this.DispatcherQueue.EnqueueAsync(async () =>
            {
                var dialog = new ContentDialog
                {
                    Title = Tools.AppTitle,
                    Content = message,
                    PrimaryButtonText = "Ok",
                    XamlRoot = @this.Content.XamlRoot,
                };
                await dialog.ShowAsync();
            });
        }

        public static async Task ShowErrorMessage(this Window @this, string message)
        {
            if (@this == null) return;
            if (@this.Content == null) return;

            await @this.DispatcherQueue.EnqueueAsync(async () =>
            {
                var dialog = new ErrorDialog()
                {
                    Title = Tools.AppTitle,
                    ErrorMessage = message,
                    PrimaryButtonText = "Ok",
                    XamlRoot = @this.Content.XamlRoot,
                };
                await dialog.ShowAsync();
            });

            
        }

    }
}
