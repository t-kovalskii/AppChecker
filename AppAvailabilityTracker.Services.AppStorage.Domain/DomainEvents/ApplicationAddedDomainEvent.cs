using AppAvailabilityTracker.Services.AppStorage.Domain.Models;
using AppAvailabilityTracker.Shared.Domain.Events;

namespace AppAvailabilityTracker.Services.AppStorage.Domain.DomainEvents;

public class ApplicationAddedDomainEvent(ApplicationDomain applicationDomain) : DomainEvent
{
    public ApplicationDomain ApplicationDomain { get; } = applicationDomain;
}
