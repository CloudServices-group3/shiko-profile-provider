using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace shiko_profile_provider.Api.OpenApi;

public static class OpenApiEndpointsExtensions
{
    public static WebApplication MapOpenApiEndpoints(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.MapOpenApi();

        app.MapScalarApiReference("/scalar", options => options
            .WithTitle("Documentation for Profile Provider API"));       

        return app;
    }
}
