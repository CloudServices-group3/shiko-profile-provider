namespace shiko_profile_provider.Api.OpenApi;

public static class OpenApiConfiguration
{
    public static IServiceCollection AddProfileProviderOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<ProfileProviderDocumentTransform>();
        });

        return services;
    }
}
