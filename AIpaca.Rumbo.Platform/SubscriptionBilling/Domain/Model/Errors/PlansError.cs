namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Errors;

/// <summary>
///     Represents errors related to subscription plans
///     in the Subscription & Billing bounded context.
/// </summary>
public enum PlansError
{
    /// <summary>No error occurred.</summary>
    None = 0,

    /// <summary>The requested plan was not found.</summary>
    PlanNotFound = 1,

    /// <summary>A plan with the same name already exists.</summary>
    PlanAlreadyExists = 2,

    /// <summary>The supplied plan data is invalid.</summary>
    InvalidPlanData = 3
}