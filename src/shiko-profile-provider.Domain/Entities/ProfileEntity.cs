namespace shiko_profile_provider.Domain.Entities;

public class ProfileEntity
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = null!; // Id from Identity
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? PhoneNumber { get; private set; }
    public string? Description { get; private set; }
    public string? ProfileImage { get; private set; } 

    private ProfileEntity()
    {
    }

    public ProfileEntity(string userId, string firstName, string lastName, string? phoneNumber, string? description, string? profileImage)
    {
        // Controll of the rules.
        SetUserId(userId);
        SetFirstName(firstName);
        SetLastName(lastName);
        SetPhoneNumber(phoneNumber);
        SetDescription(description);
        SetProfileImage(profileImage);
    }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber, string? description, string? profileImage)
    {
        // Controll of the rules.
        SetFirstName(firstName);
        SetLastName(lastName);
        SetPhoneNumber(phoneNumber);
        SetDescription(description);
        SetProfileImage(profileImage);
    }

    public void SetUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));

        UserId = userId.Trim();
    }

    public void SetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Frist name cannot be empty.", nameof(firstName));
        if(firstName.Length > 100)
            throw new ArgumentException("First name cannot exceed 100 characters", nameof(firstName));

        FirstName = firstName.Trim();
    }

    public void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        if (lastName.Length > 100)
            throw new ArgumentException("Last name cannot exceed 100 characters", nameof(lastName));

        LastName = lastName.Trim();
    }

    public void SetPhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber?.Trim();
    }

    public void SetDescription(string? description)
    {
        if (description?.Length > 1000)
            throw new ArgumentException("The description cannot exceed 1000 characters", nameof(description));

        Description = description?.Trim();
    }

    public void SetProfileImage(string? profileImage)
    {
        ProfileImage = profileImage?.Trim();
    }
}
