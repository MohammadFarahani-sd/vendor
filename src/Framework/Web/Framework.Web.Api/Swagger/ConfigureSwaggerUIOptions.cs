using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
namespace Framework.Web.Api.Swagger;

public class ConfigureSwaggerUiOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public ConfigureSwaggerUiOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerUIOptions options)
    {
        _apiVersionDescriptionProvider.ApiVersionDescriptions.ToList().ForEach(c=> options.SwaggerEndpoint($"/swagger/{c.GroupName}/swagger.json", c.GroupName.ToUpperInvariant()));
    }
}