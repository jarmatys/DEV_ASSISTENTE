using MassTransit;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Playground.Commons;

public sealed class DummyPublishEndpoint(ILogger<DummyPublishEndpoint> logger) : IPublishEndpoint
{
    public ConnectHandle ConnectPublishObserver(IPublishObserver observer)
    {
        logger.LogDebug("ConnectPublishObserver");

        return null!;
    }

    public Task Publish<T>(T message, CancellationToken cancellationToken = new()) where T : class
    {
        logger.LogDebug($"Publish: {nameof(message)}");

        return Task.CompletedTask;
    }

    public Task Publish<T>(T message, IPipe<PublishContext<T>> publishPipe, CancellationToken cancellationToken = new())
        where T : class
    {
        logger.LogDebug($"Publish: {nameof(message)}");

        return Task.CompletedTask;
    }

    public Task Publish<T>(T message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = new())
        where T : class
    {
        logger.LogDebug($"Publish: {nameof(message)}");

        return Task.CompletedTask;
    }

    public Task Publish(object message, CancellationToken cancellationToken = new())
    {
        logger.LogDebug($"Publish: {nameof(message)}");

        return Task.CompletedTask;
    }

    public Task Publish(object message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = new())
    {
        logger.LogDebug($"Publish: {nameof(message)}");

        return Task.CompletedTask;
    }

    public Task Publish(object message, Type messageType, CancellationToken cancellationToken = new())
    {
        logger.LogDebug($"Publish: {nameof(message)}");

        return Task.CompletedTask;
    }

    public Task Publish(
        object message, 
        Type messageType, 
        IPipe<PublishContext> publishPipe,
        CancellationToken cancellationToken = new())
    {
        logger.LogDebug($"Publish: {nameof(message)}");

        return Task.CompletedTask;
    }

    public Task Publish<T>(object values, CancellationToken cancellationToken = new()) 
        where T : class
    {
        logger.LogDebug($"Publish: {nameof(values)}");

        return Task.CompletedTask;
    }

    public Task Publish<T>(
        object values, 
        IPipe<PublishContext<T>> publishPipe,
        CancellationToken cancellationToken = new()) where T : class
    {
        logger.LogDebug($"Publish: {nameof(values)}");

        return Task.CompletedTask;
    }

    public Task Publish<T>(
        object values, 
        IPipe<PublishContext> publishPipe,
        CancellationToken cancellationToken = new()) where T : class
    {
        logger.LogDebug($"Publish: {nameof(values)}");

        return Task.CompletedTask;
    }
}