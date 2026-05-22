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
        if(env.IsDevelopment())
        {
            services.AddSingleton<SqliteConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:;");
                connection.Open();
                return connection;
            });

            services.AddDbContext<DataContext>((sp, options) =>
            {
                var connection = sp.GetRequiredService<SqliteConnection>();
                options.UseSqlite(connection);
            });
        }
        else
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        }

        services.AddScoped<IProfileRepository, ProfileRepository>();

        return services;
    }
}
