using Microsoft.UI.Xaml.Controls;

namespace WinuiTools.Xamls
{
    public sealed partial class ContentDialogBig : ContentDialog
    {

        //_todo remove "container"
        public ContentDialogBig()
        {
            this.InitializeComponent();
            //container.Width = App.StartupWindow.Bounds.Width * .8;
            //container.Height = App.StartupWindow.Bounds.Height * .7;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
