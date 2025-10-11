using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration _connfiguration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.PictureUrl))
            {
                return string.Empty;
            }
            return $"{_connfiguration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
