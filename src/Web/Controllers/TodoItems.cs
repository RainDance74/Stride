using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Stride.Application.TodoItems.Commands.CreateTodoItem;
using Stride.Application.TodoItems.Commands.DeleteTodoItem;
using Stride.Application.TodoItems.Commands.UpdateTodoItem;
using Stride.Application.TodoItems.Queries.GetTodoItems;

namespace Stride.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoItemsController : ControllerBase
{
    [HttpGet("{listId:int}")]
    public async Task<IResult> GetTodoItems(ISender sender, int listId) => Results.Ok(await sender.Send(new GetTodoItemsQuery(listId)));

    [HttpPost]
    public async Task<IResult> CreateTodoItem(ISender sender, CreateTodoItemCommand command) => Results.Ok(await sender.Send(command));

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateTodoItem(ISender sender, int id, UpdateTodoItemCommand command)
    {
        if(id != command.Id)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteTodoItem(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));
        return Results.NoContent();
    }
}
