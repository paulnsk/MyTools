namespace AspNetHelpers.ErrorFiltering
{
    /// <summary>
    /// Allows to specify HTTP (or any other) status code along with error message for further forwarding the code to client
    /// </summary>
    [Serializable]
    public class StatusCodeException : Exception
    {
        public int StatusCode { get; } = 400;

        public StatusCodeException()
        {

        }

        public StatusCodeException(string? message) : base(message)
        {

        }

        public StatusCodeException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        public StatusCodeException(int statusCode, string? message) : base(message)
        {
            StatusCode = statusCode;
        }

        public StatusCodeException(int statusCode, string? message, Exception? innerException) : base(message,
            innerException)
        {
            StatusCode = statusCode;
        }

    }

}
