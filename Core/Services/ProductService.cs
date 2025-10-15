using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using ServiceAbstraction;
using Services.Specifications;
using Shared;
using Shared.DataTransferObjects;

namespace Services
{
    internal class ProductService(IUintOfWork _uintOfWork , IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _uintOfWork.GetRepository<ProductBrand, int>();
            var brands = await repo.GetAllAsync();
            var brandsDto = _mapper.Map<IEnumerable<ProductBrand>,IEnumerable<BrandDto>>(brands);
            return brandsDto;
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var repo = _uintOfWork.GetRepository<Product, int>();
            var specification = new ProductWithBrandAndTypeSpecification(queryParams);
            var products =await repo.GetAllAsync(specification);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var productCount = productsDto.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var totalCount = await repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDto>(productCount, queryParams.PageIndex, totalCount, productsDto);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var repo = _uintOfWork.GetRepository<ProductType, int>();
            var types = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>,IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var repo = _uintOfWork.GetRepository<Product, int>();
            var specification = new ProductWithBrandAndTypeSpecification(id);
            var product = await repo.GetByIdAsync(specification);
            if (product == null) 
                return null;
            return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
