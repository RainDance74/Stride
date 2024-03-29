﻿using Stride.Application.TodoLists.Commands.CreateTodoList;
using Stride.Application.TodoLists.Commands.DeleteTodoList;
using Stride.Application.TodoLists.Commands.UpdateTodoList;
using Stride.Application.TodoLists.Queries.GetTodoLists;

namespace Stride.Web.Endpoints;

public class TodoLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetTodoLists)
            .MapPost(CreateTodoList)
            .MapPut(UpdateTodoList, "{id}")
            .MapDelete(DeleteTodoList, "{id}");
    }

    public async Task<TodoListsVm> GetTodoLists(ISender sender) => await sender.Send(new GetTodoListsQuery());

    public async Task<IResult> CreateTodoList(ISender sender, CreateTodoListCommand command) => Results.Ok(await sender.Send(command));

    public async Task<IResult> UpdateTodoList(ISender sender, int id, UpdateTodoListCommand command)
    {
        if(id != command.Id)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteTodoList(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoListCommand(id));
        return Results.NoContent();
    }
}
