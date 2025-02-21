using AppAvailabilityTracker.Services.AppChecker.Domain.Models;
using AppAvailabilityTracker.Shared.Domain.Events;

namespace AppAvailabilityTracker.Services.AppChecker.Domain.DomainEvents;

public class CheckPerformed(AvailabilityCheck availabilityCheck) : DomainEvent
{
    public AvailabilityCheck AvailabilityCheck { get; } = availabilityCheck;
}
