using Microsoft.EntityFrameworkCore;
using shiko_profile_provider.Application.Abstractions;
using shiko_profile_provider.Domain.Entities;
using shiko_profile_provider.Infrastructure.Persistence.Contexts;
using System.Linq.Expressions;

namespace shiko_profile_provider.Infrastructure.Persistence.Repositories;

public class ProfileRepository(DataContext context) : IProfileRepository
{
    public async Task<ProfileEntity?> CreateAsync(ProfileEntity profile)
    {
        context.Profiles.Add(profile);
        await context.SaveChangesAsync();
        return profile;
    }

    public async Task<ProfileEntity?> UpdateAsync(Guid id, ProfileEntity profile)
    {
        var existing = await context.Profiles.FindAsync(id);
        if (existing is null) return null;

        context.Entry(existing).CurrentValues.SetValues(profile);
        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var profile = await context.Profiles.FindAsync(id);
        if (profile is null) return false;

        context.Profiles.Remove(profile);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Expression<Func<ProfileEntity, bool>> expression) =>
        await context.Profiles.AsNoTracking().AnyAsync(expression);

    public async Task<IEnumerable<ProfileEntity>> GetAllAsync() =>
        await context.Profiles.AsNoTracking().ToListAsync();

    public async Task<ProfileEntity?> GetAsync(Expression<Func<ProfileEntity, bool>> expression) =>
        await context.Profiles.AsNoTracking().FirstOrDefaultAsync(expression);
}
