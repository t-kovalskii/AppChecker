using System;
using System.Collections.Generic;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Models;

public partial class AvailabilityCheck
{
    public Guid Id { get; set; }

    public Guid ApplicationId { get; set; }

    public string AppLink { get; set; } = null!;

    public DateTime CheckTime { get; set; }

    public bool CheckResult { get; set; }
}
