using AppAvailabilityTracker.Shared.Domain.Models;

using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Shared.Domain.Events;

public class DomainEventDispatcherService(
    IDomainEventDispatcher domainEventDispatcher,
    ILogger<DomainEventDispatcherService> logger)
{
    public async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Dispatching domain events");

        var registeredDomainModels = DomainModel.GetRegisteredModels().ToList();
        while (registeredDomainModels.Count != 0)
        {
            foreach (var domainModel in registeredDomainModels)
            {
                var domainEvents = domainModel.DomainEvents.ToList();
                DomainModel.RemoveRegisteredModel(domainModel);

                foreach (var domainEvent in domainEvents)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await domainEventDispatcher.DispatchAsync(domainEvent);
                }
            }

            registeredDomainModels = DomainModel.GetRegisteredModels().ToList();
        }

        DomainModel.ClearRegisteredModels();

        logger.LogInformation("Domain events dispatched");
    }
}
