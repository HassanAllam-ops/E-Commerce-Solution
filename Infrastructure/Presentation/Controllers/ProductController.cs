using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects.ProductModule;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // baseUrl: api/Products
    public class ProductController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get All Products
        [Authorize(Roles = "Admin")]
        [HttpGet]
        // GET: baseUrl/api/Products
        // Name Asc
        // Name Desc
        // Price Asc
        // Price Desc
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }
        //Get Products by Id
        [HttpGet("{id:int}")]
        // GET: baseUrl/api/Products/id
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }

        //Get All Brands
        [HttpGet("brands")]
        // GET: baseUrl/api/Products/brands
        public async Task<ActionResult<BrandDto>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }
        //Get All Types
        [HttpGet("types")]
        // GET: baseUrl/api/Products/types
        public async Task<ActionResult<TypeDto>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}
