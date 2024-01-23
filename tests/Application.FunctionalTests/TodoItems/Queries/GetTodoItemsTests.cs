using Stride.Application.TodoItems.Queries.GetTodoItems;
using Stride.Domain.Entities;

using static Stride.Application.FunctionalTests.Testing;

namespace Stride.Application.FunctionalTests.TodoItems.Queries;

public class GetTodoItemsTests : BaseTestFixture
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

        var query = new GetTodoItemsQuery(1);

        TodoItemsVm result = await SendAsync(query);

        result.Items.Should().HaveCount(7);
        result.Items.Last().Id.Should().Be(1);
    }
}
