using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MediaCycler.Utils
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
