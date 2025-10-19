using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models.ProductModule;
using Services.Specifications;
using Shared;

namespace Services
{
    internal class ProductCountSpecification : BaseSpecification<Product , int>
    {
        public ProductCountSpecification(ProductQueryParams queryParams)
            : base(P => (!queryParams.brandId.HasValue || P.BrandId == queryParams.brandId)
            && (!queryParams.typeId.HasValue || P.TypeId == queryParams.typeId)
            && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            
        }
    }
}
