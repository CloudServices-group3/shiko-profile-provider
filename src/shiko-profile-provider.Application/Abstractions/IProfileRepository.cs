using shiko_profile_provider.Domain.Entities;
using System.Linq.Expressions;

namespace shiko_profile_provider.Application.Abstractions;

public interface IProfileRepository
{
    Task<ProfileEntity?> CreateAsync(ProfileEntity profile);
    Task<bool> DeleteAsync(Guid id);

    // x => x.Id == id    x => x.Name == name
    Task<bool> ExistsAsync(Expression<Func<ProfileEntity, bool>> expression);
    Task<IEnumerable<ProfileEntity>> GetAllAsync();
    Task<ProfileEntity?> GetAsync(Expression<Func<ProfileEntity, bool>> expression);
    //Task<ProfileEntity?> UpdateAsync(Guid id, ProfileEntity profile);

    Task SaveChangesAsync();
}
