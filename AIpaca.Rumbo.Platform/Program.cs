using System.Text.Json;
using System.Text.Json.Serialization;

using AIpaca.Rumbo.Platform.Resources.Errors;
using AIpaca.Rumbo.Platform.Resources.Shared;
using AIpaca.Rumbo.Platform.Shared.Domain.Repositories;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using AIpaca.Rumbo.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;
using ProblemDetailsFactory = AIpaca.Rumbo.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.Internal.CommandServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.Internal.QueryServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Domain.Repositories;
using AIpaca.Rumbo.Platform.VehicleCredentialManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Repositories;

using AIpaca.Rumbo.Platform.Profiles.Application.CommandServices;
using AIpaca.Rumbo.Platform.Profiles.Application.Internal.CommandServices;
using AIpaca.Rumbo.Platform.Profiles.Application.Internal.QueryServices;
using AIpaca.Rumbo.Platform.Profiles.Application.QueryServices;
using AIpaca.Rumbo.Platform.Profiles.Domain.Repositories;
using AIpaca.Rumbo.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.Internal.CommandServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.Internal.QueryServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Domain.Repositories;
using AIpaca.Rumbo.Platform.RouteScheduleManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.CommandServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.Internal.CommandServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.Internal.QueryServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Application.QueryServices;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Domain.Repositories;
using AIpaca.Rumbo.Platform.AlertingIncidentManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.CommandServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.Internal.CommandServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.Internal.QueryServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Application.QueryServices;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Domain.Repositories;
using AIpaca.Rumbo.Platform.SubscriptionBilling.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper, allowIntegerValues: false)));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// CORS: los orígenes reales se restringen antes de desplegar.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .AddInterceptors(serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.AddOpenApi();

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

// Vehicle & Credential Management Bounded Context
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();

// Profiles & Relationship Management Bounded Context
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentQueryService, StudentQueryService>();
builder.Services.AddScoped<IStudentCommandService, StudentCommandService>();
builder.Services.AddScoped<IDataDeletionRequestRepository, DataDeletionRequestRepository>();
builder.Services.AddScoped<IDataDeletionRequestQueryService, DataDeletionRequestQueryService>();
builder.Services.AddScoped<IDataDeletionRequestCommandService, DataDeletionRequestCommandService>();

// Route & Schedule Management Bounded Context
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteCommandService, RouteCommandService>();
builder.Services.AddScoped<IRouteQueryService, RouteQueryService>();

// Trip Execution & Monitoring Bounded Context

// Alerting & Incident Management Bounded Context
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationCommandService, NotificationCommandService>();
builder.Services.AddScoped<INotificationQueryService, NotificationQueryService>();

// Subscription & Billing Bounded Context
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPlanCommandService, PlanCommandService>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<IPlanQueryService, PlanQueryService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

// Identity & Access Management Bounded Context


builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(ExceptionHandlingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(TransactionCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseExceptionHandler();

var supportedCultures = new[] { "en", "es" };
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures));


app.MapOpenApi();
app.MapScalarApiReference();


app.UseCors("AllowAllPolicy");

if(app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();