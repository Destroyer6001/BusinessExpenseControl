using BusinessExpenseControl.Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BusinessExpenseControl.Presentation.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var firstError = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                context.Result = new OkObjectResult(
                    ApiResponseDTO<object>.ErrorResponse(firstError!)
                );
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
