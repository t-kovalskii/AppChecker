using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Jobs;
using Hangfire;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Services;

public class HangfireRecurringJobScheduler : IRecurringJobScheduler
{
    public void ScheduleRecurringAvailabilityCheckJob(Guid applicationId, Uri appLink, TimeSpan interval)
    {
        const string cronMinutesTemplate = "*/{0} * * * *";
        var cronExpression = string.Format(cronMinutesTemplate, (int)interval.TotalMinutes);
        
        RecurringJob.AddOrUpdate<AvailabilityCheckJob>(
            applicationId.ToString(),
            job => job.ExecuteAsync(applicationId, appLink),
            cronExpression);
    }

    public void RemoveRecurringJob(Guid applicationId)
    {
        RecurringJob.RemoveIfExists(applicationId.ToString());
    }
}