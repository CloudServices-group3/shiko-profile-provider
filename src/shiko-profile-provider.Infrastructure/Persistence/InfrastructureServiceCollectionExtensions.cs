using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shiko_profile_provider.Application.Abstractions;
using shiko_profile_provider.Infrastructure.Persistence.Contexts;
using shiko_profile_provider.Infrastructure.Persistence.Repositories;

namespace shiko_profile_provider.Infrastructure.Persistence;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        // In memory db
        services.AddSingleton(_ =>
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            return connection;
        });

        // Get memory db service
        services.AddDbContext<DataContext>((sp, options) =>
        {
            var connection = sp.GetRequiredService<SqliteConnection>();
            options.UseSqlite(connection);
        });

        services.AddScoped<IProfileRepository, ProfileRepository>();

        return services;
    }
}
