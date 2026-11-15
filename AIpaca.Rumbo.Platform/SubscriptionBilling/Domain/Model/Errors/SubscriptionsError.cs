namespace AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Model.Errors;

/// <summary>
///     Represents business errors within the
///     Subscription & Billing bounded context.
/// </summary>
public enum SubscriptionsError
{
    /// <summary>No error occurred.</summary>
    None = 0,

    /// <summary>The requested subscription was not found.</summary>
    SubscriptionNotFound = 1,

    /// <summary>The selected subscription plan was not found.</summary>
    PlanNotFound = 2,

    /// <summary>The selected plan cannot be activated.</summary>
    PlanUnavailable = 3,

    /// <summary>The driver already has a current subscription.</summary>
    SubscriptionAlreadyExists = 4,

    /// <summary>The supplied subscription data is invalid.</summary>
    InvalidSubscriptionData = 5
}