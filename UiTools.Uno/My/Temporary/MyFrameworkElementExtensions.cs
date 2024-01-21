using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;

namespace UiTools.Uno.My.Temporary
{

    //todo) Remove this if this ever gets fixed by community toolkit or uno people
    //https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4833
    //https://github.com/unoplatform/Uno.WindowsCommunityToolkit/issues/203
    public static class MyFrameworkElementExtensions
    {
        private static readonly object CursorLock = new();
        private static readonly InputCursor DefaultCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        private static readonly Dictionary<InputSystemCursorShape, InputCursor> Cursors = new() { { InputSystemCursorShape.Arrow, DefaultCursor } };

        /// <summary>
        /// Dependency property for specifying the target <see cref="InputSystemCursorShape"/> to be shown
        /// over the target <see cref="FrameworkElement"/>.
        /// </summary>
        public static readonly DependencyProperty CursorProperty =
            DependencyProperty.RegisterAttached("Cursor", typeof(InputSystemCursorShape), typeof(MyFrameworkElementExtensions), new PropertyMetadata(InputSystemCursorShape.Arrow, CursorChanged));

        /// <summary>
        /// Set the target <see cref="InputSystemCursorShape"/>.
        /// </summary>
        /// <param name="element">Object where the selector cursor type should be shown.</param>
        /// <param name="value">Target cursor type value.</param>
        public static void SetCursor(FrameworkElement element, InputSystemCursorShape value)
        {
            element.SetValue(CursorProperty, value);
        }

        /// <summary>
        /// Get the current <see cref="InputSystemCursorShape"/>.
        /// </summary>
        /// <param name="element">Object where the selector cursor type should be shown.</param>
        /// <returns>Cursor type set on target element.</returns>
        public static InputSystemCursorShape GetCursor(FrameworkElement element)
        {
            return (InputSystemCursorShape)element.GetValue(CursorProperty);
        }

        private static void CursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null)
            {
                throw new NullReferenceException(nameof(element));
            }
            
            var prop = typeof(FrameworkElement).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(x => x.Name == "ProtectedCursor");

            var value = (InputSystemCursorShape)e.NewValue;


            lock (CursorLock)
            {
                if (!Cursors.ContainsKey(value))
                {
                    Cursors[value] = InputSystemCursor.Create(value);
                }

                prop!.SetValue(element, Cursors[value]);
            }
            
        }
    }
}
