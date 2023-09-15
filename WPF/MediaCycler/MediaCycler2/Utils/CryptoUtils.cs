using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MediaCycler2.Utils
{
    internal static class CryptoUtils
    { 
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
