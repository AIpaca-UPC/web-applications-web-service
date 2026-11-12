using AIpaca.Rumbo.Platform.Resources.Errors;
using AIpaca.Rumbo.Platform.Shared.Application.Model;
using AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Aggregates;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Model.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AIpaca.Rumbo.Platform.VehicleCredentialManagement.Interfaces.Rest.Transform;

/// <summary>
///     Turns the outcome of the application layer into an HTTP response.
/// </summary>
public static class VehiclesActionsResultAssembler
{
    private static int ToStatusCodeFromVehiclesError(VehiclesError error) => error switch
    {
        VehiclesError.VehicleNotFound => StatusCodes.Status404NotFound,
        VehiclesError.LicensePlateAlreadyRegistered => StatusCodes.Status409Conflict,
        VehiclesError.CredentialNotDeclared => StatusCodes.Status404NotFound,
        VehiclesError.InvalidVehicleData => StatusCodes.Status400BadRequest,
        VehiclesError.OperationCancelled => StatusCodes.Status409Conflict,
        VehiclesError.DatabaseError => StatusCodes.Status500InternalServerError,
        VehiclesError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    /// <summary>
    ///     Turns the result of any command of this context into an HTTP response.
    /// </summary>
    public static IActionResult ToActionResultFromCommandResult(
        ControllerBase controller,
        Result<Vehicle> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Vehicle, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromVehiclesError((VehiclesError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>
    ///     Turns the outcome of a lookup by identifier into an HTTP response.
    /// </summary>
    public static IActionResult ToActionResultFromGetVehicleByIdResult(
        ControllerBase controller,
        Vehicle? vehicle,
        IStringLocalizer errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Vehicle, IActionResult> successAction)
    {
        if (vehicle is not null) return successAction(vehicle);

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromVehiclesError(VehiclesError.VehicleNotFound),
            VehiclesError.VehicleNotFound,
            errorLocalizer[nameof(VehiclesError.VehicleNotFound)]);
    }
}