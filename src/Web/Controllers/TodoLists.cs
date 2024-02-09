using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Stride.Application.TodoLists.Commands.CreateTodoList;
using Stride.Application.TodoLists.Commands.DeleteTodoList;
using Stride.Application.TodoLists.Commands.UpdateTodoList;
using Stride.Application.TodoLists.Queries.GetTodoLists;

namespace Stride.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoListsController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetTodoLists(ISender sender) => Results.Ok(await sender.Send(new GetTodoListsQuery()));

    [HttpPost]
    public async Task<IResult> CreateTodoList(ISender sender, CreateTodoListCommand command) => Results.Ok(await sender.Send(command));

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateTodoList(ISender sender, int id, UpdateTodoListCommand command)
    {
        if(id != command.Id)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteTodoList(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoListCommand(id));
        return Results.NoContent();
    }
}
