using shiko_profile_provider.Application.Abstractions;
using shiko_profile_provider.Application.Dtos;
using shiko_profile_provider.Domain.Entities;

namespace shiko_profile_provider.Api.Endpoints;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/profiles")
            .WithTags("Profile");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);
    }

    // Get all profiles
    static async Task<IResult> GetAll(IProfileRepository repo, CancellationToken ct)
    {
        var profile = await repo.GetAllAsync();
        return Results.Ok(profile.Select(ToResult));
    }

    // Get profile by Id
    static async Task<IResult> GetById(Guid id, IProfileRepository repo, CancellationToken ct)
    {
        var profile = await repo.GetAsync(x => x.Id == id);
        return profile is null ? Results.NotFound() : Results.Ok(ToResult(profile));
    }

    // Create new profile
    static async Task<IResult> Create(ProfileRequest request, IProfileRepository repo, CancellationToken ct)
    {
        var profile = new ProfileEntity(request.FirstName, request.LastName, request.PhoneNumber, request.Description, request.ProfileImage);
        var created = await repo.CreateAsync(profile);
        return created is null ? Results.Problem() : Results.Created($"/api/profiles/{created.Id}", ToResult(created));
    }

    // Update profile
    static async Task<IResult> Update(Guid id, ProfileRequest request, IProfileRepository repo, CancellationToken ct)
    {
        var profile = await repo.GetAsync(x => x.Id == id);
        if (profile is null)
            return Results.NotFound();

        profile.UpdateProfile(request.FirstName, request.LastName, request.PhoneNumber, request.Description, request.ProfileImage);
        var updated = await repo.UpdateAsync(id, profile);
        return updated is null ? Results.Problem() : Results.Ok(ToResult(updated));
    }

    // Delete profile
    static async Task<IResult> Delete(Guid id, IProfileRepository repo, CancellationToken ct)
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();

    }

    static ProfileResult ToResult(ProfileEntity profile)
        => new(profile.Id, profile.FirstName, profile.LastName, profile.PhoneNumber, profile.Description, profile.ProfileImage);
}
