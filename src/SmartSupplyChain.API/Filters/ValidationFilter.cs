using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartSupplyChain.API.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(
                new
                {
                    Message = "Validation failed.",
                    Errors = context.ModelState
                        .Where(item => item.Value?.Errors.Count > 0)
                        .ToDictionary(
                            item => item.Key,
                            item => item.Value!.Errors
                                .Select(error =>
                                    error.ErrorMessage)
                                .ToArray())
                });
        }
    }

    public void OnActionExecuted(
        ActionExecutedContext context)
    {
    }
}