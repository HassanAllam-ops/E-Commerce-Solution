using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObjects;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        // Get all products
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        // Get product by id
        Task<ProductDto?> GetProductByIdAsync(int id);
        // Get All Brands
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        // Get All Types
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
    }
}
