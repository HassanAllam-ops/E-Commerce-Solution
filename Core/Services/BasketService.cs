using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exeptions;
using DomainLayer.Models.BasketModule;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;

namespace Services
{
    internal class BasketService(IBasketRepository _basketRepository , IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var basketModel = _mapper.Map<BasketDto,Basket>(basket);
            var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(basketModel);
            if (createdOrUpdatedBasket is not null)
                return await GetBasketAsync(basket.Id);
            else
            throw new Exception("Can't Create or Update Basket Now , Try Again Later");
        }

        public async Task<bool> DeleteBasketAsync(string key)
        {
            return await _basketRepository.DeleteBasketAsync(key);
        }

        public async Task<BasketDto> GetBasketAsync(string key)
        {
            var basket = await _basketRepository.GetBasketAsync(key);
            if (basket == null)
                throw new BasketNotFoundException(key);
            return _mapper.Map<Basket,BasketDto>(basket);
        }
    }
}
