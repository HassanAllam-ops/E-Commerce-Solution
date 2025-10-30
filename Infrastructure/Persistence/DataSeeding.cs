using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext,
                             UserManager<ApplicationUser> _userManager,
                             RoleManager<IdentityRole> _roleManager) : IDataSeeding
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

                if (!_dbContext.Set<DeliveryMethod>().Any())
                {
                    var DeliveryMethodsData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\delivery.json");
                    var DeliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliveryMethodsData);
                    if (DeliveryMethods is not null && DeliveryMethods.Any())
                    {
                        await _dbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethods);
                    }
                }



                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                // To Do
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Aly",
                        UserName = "MohamedAly",
                        PhoneNumber = "0123456789",
                    };
                    var user02 = new ApplicationUser()
                    {
                        Email = "Salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        UserName = "SalmaMohamed",
                        PhoneNumber = "01165423559",
                    };

                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "Admin");
                    await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                // To Do
            }
        }
    }
}
