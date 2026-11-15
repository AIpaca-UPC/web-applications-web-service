using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(
    AccountId AccountId, 
    string FirstName, 
    string LastName, 
    string PhoneNumber, 
    EProfileType ProfileType);
