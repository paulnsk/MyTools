using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public class ApiClient
    {

        #region Public

        public bool ThrowHttpExceptions { get; set; } = false;


        /// <summary>
        /// Optional: values will be ignored unless they are set before calling ctor()
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Optional: values will be ignored unless they are set before calling ctor()
        /// </summary>
        public string Password { get; set; }


        public ApiClient()
        {
            Http = ApiCLientUtils.CreateHttpClientWithAuthentication(User, Password);
        }

        public ApiClient(string user, string password)
        {
            User = user;
            Password = password;
            Http = ApiCLientUtils.CreateHttpClientWithAuthentication(user, password);
        }

        public ApiClientError LastError => _lastError;


        public event EventHandler<ApiCLientLogEventArgs> Log;

        #endregion

        #region private 

        protected HttpClient Http;
        private ApiClientError _lastError;

        protected virtual void LogEvent(string s, ConsoleColor color)
        {
            Log?.Invoke(this, new ApiCLientLogEventArgs { S = s, Color = color });
        }

        private void LogHeaders(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers, bool incoming)
        {
            var prefix = " -> ";
            var color = ConsoleColor.Green;
            if (incoming)
            {
                prefix = " <- ";
                color = ConsoleColor.Blue;
            }
            foreach (var header in headers)
            {
                LogEvent(prefix + header.Key + " = " + string.Join(",", header.Value), color);
            }
        }


        // most of the code originnaly wrriten for SolrTools
        protected async Task<string> ApiGet(string url)
        {
            _lastError = null;
            LogEvent("HTTP GET: " + url, ConsoleColor.Green);
            
            try
            {
                using (var response = await Http.GetAsync(url))
                {
                    LogHeaders(response.RequestMessage.Headers, false);
                    LogHeaders(response.Headers.Concat(response.Content.Headers), true);

                    //Success:
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        LogEvent("Result: [" + result.Length + " chars]", ConsoleColor.DarkBlue);
                        return result;
                    }

                    _lastError = new ApiClientError(response);
                    //Error
                    LogEvent("ERROR: " + response.ReasonPhrase, ConsoleColor.Red);
                    LogEvent("Error content: " + response.Content.ReadAsStringAsync().Result, ConsoleColor.DarkRed);
                    return "";
                }

            }
            catch (Exception e)
            {
                LogEvent("ApiGet ERROR: " + e.ToStringWithInner(), ConsoleColor.Red);
                if (ThrowHttpExceptions) throw;
                return "";
            }
        }


        /// <summary>
        /// Returns server's response as Ienumerable<string> while it is being transferred. This is different from ApiGet() which reads the whole response into RAM first
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected IEnumerable<string> ApiGetLines(string url)
        {
            LogEvent("HTTP GET (by lines): " + url, ConsoleColor.Green);

            //can't do try..catch with yield return so errors need to be handled outside foreach look that consumes this

            _lastError = null;
            using (var responseStream = Http.GetStreamAsync(url).Result)
            {
                using (var sr = new StreamReader(responseStream))
                {
                    //todo LastError
                    while (!sr.EndOfStream)
                    {
                        yield return sr.ReadLine();
                        //Konsole.WriteLine(sr.ReadLine()); 
                    }
                }
            }
        }

        protected async Task<string> ApiPost(string url, string postData)
        {

            LogEvent("HTTP POST: " + url, ConsoleColor.Green);
            var displayData = postData.Length <= 1000 ? postData : $"[{postData.Length:N0} bytes]";

            LogEvent("HTTP POST data: " + displayData, ConsoleColor.Green);
            
            _lastError = null;
            try
            {
                using (var response = await Http.PostAsync(url, new StringContent(postData)))
                {
                    LogHeaders(response.RequestMessage.Headers, false);
                    LogHeaders(response.Headers.Concat(response.Content.Headers), true);

                    //Success:
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        LogEvent("Result: [" + result.Length + " chars]", ConsoleColor.DarkBlue);
                        return result;
                    }

                    _lastError = new ApiClientError(response);
                    //Error
                    LogEvent("ERROR: " + response.ReasonPhrase, ConsoleColor.Red);
                    LogEvent("Error content: " + response.Content.ReadAsStringAsync().Result, ConsoleColor.DarkRed);
                    return "";
                }
            }
            catch (Exception e)
            {
                LogEvent("ApiPost ERROR: " + e.ToStringWithInner(), ConsoleColor.Red);
                if (ThrowHttpExceptions) throw;
                return "";
            }

        }

        #endregion private 



    }

    public class ApiCLientLogEventArgs : EventArgs
    {
        public string S { get; set; }
        public ConsoleColor Color { get; set; }
    }

}
