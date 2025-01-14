﻿namespace Logistics.Application.Handlers.Commands;

internal sealed class UpdateCargoHandler : RequestHandlerBase<UpdateCargoCommand, DataResult>
{
    private readonly ITenantRepository<Cargo> _cargoRepository;
    private readonly ITenantRepository<Truck> _truckRepository;

    public UpdateCargoHandler(
        ITenantRepository<Cargo> cargoRepository,
        ITenantRepository<Truck> truckRepository)
    {
        _cargoRepository = cargoRepository;
        _truckRepository = truckRepository;
    }

    protected override async Task<DataResult> HandleValidated(
        UpdateCargoCommand request, CancellationToken cancellationToken)
    {
        var truck = await _truckRepository.GetAsync(request.AssignedTruckId!);

        if (truck == null)
        {
            return DataResult.CreateError("Could not find the specified truck");
        }

        var cargoEntity = await _cargoRepository.GetAsync(request.Id!);

        if (cargoEntity == null)
        {
            return DataResult.CreateError("Could not find the specified cargo");
        }

        cargoEntity.Source = request.Source;
        cargoEntity.Destination = request.Destination;
        cargoEntity.TotalTripMiles = request.TotalTripMiles;
        cargoEntity.PricePerMile = request.PricePerMile;
        cargoEntity.IsCompleted = request.IsCompleted;
        cargoEntity.PickUpDate = request.PickUpDate;
        cargoEntity.Status = CargoStatus.Get(request.Status!);
        cargoEntity.AssignedTruck = truck;

        _cargoRepository.Update(cargoEntity);
        await _cargoRepository.UnitOfWork.CommitAsync();
        return DataResult.CreateSuccess();
    }

    protected override bool Validate(UpdateCargoCommand request, out string errorDescription)
    {
        errorDescription = string.Empty;

        if (string.IsNullOrEmpty(request.Id))
        {
            errorDescription = "Id is an empty string";
        }
        else if (string.IsNullOrEmpty(request.Source))
        {
            errorDescription = "Source address is an empty string";
        }
        else if (string.IsNullOrEmpty(request.Destination))
        {
            errorDescription = "Destination address is an empty string";
        }
        else if (string.IsNullOrEmpty(request.Status))
        {
            errorDescription = "Cargo status is not specified";
        }
        else if (string.IsNullOrEmpty(request.AssignedTruckId))
        {
            errorDescription = "AssignedTruckId is an empty string";
        }
        else if (request.PricePerMile < 0)
        {
            errorDescription = "Price per mile should be non-negative value";
        }
        else if (request.TotalTripMiles < 0)
        {
            errorDescription = "Total trip miles should be non-negative value";
        }

        return string.IsNullOrEmpty(errorDescription);
    }
}
