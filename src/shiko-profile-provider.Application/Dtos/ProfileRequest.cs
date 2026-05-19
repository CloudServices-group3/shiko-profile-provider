namespace shiko_profile_provider.Application.Dtos;

public record ProfileRequest
(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Description,
    string? ProfileImage
);

