using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public  static class CryptoUtils
    {

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

        public static async Task<string> Md5FileAsync(string fileName)
        {
            var result = await Task.Run(() => Md5File(fileName));
            return result;
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
    }
}
