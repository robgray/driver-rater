﻿namespace HelmetRanker.Features.HelmetPack.v1;

using AutoMapper;
using HelmetRanker.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class HelmetPackController : BaseController
{
    public HelmetPackController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("{userId:Guid}")]
    public Task<IActionResult> DownloadHelmetPackForUser(Guid userId)
    {
        // Downloads a helmet pack for just this user's input
        throw new NotImplementedException();
    }

    [HttpGet("all")]
    public Task<IActionResult> DownloadHelmetPackForAll()
    {
        // Downloads a helmet pack based on the average of all users inputs.
        throw new NotImplementedException();
    }
}