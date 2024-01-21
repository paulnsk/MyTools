using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace UiTools.Uno.My.Views
{
    public sealed partial class InputDialog : ContentDialog
    {
        public InputDialog()
        {
            this.InitializeComponent();
        }


        public string Prompt
        {
            get => (string)GetValue(PromptProperty);
            set => SetValue(PromptProperty, value);
        }
        public static readonly DependencyProperty PromptProperty =
            DependencyProperty.Register(nameof(Prompt), typeof(string), typeof(InputDialog), new PropertyMetadata(""));

        public string Input
        {
            get => (string)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }
        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register(nameof(Input), typeof(string), typeof(InputDialog), new PropertyMetadata(""));



        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(InputDialog), new PropertyMetadata(""));


        private void InputDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbInput.SelectAll();
        }
    }
}
