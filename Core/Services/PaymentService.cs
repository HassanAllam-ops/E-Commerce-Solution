using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exeptions;
using DomainLayer.Models.BasketModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Services.Specifications;
using Shared.DataTransferObjects.BasketModule;
using Stripe;
using Stripe.Climate;

namespace Services
{
    internal class PaymentService(IConfiguration _configuration,
                                  IBasketRepository _basketRepository,
                                  IUintOfWork _uintOfWork,
                                  IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["Secretkey"];
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
            {
                throw new BasketNotFoundException(basketId);
            }

            var productRepo = _uintOfWork.GetRepository<DomainLayer.Models.ProductModule.Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if(product is null)
                {
                    throw new ProductNotFoundException(item.Id);
                }
                item.Price = product.Price;
            }

            if(basket.DeliveryMethodId is null)
            {
                throw new ArgumentNullException();
            }

            var deliveryMethod = await _uintOfWork.GetRepository<DeliveryMethod , int>()
                                 .GetByIdAsync(basket.DeliveryMethodId.Value);
            if(deliveryMethod is null)
            {
                throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            }
            basket.ShippingPrice = deliveryMethod.Price;

            var amount = (long)(basket.Items.Sum(I => I.Price * I.Quantity) + basket.ShippingPrice) * 100;

            var service = new PaymentIntentService();
            if(string.IsNullOrEmpty(basket.PaymentIntentId)) // Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "AED",
                    PaymentMethodTypes = ["card"]
                };

                var paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await service.UpdateAsync(basket.PaymentIntentId , options);    
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return _mapper.Map<Basket, BasketDto>(basket);

        }

        public async Task UpdateOrderPaymentStatusAsync(string request, string stripeHeader)
        {
            var endPoinSecret = _configuration.GetSection("Stripe")["EndPointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeader, endPoinSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentFailedAsync(paymentIntent.Id);
                    break;
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentRecievedAsync(paymentIntent.Id);
                    break;
                default:
                    Console.WriteLine($"Unhandled Stripe Event Type : {stripeEvent.Type}");
                    break;
            }
        }
        private async Task UpdatePaymentRecievedAsync(string paymentIntentId)
        {
            var order = await _uintOfWork.GetRepository<DomainLayer.Models.OrderModule.Order, Guid>()
                           .GetByIdAsync(new OrderWithPaymentIntentSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentReceived;
            _uintOfWork.GetRepository<DomainLayer.Models.OrderModule.Order, Guid>().Update(order);
            await _uintOfWork.SaveChangesAsync(); 
        }

        private async Task UpdatePaymentFailedAsync(string paymentIntentId)
        {
            var order = await _uintOfWork.GetRepository<DomainLayer.Models.OrderModule.Order, Guid>()
                           .GetByIdAsync(new OrderWithPaymentIntentSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentFailed;
            _uintOfWork.GetRepository<DomainLayer.Models.OrderModule.Order, Guid>().Update(order);
            await _uintOfWork.SaveChangesAsync();
        }
    }
}
