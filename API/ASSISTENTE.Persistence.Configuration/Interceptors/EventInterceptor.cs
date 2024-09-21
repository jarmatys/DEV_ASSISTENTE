using ASSISTENTE.Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Persistence.Configuration.Interceptors;

public class EventInterceptor(IPublisher publisher, ILogger<EventInterceptor> logger) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var savedChangesResult = await base.SavedChangesAsync(eventData, result, cancellationToken);

        if (eventData.Context is null)
            return savedChangesResult;

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var events = entity.GetEvents();
                entity.ClearEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            logger.LogInformation("Publishing domain event: {EventName}", domainEvent.GetType().Name);

            await publisher.Publish(domainEvent, cancellationToken);
        }

        return savedChangesResult;
    }
}