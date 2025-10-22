using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get Basket
        [HttpGet] // GET : baseurl/api/Basket
        public async Task<ActionResult<BasketDto>> GetBasket(string key)
        {
            var basket = await _serviceManager.BasketService.GetBasketAsync(key);
            return Ok(basket);
        }
        // Create or Update Basket
        [HttpPost] // POST : baseurl/api/Basket
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
        {
            var Basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }
        // Delete Basket
        [HttpDelete("{key}")] // DELETE : baseurl/api/Basket
        public async Task<ActionResult<bool>> DeleteBasket (string key)
        {
           var result = await _serviceManager.BasketService.DeleteBasketAsync(key);
              return Ok(result);
        }
    }
}
