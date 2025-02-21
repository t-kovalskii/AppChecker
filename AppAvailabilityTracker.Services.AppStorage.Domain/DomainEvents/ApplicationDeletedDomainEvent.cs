using AppAvailabilityTracker.Services.AppStorage.Domain.Models;
using AppAvailabilityTracker.Shared.Domain.Events;

namespace AppAvailabilityTracker.Services.AppStorage.Domain.DomainEvents;

public class ApplicationDeletedDomainEvent(ApplicationDomain application) : DomainEvent
{
    public ApplicationDomain Application { get; } = application;
}