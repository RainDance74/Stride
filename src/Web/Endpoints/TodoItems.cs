﻿using Stride.Application.TodoItems.Commands.CreateTodoItem;
using Stride.Application.TodoItems.Commands.DeleteTodoItem;
using Stride.Application.TodoItems.Commands.UpdateTodoItem;
using Stride.Application.TodoItems.Commands.UpdateTodoItemDetail;
using Stride.Application.TodoItems.Queries.GetTodoItems;

namespace Stride.Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetTodoItems, "{listId}")
            .MapPost(CreateTodoItem)
            .MapPut(UpdateTodoItem, "{id}")
            .MapPut(UpdateTodoItemDetail, "UpdateDetail/{id}")
            .MapDelete(DeleteTodoItem, "{id}");
    }

    public async Task<TodoItemsVm> GetTodoItems(ISender sender, int listId) => await sender.Send(new GetTodoItemsQuery(listId));

    public async Task<IResult> CreateTodoItem(ISender sender, CreateTodoItemCommand command) => Results.Ok(await sender.Send(command));

    public async Task<IResult> UpdateTodoItem(ISender sender, int id, UpdateTodoItemCommand command)
    {
        if(id != command.Id)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> UpdateTodoItemDetail(ISender sender, int id, UpdateTodoItemDetailCommand command)
    {
        if(id != command.Id)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteTodoItem(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));
        return Results.NoContent();
    }
}
