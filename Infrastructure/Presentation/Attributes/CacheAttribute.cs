using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;

namespace Presentation.Attributes
{
    internal class CacheAttribute(int DurationInSec = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             // Create Cache Key
            string CacheKey = CreateCacheKey(context.HttpContext.Request);

            // Search for Value with Cache Key
            ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cachValue = await cacheService.GetAsync(CacheKey);

            // Return Value if Not Null 
            if(cachValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cachValue,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }
            // If Null
            //Invoke Next
            var ExecutedContext = await next.Invoke();
            // Set Value(Response) with Cache Key
            if(ExecutedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(CacheKey, result, TimeSpan.FromSeconds(DurationInSec));
            }
            // Return Value
        }
        private string CreateCacheKey(HttpRequest request)
        {
            // /api/ Product
            StringBuilder key = new StringBuilder();
            key.Append(request.Path);
            key.Append("?");
            foreach(var item in request.Query.OrderBy(Q => Q.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }

            return key.ToString();
        }
    }
}
