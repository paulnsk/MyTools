namespace Crypt
{


    public static class CypherExt
    {
        private static string defaultPassword = "0Мастер@Пасворд0";
        private static string _lastusedPassword = string.Empty;


        public static string Enkript(this string @this)
        {
            _lastusedPassword = defaultPassword;
            return StringCipher.Encrypt(@this, defaultPassword);
        }

        public static string Enkript(this string @this, string password)
        {
            _lastusedPassword = password; 
            return StringCipher.Encrypt(@this, password);
        }

        public static string Dekript(this string @this)
        {
            return StringCipher.Decrypt(@this, defaultPassword);
        }

        public static string Dekript(this string @this, string password)
        {
            return StringCipher.Decrypt(@this, password);
        }
    }

}