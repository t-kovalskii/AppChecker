using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Shared.EventBus.Abstractions;

public interface IEventBusBuilder
{
    IServiceCollection Services { get; }
}