namespace shiko_profile_provider.Application.Dtos;

public record ProfileResult
(
    Guid Id,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Description,
    string? ProfileImage
);

