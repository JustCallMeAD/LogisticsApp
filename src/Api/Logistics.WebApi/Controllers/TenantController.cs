﻿namespace Logistics.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TenantController(
        IMapper mapper,
        IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("{identifier}")]
    [ProducesResponseType(typeof(DataResult<TenantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Policies.Tenant.CanRead)]
    public async Task<IActionResult> GetById(string? identifier)
    {
        var result = await _mediator.Send(new GetTenantQuery
        {
            Id = identifier,
            Name = identifier
        });

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("displayName/{identifier}")]
    [ProducesResponseType(typeof(DataResult<TenantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Policies.Tenant.CanRead)]
    public async Task<IActionResult> GetDisplayName(string? identifier)
    {
        var result = await _mediator.Send(new GetTenantDisplayNameQuery
        {
            Id = identifier,
            Name = identifier
        });

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(PagedDataResult<CargoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Policies.Tenant.CanRead)]
    public async Task<IActionResult> GetList([FromQuery] GetTenantsQuery request)
    {
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Policies.Tenant.CanWrite)]
    public async Task<IActionResult> Create([FromBody] TenantDto request)
    {
        var result = await _mediator.Send(_mapper.Map<CreateTenantCommand>(request));

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Policies.Tenant.CanWrite)]
    public async Task<IActionResult> Update(string id, [FromBody] TenantDto request)
    {
        var updateRequest = _mapper.Map<UpdateTenantCommand>(request);
        updateRequest.Id = id;
        var result = await _mediator.Send(updateRequest);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Policies.Tenant.CanWrite)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteTenantCommand
        {
            Id = id
        });

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}
