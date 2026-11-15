using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Events;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.ValueObjects;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;

/// <summary>
///     A vehicle enrolled in the school transport service, together with the documents
///     declared for it.
/// </summary>
public partial class Vehicle : AggregateRoot
{
     private readonly List<Credential> _credentials = [];

    public Vehicle()
    {
        Plate = new LicensePlate();
        Brand = string.Empty;
        Model = string.Empty;
        DriverName = string.Empty;
    }

    public Vehicle(CreateVehicleCommand command)
    {
        Plate = new LicensePlate(command.LicensePlate);
        Brand = command.Brand;
        Model = command.Model;
        Capacity = command.Capacity;
        Year = command.Year;
        Status = command.Status;
        DriverName = command.DriverName;
        _credentials.AddRange(command.Credentials.Select(declaration =>
            new Credential(declaration.Type, declaration.Number, declaration.ExpiresAt)));
        AddDomainEvent(new VehicleRegisteredEvent(Id, Plate));
    }

    public VehicleId Id { get; }

    public LicensePlate Plate { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Capacity { get; private set; }
    public int Year { get; private set; }
    public EVehicleStatus Status { get; private set; }
    public string DriverName { get; private set; }

    /// <summary>
    ///     Gets the documents declared for this vehicle.
    /// </summary>
    public IReadOnlyCollection<Credential> Credentials => _credentials.AsReadOnly();

    public string LicensePlate => Plate.Value;
    public bool IsActive => Status == EVehicleStatus.Active;

    /// <summary>
    ///     Gets a value indicating whether every declared document is verified and in force.
    /// </summary>
    public bool HasAllCredentialsVerified =>
        _credentials.Count > 0 &&
        _credentials.All(credential =>
            credential.DisplayStatus == ECredentialDisplayStatus.Verified);

    /// <summary>
    ///     Applies the data declared by the driver.
    /// </summary>
    /// <remarks>
    ///     Re-declaring a document discards its previous verification outcome: the number or the
    ///     expiry date changed, so what was verified is no longer what is on file.
    /// </remarks>
    /// <param name="command">The declared vehicle data.</param>
    public void Update(UpdateVehicleCommand command)
    {
        Plate = new LicensePlate(command.LicensePlate);
        Brand = command.Brand;
        Model = command.Model;
        Capacity = command.Capacity;
        Year = command.Year;
        Status = command.Status;
        DriverName = command.DriverName;

        _credentials.Clear();
        _credentials.AddRange(command.Credentials.Select(declaration =>
            new Credential(declaration.Type, declaration.Number, declaration.ExpiresAt)));
    }

    /// <summary>
    ///     Records the outcome a verifying service returned for one document.
    /// </summary>
    /// <param name="command">The verification outcome to record.</param>
    /// <returns>
    ///     <c>true</c> when a document of that kind was declared and the outcome was recorded;
    ///     otherwise <c>false</c>.
    /// </returns>
    public bool RegisterCredentialVerification(RegisterCredentialVerificationCommand command)
    {
        var index = _credentials.FindIndex(credential => credential.Type == command.Type);
        if (index < 0) return false;

        _credentials[index] = command.Answered
            ? _credentials[index].WithVerification(command.Verified, command.Source)
            : _credentials[index].AsPending();

        return true;
    }
}