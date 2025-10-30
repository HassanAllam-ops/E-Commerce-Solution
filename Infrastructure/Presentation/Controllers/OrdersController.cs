using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.OrderModule;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Create Order
        //[Authorize]
        [HttpPost] // POST: baseUrl/api/Orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManager.OrderService.CreateOrderAsync(orderDto, GetEmailFromToken() );  
            return Ok(order);
        }

        // Get Delivery Methods
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")] // GET: baseUrl/api/Orders/DeliveryMethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodAsync();
            return Ok(deliveryMethods);
        }

        // Get All Orders By Email
        //[Authorize]
        [HttpGet] // GET: baseUrl/api/Orders
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {
            var orders = await _serviceManager.OrderService.GetAllOrdersAsync(GetEmailFromToken());
            return Ok(orders);
        }

        // Get Order By Id
        //[Authorize]
        [HttpGet("{id:guid}")] // GET: baseUrl/api/Orders/{id}
        public ActionResult<OrderToReturnDto> GetOrderById(Guid id)
        {
            var order =  _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
    }
}
