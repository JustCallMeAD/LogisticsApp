﻿namespace Logistics.Application.Contracts.Commands;

public sealed class CreateEmployeeCommand : RequestBase<DataResult>
{
    public string? ExternalId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
}
