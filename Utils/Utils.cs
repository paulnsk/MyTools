using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MyTools
{

    

    public static class Utils
    {


        
        #region ToSort

        /// <summary>
        /// Attempts to read text file multiple times in the hope that if file was blocked by another process it will be released within <see cref="timeOutMilliSeconds"/>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="timeOutMilliSeconds"></param>
        /// <returns></returns>
        public static string ReadTextFileSafe(string fileName, int timeOutMilliSeconds = 10000)
        {

            string result;
            int timeSpent = 0;

            while (true)
            {
                if (!File.Exists(fileName)) return string.Empty;
                try
                {
                    result = File.ReadAllText(fileName);
                    break;
                }
                catch
                {
                    timeSpent += 100;
                    Thread.Sleep(100);
                    if (timeSpent > timeOutMilliSeconds) throw;
                }
            }
            return result;
        }


        ///// <summary>
        ///// Removes field from a dynamic object
        ///// </summary>
        ///// <param name="o"></param>
        ///// <param name="fieldName"></param>
        //public static void RemoveDynamicField(dynamic o, string fieldName)
        //{
        //    IDictionary<string, object> map = o;
        //    map.Remove(fieldName);
        //}




        /// <summary>
        /// Splits a string that contains some quoted parts within which spaces are ignored
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static List<string> SplitQuoted(this string @this)
        {
            //https://stackoverflow.com/questions/14655023/split-a-string-that-has-white-spaces-unless-they-are-enclosed-within-quotes
            return Regex.Matches(@this, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();
        }


        public static void CopyProperties(object source, object target)
        {
            var type1 = source.GetType();
            var type2 = target.GetType();
            if (type1 != type2) throw new Exception("Source and target types must be identical");
            foreach (var property in type1.GetProperties())
            {
                var obj = property.GetValue(source);
                property.SetValue(target, obj);
            }
        }

        #endregion ToSort


    }
}
