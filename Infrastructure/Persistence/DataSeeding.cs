using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                   await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    //var ProductBrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var ProductBrandsData =  File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandsData);
                    if (ProductBrands is not null && ProductBrands.Any())
                    {
                       await _dbContext.ProductBrands.AddRangeAsync(ProductBrands);
                    }
                }

                if (!_dbContext.ProductTypes.Any())
                {
                    var ProductTypesData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypesData);
                    if (ProductTypes is not null && ProductTypes.Any())
                    {
                       await _dbContext.ProductTypes.AddRangeAsync(ProductTypes);
                    }
                }

                if (!_dbContext.Products.Any())
                {
                    var ProductsData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                    if (Products is not null && Products.Any())
                    {
                       await _dbContext.Products.AddRangeAsync(Products);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                // To Do
            }
        }
    }
}
