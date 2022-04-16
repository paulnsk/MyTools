using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{

    /// <summary>
    /// Container class to copy HttpClient error response to so it can survive longer than HttpCLient instance which is usially killed by the end of using() block
    /// </summary>
    public class ApiClientError
    {
        public string ReasonPhrase { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }

        public ApiClientError(HttpResponseMessage responseMessage)
        {
            ReasonPhrase = responseMessage.ReasonPhrase;
            StatusCode = responseMessage.StatusCode;
            Content = responseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
