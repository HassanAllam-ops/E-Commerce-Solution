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
        public void DataSeed()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var ProductBrandsData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var ProductBrands = JsonSerializer.Deserialize<List<ProductBrand>>(ProductBrandsData);
                    if (ProductBrands is not null && ProductBrands.Any())
                    {
                        _dbContext.ProductBrands.AddRange(ProductBrands);
                    }
                }

                if (!_dbContext.ProductTypes.Any())
                {
                    var ProductTypesData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    var ProductTypes = JsonSerializer.Deserialize<List<ProductType>>(ProductTypesData);
                    if (ProductTypes is not null && ProductTypes.Any())
                    {
                        _dbContext.ProductTypes.AddRange(ProductTypes);
                    }
                }

                if (!_dbContext.Products.Any())
                {
                    var ProductsData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    if (Products is not null && Products.Any())
                    {
                        _dbContext.Products.AddRange(Products);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                // To Do
            }
        }
    }
}
