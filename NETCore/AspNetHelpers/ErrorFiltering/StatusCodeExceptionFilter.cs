using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetHelpers.ErrorFiltering
{

    /// <summary>
    /// Provides verbal responses to client for <see cref="StatusCodeException"/>. Assigns 500 to all other errors
    /// </summary>
    public class StatusCodeExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = 500;

            if (context.Exception is StatusCodeException sce)
            {
                statusCode = sce.StatusCode;

                context.Result = new ObjectResult(sce.Message)
                {
                    StatusCode = sce.StatusCode
                };
            }

            context.HttpContext.Response.StatusCode = statusCode;

        }
    }

}
