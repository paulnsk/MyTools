namespace MyTools.Crypt
{


    public static class CypherUtils
    {
        private static string _currentPassword = string.Empty;

        public static string CurrentPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_currentPassword)) throw new Exception("Current password is empty!");
                return _currentPassword;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new Exception("Password cannot be empty!");
                _currentPassword = value;
            }
        }

        public static string Enkript(this string @this)
        {
            return StringCipher.Encrypt(@this, CurrentPassword);
        }

        public static string Dekript(this string @this)
        {
            return StringCipher.Decrypt(@this, CurrentPassword);
        }


        public static bool TryDekript(this string @this, out string result)
        {
            try
            {
                result = StringCipher.Decrypt(@this, CurrentPassword);
                return true;
            }
            catch (Exception)
            {
                result = "";
                return false;
            }

        }
    }

}