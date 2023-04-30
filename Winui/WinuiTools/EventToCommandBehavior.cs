using Microsoft.UI.Xaml;
using System;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Xaml.Interactivity;

//using CommunityToolkit.WinUI.UI.Behaviors

namespace WinuiTools
{

    /*
     *
     
     
     XAML usage:

        xmlns:t="using:WinuiTools"
        xmlns:interactivity="using:Microsoft.Xaml.Interactivity"


    <Page>
    ...
        <interactivity:Interaction.Behaviors>
            <t:EventToCommandBehavior 
                EventName="Loaded"
                Command="{x:Bind ViewModel.LoadedCommand}"/>
        </interactivity:Interaction.Behaviors>
     *
     *
     *
     *
     *
     */
    
    //todo look up toolkit might already have one
    public class EventToCommandBehavior : DependencyObject, IBehavior
    {
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register(
                nameof(EventName),
                typeof(string),
                typeof(EventToCommandBehavior),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(EventToCommandBehavior),
                new PropertyMetadata(null));

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            var eventInfo = associatedObject.GetType().GetRuntimeEvent(EventName);
            if (eventInfo != null)
            {
                eventInfo.AddEventHandler(associatedObject, new RoutedEventHandler(OnEventRaised));
            }
        }

        public void Detach()
        {
            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(EventName);
            if (eventInfo != null)
            {
                eventInfo.RemoveEventHandler(AssociatedObject, new RoutedEventHandler(OnEventRaised));
            }
            AssociatedObject = null;
        }

        private void OnEventRaised(object sender, RoutedEventArgs e)
        {
            if (Command != null && Command.CanExecute(null))
            {
                Command.Execute(null);
            }
        }
    }


}
