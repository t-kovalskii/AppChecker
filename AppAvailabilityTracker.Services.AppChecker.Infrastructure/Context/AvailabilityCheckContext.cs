using System;
using System.Collections.Generic;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Models;
using AppAvailabilityTracker.Shared.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Context;

public partial class AvailabilityCheckContext : DbContext
{
    private const string ConnectionStringName = "DefaultConnection";
    
    public AvailabilityCheckContext()
    {
    }

    private readonly ILogger<AvailabilityCheckContext> _logger;
    private readonly DomainEventDispatcherService _domainEventDispatcherService;
    private readonly IConfiguration _configuration;
    
    public AvailabilityCheckContext(DbContextOptions<AvailabilityCheckContext> options,
        DomainEventDispatcherService domainEventDispatcherService,
        IConfiguration configuration,
        ILogger<AvailabilityCheckContext> logger)
        : base(options)
    {
        _domainEventDispatcherService = domainEventDispatcherService;
        _configuration = configuration;
        _logger = logger;
    }

    public virtual DbSet<AvailabilityCheck> AvailabilityChecks { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(ConnectionStringName));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailabilityCheck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("availability_checks_pkey");

            entity.ToTable("availability_checks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AppLink).HasColumnName("app_link");
            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.CheckResult).HasColumnName("check_result");
            entity.Property(e => e.CheckTime).HasColumnName("check_time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
