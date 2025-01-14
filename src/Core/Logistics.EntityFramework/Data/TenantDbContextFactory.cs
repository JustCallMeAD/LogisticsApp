﻿using Microsoft.EntityFrameworkCore.Design;

namespace Logistics.EntityFramework.Data;

public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        var connectionString = ConnectionStrings.LocalDefaultTenant;
        return new TenantDbContext(connectionString);
    }
}