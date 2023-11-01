using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MyTools
{
    public static class ApiCLientUtils
    {
        [Obsolete("no longer works in .net 7")]
        public static HttpClient CreateHttpClientWithAuthentication(string user, string password)
        {
            var result = new HttpClient();

            if (!string.IsNullOrEmpty(user))
            {
                var credentials = Encoding.ASCII.GetBytes(user + ":" + password);
                var credentialsBase64 = Convert.ToBase64String(credentials);
                result.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentialsBase64);
            }

            return result;
        }
    }
}
