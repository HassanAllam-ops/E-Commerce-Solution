using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using ServiceAbstraction;

namespace Services
{
    public class ServiceManager(IUintOfWork _uintOfWork , IMapper _mapper) : IServiceManager
    {
        private readonly Lazy<IProductService> _LazyProductService = new Lazy<IProductService>(() => new ProductService(_uintOfWork , _mapper));
        public IProductService ProductService => _LazyProductService.Value;
    }
}
