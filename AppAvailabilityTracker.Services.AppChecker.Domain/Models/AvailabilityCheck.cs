using AppAvailabilityTracker.Services.AppChecker.Domain.DomainEvents;
using AppAvailabilityTracker.Shared.Domain.Models;

namespace AppAvailabilityTracker.Services.AppChecker.Domain.Models;

public class AvailabilityCheck : DomainModel
{
    public AvailabilityCheck(Guid applicationId, Uri appLink, bool checkResult)
    {
        Id = Guid.NewGuid();
        ApplicationId = applicationId;
        AppLink = appLink;
        CheckResult = checkResult;
        CheckTime = DateTime.UtcNow;
        
        AddDomainEvent(new CheckPerformed(this));
    }
        
    public Guid Id { get; }
    
    public Guid ApplicationId { get; }
    
    public Uri AppLink { get; }
    
    public DateTime CheckTime { get; private set; }
    
    public bool CheckResult { get; private set; }
}
