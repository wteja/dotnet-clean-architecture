using Application.Exceptions;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public abstract class BaseApiController : ControllerBase
{
    private IMediator _mediator = null!;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
}