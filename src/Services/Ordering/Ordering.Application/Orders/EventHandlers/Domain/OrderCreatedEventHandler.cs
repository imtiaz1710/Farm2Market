using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;
public class OrderCreatedEventHandler
    (ILogger<OrderCreatedEventHandler> logger, IFeatureManager featureManager, IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();

        if(await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);

            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
        }
    }
}