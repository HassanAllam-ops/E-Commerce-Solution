using System.Threading.Tasks;
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegisteration
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            var Scope = app.Services.CreateScope();

            var Seed = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await Seed.DataSeedAsync();
            await Seed.IdentityDataSeedAsync();
        }   

        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
             app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
             return app;
        }
        public static IApplicationBuilder UseSwaggerMiddlewares(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
