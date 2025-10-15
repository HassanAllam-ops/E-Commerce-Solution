using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using Shared;

namespace Services.Specifications
{
     class ProductWithBrandAndTypeSpecification : BaseSpecification<Product , int>
     {
        // Get All
        // 1. brandId == null && typeId == null  => true && true
        // 2. brandId == null && typeId == P.typeId  => true && typeId
        // 3. brandId == value && typeId == null  => P.brandId == brandId && true
        // 4. brandId == value && typeId == value  => P.brandId == brandId && P.typeId == typeId
        public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams)
            :base(P => (!queryParams.brandId.HasValue || P.BrandId == queryParams.brandId) 
            && (!queryParams.typeId.HasValue ||P.TypeId == queryParams.typeId)
            && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            switch(queryParams.sortingOption)
            {
                case ProductSortingOption.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOption.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOption.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOption.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                default:
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        // Get By Id
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
