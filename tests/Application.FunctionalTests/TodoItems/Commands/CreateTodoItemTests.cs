using Stride.Application.TodoItems.Commands.CreateTodoItem;
using Stride.Application.TodoLists.Commands.CreateTodoList;
using Stride.Domain.Entities;

using static Stride.Application.FunctionalTests.Testing;

namespace Stride.Application.FunctionalTests.TodoItems.Commands;

public class CreateTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var command = new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Tasks"
        };

        var itemId = await SendAsync(command);

        TodoItem? item = await FindAsync<TodoItem>(itemId);

        item.Should().NotBeNull();
        item!.Title.Should().Be(command.Title);
        item.CreatedBy.Should().Be(userId);
        item.CreatedDateTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.UpdatedBy.Should().Be(null);
        item.UpdatedDateTime.Should().Be(null);
    }
}
