using System;
using System.Collections.Generic;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Models;
using AppAvailabilityTracker.Shared.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Context;

public partial class ApplicationStoreContext : DbContext
{
    public ApplicationStoreContext()
    {
    }

    private readonly ILogger<ApplicationStoreContext> _logger;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly DomainEventDispatcherService _domainEventDispatcherService;

    public ApplicationStoreContext(DbContextOptions<ApplicationStoreContext> options,
        DomainEventDispatcherService domainEventDispatcherService,
        ILogger<ApplicationStoreContext> logger,
        IDomainEventDispatcher domainEventDispatcher)
        : base(options)
    {
        _logger = logger;
        _domainEventDispatcher = domainEventDispatcher;
        _domainEventDispatcherService = domainEventDispatcherService;
    }

    public virtual DbSet<Models.Application> Applications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("applications_pkey");

            entity.ToTable("applications");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IsAvailable).HasColumnName("is_available");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Store).HasColumnName("store");
            entity.Property(e => e.StoreLink).HasColumnName("store_link");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
