using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public abstract class BaseCRUDController<TGetAllQuery, TGetByIdQuery, TAddCommand, TUpdateCommand, TDeleteCommand> : BaseApiController
where TGetAllQuery : class
where TGetByIdQuery : class, IHasNumericId
where TAddCommand : class
where TUpdateCommand : class, IHasNumericId
where TDeleteCommand : class, IHasNumericId
{
    public BaseCRUDController()
    {
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] TGetAllQuery request, CancellationToken cancellationToken)
    {
        if (request != null)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request object.");
        }
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var request = (TGetByIdQuery)Activator.CreateInstance(typeof(TGetByIdQuery))!;
        if (request != null)
        {
            request.Id = id;
            return Ok(await Mediator.Send(request, cancellationToken));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request object.");
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TAddCommand request, CancellationToken cancellationToken)
    {
        if (request != null)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request object.");
        }
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] TUpdateCommand request, CancellationToken cancellationToken)
    {
        request.Id = id;
        if (request != null)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request object.");
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var request = (TDeleteCommand)Activator.CreateInstance(typeof(TDeleteCommand))!;
        if (request != null)
        {
            request.Id = id;
            return Ok(await Mediator.Send(request, cancellationToken));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request object.");
        }
    }
}
