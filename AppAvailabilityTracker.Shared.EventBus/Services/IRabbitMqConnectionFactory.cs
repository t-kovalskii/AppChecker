using RabbitMQ.Client;

namespace AppAvailabilityTracker.Shared.EventBus.Services;

public interface IRabbitMqConnectionFactory
{ 
    Task<IConnection> EstablishConnectionAsync();
}