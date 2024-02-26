using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BowlingParkMicroService.Helpers;

public class AppExceptionFiltersAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context) 
    {
        if (context.Exception is AppException exception)
        {
            var errorResponse = new 
            {
                error = exception.Message
            };
            
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = exception.ErrorCode
            };
        }
    }
}