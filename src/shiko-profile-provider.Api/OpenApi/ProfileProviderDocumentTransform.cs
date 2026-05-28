using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace shiko_profile_provider.Api.OpenApi;

public sealed class ProfileProviderDocumentTransform : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        foreach (var path in document.Paths.Values)
        {
            if (path.Operations is null) continue;

            foreach(var operation in path.Operations)
            {
                operation.Value.Security ??= [];
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("ApiKey", document)] = []
                });
            }
        }

        return Task.CompletedTask;
    }
}
