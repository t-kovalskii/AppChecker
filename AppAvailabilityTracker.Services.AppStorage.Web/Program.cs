using AppAvailabilityTracker.Services.AppStorage.Application.Extensions;
using AppAvailabilityTracker.Services.AppStorage.Application.IntegrationEventHandlers;
using AppAvailabilityTracker.Services.AppStorage.Application.IntegrationEvents;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Extensions;

using AppAvailabilityTracker.Shared.EventBus.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddEventBus(builder.Configuration)
    .AddSubscription<CheckPerformedIntegrationEvent, CheckPerformedIntegrationEventHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
