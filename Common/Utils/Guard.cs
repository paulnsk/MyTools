using System;
using System.Collections.Generic;
using System.Text;

namespace MyTools
{



    //https://stackoverflow.com/questions/44005383/using-a-custom-argument-validation-helper-breaks-code-analysis
    /// <summary>
    /// Indicates to Code Analysis that a method validates a particular parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }


    public static class Guard
    {
        public static void NotNull<T>(string argumentName, [ValidatedNotNull] T? value, string message = null) where T : struct
        {
            if (value.HasValue)
            {
                return;
            }

            throw (message == null) ? new ArgumentNullException(argumentName) : new ArgumentNullException(argumentName, message);
        }

        public static void NotNull<T>(string argumentName, [ValidatedNotNull] T value, string message = null) where T : class
        {
            if (value == null)
            {
                throw (message == null) ? new ArgumentNullException(argumentName) : new ArgumentNullException(argumentName, message);
            }
        }

        public static void NotEmpty(string argumentName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Non Empty string expected", argumentName);
            }
        }
    }
}
