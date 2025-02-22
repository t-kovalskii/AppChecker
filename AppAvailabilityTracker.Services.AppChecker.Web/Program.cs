using AppAvailabilityTracker.Services.AppChecker.Application.Extensions;
using AppAvailabilityTracker.Services.AppChecker.Application.IntegrationEventHandlers;
using AppAvailabilityTracker.Services.AppChecker.Application.IntegrationEvents;
using AppAvailabilityTracker.Services.AppChecker.Domain.Repositories;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Extensions;
using AppAvailabilityTracker.Services.AppChecker.Web.Services;

using AppAvailabilityTracker.Shared.Domain.Configuration;
using AppAvailabilityTracker.Shared.EventBus.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.Configure<ServiceConfiguration>(builder.Configuration.GetSection(nameof(ServiceConfiguration)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddEventBus(builder.Configuration)
    .AddSubscription<ApplicationDeletedIntegrationEvent, ApplicationDeletedIntegrationEventHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<AvailabilityCheckerService>();

app.MapGet("/", async (IAvailabilityChecksRepository availabilityChecksRepository) =>
{
    var availabilityChecks = await availabilityChecksRepository.GetAllAsync();
        
    return Results.Ok(availabilityChecks.Select(check => new
    {
        check.Id,
        check.ApplicationId,
        check.AppLink,
        check.CheckTime
    }));
}).WithName("GetAvailabilityChecks");

app.Run();
