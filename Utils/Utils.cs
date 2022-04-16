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

namespace MyTools
{

    

    public static class Utils
    {

        #region Paths

        public static string ExePath()
        {
            return Assembly.GetEntryAssembly()?.Location;
        }

        public static string ExeDir()
        {
            return Path.GetDirectoryName(ExePath());
        }

        public static string ExeName()
        {
            return Path.GetFileName(ExePath());
        }


        public static string ExeNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(ExePath());
        }

        #endregion Paths

        #region Crypto  
        public static string Md5File(string fileName)
        {
            try
            {
                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] hash;
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    hash = md5.ComputeHash(fs);
                }
                fs.Close();
                var sb = new StringBuilder();
                foreach (byte hex in hash) sb.Append(hex.ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }


        public static string Md5String(string sourceString)
        {
            if (sourceString == null) sourceString = "";
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var data = Encoding.UTF8.GetBytes(sourceString);
                data = md5.ComputeHash(data);
                var sb = new StringBuilder();
                foreach (byte hex in data) sb.Append(hex.ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }
        }

        #endregion Crypto

        

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
        /// Returns a copy (a snapshot) of an object
        /// </summary>
        public static T DeepCopy<T>(this T @this)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, @this);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }


        /// <summary>
        /// Does kinda same thing as DeepCopy but preserves the original object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyProperties(object source, object target)
        {
            var srcType = source.GetType();
            var trgType = target.GetType();

            if (srcType != trgType) throw new Exception("Source and target types must be identical");

            foreach (var prop in srcType.GetProperties())
            {
                var val = prop.GetValue(source);
                prop.SetValue(target, val);
            }
        }


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


        public static void StartProcess(string filepathOrUrl, string arguments = "")
        {
            var p = new Process { StartInfo = { FileName = filepathOrUrl, Arguments = arguments, UseShellExecute = true } };
            p.Start();
        }

        #endregion ToSort





    }
}
