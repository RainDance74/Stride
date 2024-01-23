using Stride.Application.TodoLists.Commands.CreateTodoList;
using Stride.Application.TodoLists.Commands.DeleteTodoList;
using Stride.Domain.Entities;

using static Stride.Application.FunctionalTests.Testing;

namespace Stride.Application.FunctionalTests.TodoLists.Commands;

public class DeleteTodoListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoList()
    {
        await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await SendAsync(new DeleteTodoListCommand(listId));

        TodoList? list = await FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
}
