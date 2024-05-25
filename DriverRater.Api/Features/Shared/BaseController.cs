namespace DriverRater.Api.Features.Shared;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DriverRater.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class BaseController(IMediator mediator, IMapper mapper) : ControllerBase
{
    protected async Task<IActionResult> ExecuteQuery<TCommand, TMappedResult>(params object[] models) where TCommand : new()
    {
        return Ok(await ExecuteMediatorRequest<TCommand, TMappedResult>(models));
    }

    protected async Task<IActionResult> ExecuteCommand<TCommand>(params object[] models)
    {
        var command = MapperExtensions.Map<TCommand>(mapper, models);
        await mediator.Send(command!);
        return NoContent();
    }

    protected async Task<IActionResult> ExecuteCommand<TCommand, TMappedResult>(params object[] models) where TCommand : new()
    {
        return Ok(await ExecuteMediatorRequest<TCommand, TMappedResult>(models));
    }

    protected async Task<TMappedResult> ExecuteMediatorRequest<TRequest, TMappedResult>(params object[] models) where TRequest : new()
    {
        var command = models != null && models.Any() ? MapperExtensions.Map<TRequest>(mapper, models) : new TRequest();
        var result = await mediator.Send(command!);
        var mappedResult = MapperExtensions.Map<TMappedResult>(mapper, result!);

        return mappedResult;
    }
}