using Stride.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Stride.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger) : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger = logger;

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stride Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
