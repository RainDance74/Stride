﻿namespace Stride.Domain.Events;

public class TodoItemCompletedEvent(TodoItem item) : BaseEvent
{
    public TodoItem Item { get; } = item;
}
