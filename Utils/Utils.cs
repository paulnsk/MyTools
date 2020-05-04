using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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

        #endregion ToSort





    }
}
