using shiko_profile_provider.Application.Abstractions;
using shiko_profile_provider.Application.Dtos;
using shiko_profile_provider.Domain.Entities;
using System.Security.Claims;

namespace shiko_profile_provider.Api.Endpoints;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/api/profiles")
            
            .RequireAuthorization();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/me", Me);
        group.MapPost("/", Create);
        group.MapPut("/", Update); 
        group.MapDelete("/{id:guid}", Delete);      
    }

    // Me
    static async Task<IResult> Me (ClaimsPrincipal user, IProfileRepository repo, CancellationToken ct = default)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userId))
            return Results.Unauthorized();
        var profile = await repo.GetAsync(x => x.UserId == userId);

        if (profile is null)
            return Results.NotFound();

        return Results.Ok(ToResult(profile));
    }

    // Get all profiles
    static async Task<IResult> GetAll(IProfileRepository repo, CancellationToken ct = default)
    {
        var profile = await repo.GetAllAsync();
        return Results.Ok(profile.Select(ToResult));
    }

    // Get profile by Id
    static async Task<IResult> GetById(Guid id, IProfileRepository repo, CancellationToken ct = default)
    {
        var profile = await repo.GetAsync(x => x.Id == id);
        return profile is null ? Results.NotFound() : Results.Ok(ToResult(profile));
    }

    // Create new profile
    static async Task<IResult> Create(ProfileRequest request, ClaimsPrincipal user, IProfileRepository repo, CancellationToken ct = default)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub"); // Fetch UserId from Identity
        if (string.IsNullOrWhiteSpace(userId))
            return Results.Unauthorized();

        var exists = await repo.ExistsAsync(x => x.UserId == userId);
        if (exists)
            return Results.Conflict("Profile already exists.");

        var profile = new ProfileEntity(userId, request.FirstName, request.LastName, request.PhoneNumber, request.Description, request.ProfileImage); // Save UserId in ProfileEntity

        var created = await repo.CreateAsync(profile);

        return Results.Created($"/api/profiles/{created!.Id}", created);
    }

    // Update profile
    static async Task<IResult> Update(ProfileRequest request, ClaimsPrincipal user, IProfileRepository repo, CancellationToken ct = default)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub"); // Fetch UserId from Identity

        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();

        var profile = await repo.GetAsync(x => x.UserId == userId);
        if (profile is null)
            return Results.NotFound();

        profile.UpdateProfile(request.FirstName, request.LastName, request.PhoneNumber, request.Description, request.ProfileImage);

        await repo.UpdateAsync(profile.Id, profile);
        return Results.Ok(ToResult(profile));
    }

    // Delete profile
    static async Task<IResult> Delete(Guid id, IProfileRepository repo, CancellationToken ct = default)
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    }

    static ProfileResult ToResult(ProfileEntity profile)
        => new(profile.Id, profile.FirstName, profile.LastName, profile.PhoneNumber, profile.Description, profile.ProfileImage);
}
