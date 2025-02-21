namespace AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;

public interface IRecurringJobScheduler
{
    void ScheduleRecurringAvailabilityCheckJob(Guid applicationId, Uri appLink, TimeSpan interval);
    
    void RemoveRecurringJob(Guid applicationId);
}
