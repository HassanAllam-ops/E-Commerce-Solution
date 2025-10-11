using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models;
using Shared.DataTransferObjects;

namespace Services.MappingProfiles
{
    public class ProductProfile :Profile
    {
        public ProductProfile()
        {
            CreateMap<Product , ProductDto>()
                .ForMember(Dist => Dist.BrandName , options => options.MapFrom(src => src.ProductBrand.Name))
                .ForMember(Dist => Dist.TypeName , options => options.MapFrom(src => src.ProductType.Name))
                .ForMember(Dist => Dist.PictureUrl , options => options.MapFrom<PictureUrlResolver>());
            
            CreateMap<ProductBrand , BrandDto>();
            CreateMap<ProductType , TypeDto>();
        }
    }
}
