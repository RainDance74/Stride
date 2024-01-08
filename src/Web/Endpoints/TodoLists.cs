using Stride.Application.TodoLists.Commands.CreateTodoList;
using Stride.Application.TodoLists.Commands.DeleteTodoList;
using Stride.Application.TodoLists.Commands.UpdateTodoList;
// using Stride.Application.TodoLists.Queries.GetTodos;

namespace Stride.Web.Endpoints;

public class TodoLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateTodoList)
            .MapPut(UpdateTodoList, "{id}")
            .MapDelete(DeleteTodoList, "{id}");
    }

    public async Task<int> CreateTodoList(ISender sender, CreateTodoListCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<IResult> UpdateTodoList(ISender sender, int id, UpdateTodoListCommand command)
    {
        try
        {
            if (id != command.Id) return Results.BadRequest();
            await sender.Send(command);
            return Results.NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            // TODO: Ideally should return forbiden access in a case if envrimoment is set to debug
            return Results.NotFound();
        }
    }

    public async Task<IResult> DeleteTodoList(ISender sender, int id)
    {
        try
        {
            await sender.Send(new DeleteTodoListCommand(id));
            return Results.NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            // TODO: Ideally should return forbiden access in a case if envrimoment is set to debug
            return Results.NotFound();
        }
    }
}
