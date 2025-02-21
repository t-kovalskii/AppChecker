using AppAvailabilityTracker.Services.AppStorage.Domain.Models;
using AppAvailabilityTracker.Shared.DataAccess.Interfaces;

namespace AppAvailabilityTracker.Services.AppStorage.Domain.Repositories;

public interface IApplicationRepository : IRepository<ApplicationDomain>;
