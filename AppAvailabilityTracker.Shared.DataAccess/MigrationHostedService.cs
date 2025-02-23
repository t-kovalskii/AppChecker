using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Shared.DataAccess;

public class MigrationHostedService<TContext>(IServiceProvider serviceProvider) : IHostedService
    where TContext : DbContext
{
    private readonly ILogger<TContext> _logger = serviceProvider.GetRequiredService<ILogger<TContext>>();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Applying migrations for context {ContextName}...", typeof(TContext).Name);
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync(cancellationToken);
        }
        _logger.LogInformation("Migrations applied for context {ContextName}.", typeof(TContext).Name);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

