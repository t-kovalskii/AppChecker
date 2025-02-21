using System;
using System.Collections.Generic;

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Models;

public partial class Application
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string StoreLink { get; set; } = null!;

    public string Store { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public DateTime UpdatedAt { get; set; }
}
