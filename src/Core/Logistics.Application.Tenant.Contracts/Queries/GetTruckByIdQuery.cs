﻿namespace Logistics.Application.Contracts.Queries;

public sealed class GetTruckByIdQuery : RequestBase<DataResult<TruckDto>>
{
    public string? Id { get; set; }
}
