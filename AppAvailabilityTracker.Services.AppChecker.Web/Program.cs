using AppAvailabilityTracker.Services.AppChecker.Application.Extensions;
using AppAvailabilityTracker.Services.AppChecker.Application.IntegrationEventHandlers;
using AppAvailabilityTracker.Services.AppChecker.Application.IntegrationEvents;
using AppAvailabilityTracker.Services.AppChecker.Domain.Repositories;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Extensions;
using AppAvailabilityTracker.Services.AppChecker.Web.Services;

using AppAvailabilityTracker.Shared.Domain.Configuration;
using AppAvailabilityTracker.Shared.EventBus.Extensions;
using Hangfire;

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

app.MapGrpcService<AvailabilityCheckerService>();

app.Run();
