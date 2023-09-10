using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Framework.Web.Api.ExceptionPolicies;

public class ConfigureApiBehaviorOptions : IConfigureOptions<ApiBehaviorOptions>
{
    public void Configure(ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = ctx => new ValidationProblemDetailsResult();
    }
}

public class ValidationProblemDetailsResult : IActionResult
{
    public Task ExecuteResultAsync(ActionContext context)
    {
        var invalidModelStateEntries = context.ModelState.Where(m => m.Value?.Errors.Count > 0).ToArray();

        if (invalidModelStateEntries.Any())
        {
            var firstErrorMessage = invalidModelStateEntries[0].Value?.Errors[0].ErrorMessage!;
            throw new BadHttpRequestException(firstErrorMessage);
        }

        return Task.CompletedTask;
    }
}