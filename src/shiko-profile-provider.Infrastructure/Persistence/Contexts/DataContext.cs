using Microsoft.EntityFrameworkCore;
using shiko_profile_provider.Domain.Entities;

namespace shiko_profile_provider.Infrastructure.Persistence.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();
}
