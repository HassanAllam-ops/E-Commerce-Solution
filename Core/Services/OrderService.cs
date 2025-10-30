using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exeptions;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using ServiceAbstraction;
using Services.Specifications;
using Shared.DataTransferObjects.IdentityModule;
using Shared.DataTransferObjects.OrderModule;

namespace Services
{
    internal class OrderService(IMapper _mapper , IBasketRepository _basketRepository , IUintOfWork _uintOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map AddressDto to OrderAddress
            var address = _mapper.Map<AddressDto,OrderAddress>(orderDto.Address);
            // Get Basket
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId);
            if (basket == null)
                throw new BasketNotFoundException(orderDto.BasketId);
            
            // Create Order Item List
            List<OrderItem> orderItems = new List<OrderItem>();
            var productRepo = _uintOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if (product is null)
                    throw new ProductNotFoundException(item.Id);
                var OrderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    },
                    Price = product.Price,
                    Quantity = item.Quantity
                };
                orderItems.Add(OrderItem);
            }
            // Get Delivery Method
            var deliveryMethod = await _uintOfWork.GetRepository<DeliveryMethod , int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod == null)
                throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            // Calculate Sub Total
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var order = new Order(email, address, deliveryMethod, orderItems, subTotal);

            await _uintOfWork.GetRepository<Order , Guid>().AddAsync(order);
            await _uintOfWork.SaveChangesAsync();

            return _mapper.Map<Order , OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodAsync()
        {
            var deliveryMethods = await _uintOfWork.GetRepository<DeliveryMethod , int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod> , IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var spec = new OrderSpecifications(email);
            var orders = await _uintOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<Order> , IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);
            var order = await _uintOfWork.GetRepository<Order , Guid>().GetByIdAsync(spec);
            return _mapper.Map<Order , OrderToReturnDto>(order);
        }
    }
}
