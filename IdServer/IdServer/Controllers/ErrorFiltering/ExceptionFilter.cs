using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IdServer.Controllers.ErrorFiltering
{

    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {

            if (context.Exception is StatusCodeException sce)
            {
                context.Result = new ObjectResult(ToStringWithInners(sce)) { StatusCode = sce.StatusCode };                
            }
        }


        private static string ToStringWithInners(Exception @this)
        {
            if (@this == null) return "";
            var result = "[" + @this.GetType() + "]: " + @this.Message;
            if (@this.InnerException != null) result += "; Inner: " + ToStringWithInners(@this.InnerException);
            return result;
        }
    }

}
