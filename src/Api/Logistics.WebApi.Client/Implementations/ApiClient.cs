﻿using System.IdentityModel.Tokens.Jwt;

namespace Logistics.WebApi.Client.Implementations;

internal class ApiClient : ApiClientBase, IApiClient
{
    private string? _accessToken;
    private string? _currentTenant;
    
    public ApiClient(ApiClientOptions options) : base(options.Host!)
    {
        AccessToken = options.AccessToken;
    }

    public string? AccessToken
    {
        get => _accessToken;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            SetAuthorizationHeader("Bearer", value);
            SetCurrentTenantId(value);
            _accessToken = value;
        }
    }
    
    private void SetCurrentTenantId(string? accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return;
        }
        
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);
        var tenantId = token?.Claims?.FirstOrDefault(i => i.Type == "tenant")?.Value;
        
        if (_currentTenant == tenantId)
            return;
        
        _currentTenant = tenantId;
        SetRequestHeader("X-Tenant", tenantId);
    }

    #region Cargo API

    public async Task<CargoDto> GetCargoAsync(string id)
    {
        var result = await GetRequestAsync<DataResult<CargoDto>>($"cargo/{id}");
        return result.Value!;
    }

    public async Task<PagedDataResult<CargoDto>> GetCargoesAsync(string searchInput = "", int page = 1, int pageSize = 10)
    {
        var query = new Dictionary<string, string>
        {
            {"page", page.ToString() },
            {"pageSize", pageSize.ToString() }
        };

        if (!string.IsNullOrEmpty(searchInput))
        {
            query.Add("search", searchInput);
        }
        var result = await GetRequestAsync<PagedDataResult<CargoDto>>("cargo/list", query);
        return result;
    }

    public Task CreateCargoAsync(CargoDto cargo)
    {
        return PostRequestAsync("cargo/create", cargo);
    }
    
    public Task UpdateCargoAsync(CargoDto cargo)
    {
        return PutRequestAsync($"cargo/update/{cargo.Id}", cargo);
    }

    public Task DeleteCargoAsync(string id)
    {
        return DeleteRequestAsync($"cargo/{id}");
    }

    #endregion


    #region Truck API

    public async Task<TruckDto> GetTruckAsync(string id)
    {
        var result = await GetRequestAsync<DataResult<TruckDto>>($"truck/{id}");
        return result.Value!;
    }

    public async Task<TruckDto> GetTruckByDriverAsync(string driverId)
    {
        var result = await GetRequestAsync<DataResult<TruckDto>>($"truck/driver/{driverId}");
        return result.Value!;
    }

    public async Task<PagedDataResult<TruckDto>> GetTrucksAsync(string searchInput = "", int page = 1, int pageSize = 10, bool includeCargoIds = false)
    {
        var query = new Dictionary<string, string>
        {
            {"page", page.ToString() },
            {"pageSize", pageSize.ToString() },
            {"includeCargoIds", includeCargoIds.ToString() }
        };

        if (!string.IsNullOrEmpty(searchInput))
        {
            query.Add("search", searchInput);
        }
        var result = await GetRequestAsync<PagedDataResult<TruckDto>>("truck/list", query);
        return result;
    }

    public Task CreateTruckAsync(TruckDto truck)
    {
        return PostRequestAsync("truck/create", truck);
    }

    public Task UpdateTruckAsync(TruckDto truck)
    {
        return PutRequestAsync($"truck/update/{truck.Id}", truck);
    }

    public Task DeleteTruckAsync(string id)
    {
        return DeleteRequestAsync($"truck/{id}");
    }

    #endregion


    #region Employee API

    public async Task<EmployeeDto> GetEmployeeAsync(string id)
    {
        var result = await GetRequestAsync<DataResult<EmployeeDto>>($"employee/{id}");
        return result.Value!;
    }

    public async Task<EmployeeRoleDto> GetEmployeeRoleAsync(string userId)
    {
        var result = await GetRequestAsync<DataResult<EmployeeRoleDto>>($"employee/role/{userId}");
        return result.Value!;
    }

    public async Task<PagedDataResult<EmployeeDto>> GetEmployeesAsync(string searchInput = "", int page = 1, int pageSize = 10)
    {
        var query = new Dictionary<string, string>
        {
            {"page", page.ToString() },
            {"pageSize", pageSize.ToString() }
        };

        if (!string.IsNullOrEmpty(searchInput))
        {
            query.Add("search", searchInput);
        }
        var result = await GetRequestAsync<PagedDataResult<EmployeeDto>>("employee/list", query);
        return result;
    }

    public async Task<bool> EmployeeExistsAsync(string externalId)
    {
        try
        {
            await GetEmployeeAsync(externalId);
            return true;
        }
        catch (ApiException)
        {
            return false;
        }
    }

    public Task CreateEmployeeAsync(EmployeeDto user)
    {
        return PostRequestAsync("employee/create", user);
    }

    public async Task<bool> TryCreateEmployeeAsync(EmployeeDto user)
    {
        if (string.IsNullOrEmpty(user.ExternalId))
        {
            throw new ApiException("ExternalId is null or empty");
        }

        var userExists = await EmployeeExistsAsync(user.ExternalId);

        if (!userExists)
        {
            await CreateEmployeeAsync(user);
            return true;
        }

        return false;
    }

    public Task UpdateEmployeeAsync(EmployeeDto user)
    {
        return PutRequestAsync($"employee/update/{user.Id}", user);
    }

    #endregion


    #region Tenant API

    public async Task<string> GetTenantDisplayNameAsync(string identifier)
    {
        var result = await GetRequestAsync<DataResult<string>>($"tenant/displayName/{identifier}");
        return result.Value!;
    }

    public async Task<TenantDto> GetTenantAsync(string identifier)
    {
        var result = await GetRequestAsync<DataResult<TenantDto>>($"tenant/{identifier}");
        return result.Value!;
    }

    public async Task<PagedDataResult<TenantDto>> GetTenantsAsync(string searchInput = "", int page = 1, int pageSize = 10)
    {
        var query = new Dictionary<string, string>
        {
            {"page", page.ToString() },
            {"pageSize", pageSize.ToString() }
        };

        if (!string.IsNullOrEmpty(searchInput))
        {
            query.Add("search", searchInput);
        }
        var result = await GetRequestAsync<PagedDataResult<TenantDto>>("tenant/list", query);
        return result;
    }

    public Task CreateTenantAsync(TenantDto tenant)
    {
        return PostRequestAsync("tenant/create", tenant);
    }

    public Task UpdateTenantAsync(TenantDto tenant)
    {
        return PutRequestAsync($"tenant/update/{tenant.Id}", tenant);
    }

    public Task DeleteTenantAsync(string id)
    {
        return DeleteRequestAsync($"tenant/{id}");
    }

    #endregion

}
