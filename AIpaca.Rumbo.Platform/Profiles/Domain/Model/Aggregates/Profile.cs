using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;

public partial class Profile : AggregateRoot
{
    public Profile()
    {
        Name = new PersonName();
    }
    
    public Profile(AccountId accountId, string firstName, string lastName, 
        string phoneNumber, EProfileType profileType)
    {
        AccountId = accountId;
        Name = new PersonName(firstName, lastName);
        PhoneNumber = new PhoneNumber(phoneNumber);
        ProfileType = profileType;
    }
    
    public Profile(CreateProfileCommand command)
    {
        AccountId = command.AccountId;
        Name = new PersonName(command.FirstName, command.LastName);
        PhoneNumber = new PhoneNumber(command.PhoneNumber);
        ProfileType = command.ProfileType;
    }

    public ProfileId Id { get; private set; }
    
    public AccountId AccountId { get; private set; }
    public PersonName Name { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public EProfileType ProfileType { get; private set; }
    
    public string FullName => Name.FullName;
}
