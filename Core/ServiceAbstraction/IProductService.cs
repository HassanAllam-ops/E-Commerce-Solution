using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.DataTransferObjects.ProductModule;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        // Get all products
        Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);
        // Get product by id
        Task<ProductDto> GetProductByIdAsync(int id);
        // Get All Brands
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        // Get All Types
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
    }
}
