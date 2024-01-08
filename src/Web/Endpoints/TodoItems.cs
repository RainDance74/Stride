﻿using Stride.Application.Common.Models;
using Stride.Application.TodoItems.Commands.CreateTodoItem;
using Stride.Application.TodoItems.Commands.DeleteTodoItem;
using Stride.Application.TodoItems.Commands.UpdateTodoItem;
using Stride.Application.TodoItems.Commands.UpdateTodoItemDetail;

namespace Stride.Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateTodoItem)
            .MapPut(UpdateTodoItem, "{id}")
            .MapPut(UpdateTodoItemDetail, "UpdateDetail/{id}")
            .MapDelete(DeleteTodoItem, "{id}");
    }

    public async Task<int> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        try
        {
            return await sender.Send(command);
        }
        catch (UnauthorizedAccessException)
        {
            return 0;
        }
    }

    public async Task<IResult> UpdateTodoItem(ISender sender, int id, UpdateTodoItemCommand command)
    {
        try
        { 
            if (id != command.Id) return Results.BadRequest();
            await sender.Send(command);
            return Results.NoContent();
        }
        catch(UnauthorizedAccessException)
        {
            // TODO: Ideally should return forbiden access in a case if envrimoment is set to debug
            return Results.NotFound();
        }
    }

    public async Task<IResult> UpdateTodoItemDetail(ISender sender, int id, UpdateTodoItemDetailCommand command)
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

    public async Task<IResult> DeleteTodoItem(ISender sender, int id)
    {
        try
        {
            await sender.Send(new DeleteTodoItemCommand(id));
            return Results.NoContent();
        }
        catch(UnauthorizedAccessException)
        {
            // TODO: Ideally should return forbiden access in a case if envrimoment is set to debug
            return Results.NotFound();
        }
    }
}
