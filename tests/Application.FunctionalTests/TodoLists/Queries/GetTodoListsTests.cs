using Stride.Application.TodoLists.Queries.GetTodoLists;
using Stride.Domain.Entities;

using static Stride.Application.FunctionalTests.Testing;

namespace Stride.Application.FunctionalTests.TodoLists.Queries;

public class GetTodoListsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        var userId = await RunAsDefaultUserAsync();

        await AddAsync(new TodoList
        {
            Title = "Shopping",
            Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" }
                    },
            Owner = userId
        });

        var query = new GetTodoListsQuery();

        TodoListsVm result = await SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(7);
    }

    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetTodoListsQuery();

        Func<Task<TodoListsVm>> action = () => SendAsync(query);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
