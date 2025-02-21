using AppAvailabilityTracker.Services.AppChecker.Domain.Models;
using AppAvailabilityTracker.Shared.DataAccess.Interfaces;

namespace AppAvailabilityTracker.Services.AppChecker.Domain.Repositories;

public interface IAvailabilityChecksRepository : IRepository<AvailabilityCheck>;