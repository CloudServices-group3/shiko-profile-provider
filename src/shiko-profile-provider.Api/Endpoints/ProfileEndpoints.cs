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
            .WithTags("Profiles");

        group.MapGet("/", GetAll)
            .WithName("GetAllProfiles")
            .WithSummary("Get all profiles")
            .WithDescription("Returns a list of all profiles.")
            .Produces<IEnumerable<ProfileResult>>();

        group.MapGet("/{id:guid}", GetById)
            .WithName("GetProfileById")
            .WithSummary("Get a profile by ID")
            .WithDescription("Returns a single profile matching the specific ID.")
            .Produces<ProfileResult>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", Create)
            .WithName("CreateProfile")
            .WithSummary("Create a new profile")
            .WithDescription("Creates a new profile and returns the created resource with its assigned ID.")
            .Produces<ProfileResult>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapPut("/{id:guid}", Update)
            .WithName("UpdateProfile")
            .WithSummary("Updated an existing profile")
            .WithDescription("Updates all fields of an existing profile identified by its ID.")
            .Produces<ProfileResult>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("/{id:guid}", Delete)
            .WithName("DeleteProfile")
            .WithSummary("Delete a profile")
            .WithDescription("Permanently deletes the product with the specified ID.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
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
    static async Task<IResult> Create(ProfileRequest request, ClaimsPrincipal user, IProfileRepository repo, CancellationToken ct)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier); // Fetch UserId from Identity
        if (string.IsNullOrWhiteSpace(userId))
            return Results.Unauthorized();

        var exists = await repo.ExistsAsync(x => x.UserId == userId);

        var profile = new ProfileEntity(userId, request.FirstName, request.LastName, request.PhoneNumber, request.Description, request.ProfileImage); // Save UserId in ProfileEntity

        var created = await repo.CreateAsync(profile);

        return Results.Created($"/api/profiles/{created!.Id}", created);
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
