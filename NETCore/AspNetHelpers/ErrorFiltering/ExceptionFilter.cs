using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetHelpers.ErrorFiltering
{

    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = 400;

            if (context.Exception is StatusCodeException sce)
            {
                statusCode = sce.StatusCode;
            }

            context.HttpContext.Response.StatusCode = statusCode;
        }
        //_todo optionally, for debugging, send exception.Message as content to client. Here or in logurlmiddlware?

    }

}
