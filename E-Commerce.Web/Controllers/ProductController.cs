using E_Commerce.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    [Route("api/[controller]")] // baseUrl/api/Product
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET: baseUrl/api/Product/id
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            return new Product() { Id = id };
        }

        // GET: baseUrl/api/Product
        [HttpGet]
        public ActionResult<Product> GetAll()
        {
            return new Product() { Id = 100 };
        }

        // POST: baseUrl/api/Product
        [HttpPost]
        public ActionResult<Product> Add(Product product)
        {
            return product;
        }

        // PUT: baseUrl/api/Product
        [HttpPut]
        public ActionResult<Product> Update(Product product)
        {
            return product;
        }

        // DELETE: baseUrl/api/Product
        [HttpDelete]
        public ActionResult<Product> Delete(Product product)
        {
            return product;
        }
    }
}
