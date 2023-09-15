using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
namespace MediaCycler2.Utils
{

    //This is a .net framework c# 7.3 version. There are newer versions somewhere in repos. Removed cancellationtoken (not supported in old .net), added DownloadFile()


    /// <summary>
    /// Wrapper for HTTPCLient. Similar to HttpExecutor but with ability to expire httpclientstance when a critical number of errors has occured
    /// </summary>
    public static class HttpEx
    {

        #region Settings
        public static int LifeSpanMinutes { get; set; } = 6;
        public static int MaxErrorsBeforeExpire { get; set; } = 5;
        public static int TimeoutSeconds { get; set; } = 20;
        #endregion

        #region HttpClientMethods
        //Add more methods as needed

        public static Task<string> GetStringAsync(string requestUri)
        {
            return ExecuteHandled(() => Client.GetStringAsync(requestUri));
        }

        public static Task DownloadFile(string url, string localFilePath)
        {
            return ExecuteHandled(async () =>
            {
                using (var stream = await Client.GetStreamAsync(url))
                {
                    using (var fileStream = new FileStream(localFilePath, FileMode.CreateNew))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                return true;
            });
        }


        #endregion

        public static void CloseAllConnections()
        {
            if (_client != null)
            {
                BeginDispose(_client);
            }
            _client = null;
        }

        private const int DisposeTimeoutSeconds = 180;

        private static HttpClient _client;
        private static DateTime createdOn;
        private static int _errorCounter;

        private static HttpClient Client
        {
            get
            {
                if (_client == null || Expired)
                {
                    RenewInstance();
                }
                return _client;
            }
        }

        private static bool Expired => (DateTime.UtcNow - createdOn) > TimeSpan.FromMinutes(LifeSpanMinutes);

        private static void ExpireNow()
        {
            createdOn = DateTime.UtcNow.AddMinutes(-LifeSpanMinutes * 2);
        }

        private static void RenewInstance()
        {
            if (_client != null)
            {
                BeginDispose(_client);
            }
            _client = MakeClient();
            createdOn = DateTime.UtcNow;
        }

        private static HttpClient MakeClient()
        {
            //unlike ServicePointManager, this works under both .NET Framework 4.x and .NET core

            //var handler = new HttpClientHandler();
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(TimeoutSeconds)
            };
            client.DefaultRequestHeaders.ConnectionClose = false;
            return client;
        }

        private static void BeginDispose(HttpClient client)
        {
            var timer = new Timer { Interval = DisposeTimeoutSeconds * 1000 };

            timer.Elapsed += (σ, ε) =>
            {
                ((Timer)σ).Stop();
                ((Timer)σ).Dispose();
                client.Dispose();
            };
            timer.Start();
        }

        private static readonly object _handleErrorLock = new object();
        private static void HandleError()
        {
            lock (_handleErrorLock)
            {
                _errorCounter++;
                if (_errorCounter >= MaxErrorsBeforeExpire)
                {
                    _errorCounter = 0;
                    ExpireNow();
                }
            }
        }
        private static Task<T> ExecuteHandled<T>(Func<Task<T>> httpMethod)
        {
            try
            {
                return httpMethod();
            }
            catch (Exception)
            {
                HandleError();
                throw;
            }
        }

    }
}
