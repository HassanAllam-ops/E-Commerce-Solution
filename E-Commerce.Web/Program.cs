
using System.Threading.Tasks;
using Azure;
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using ServiceAbstraction;
using Services;
using Services.MappingProfiles;
using Shared.ErrorModels;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            builder.Services.AddInfrastructureService(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();
            #endregion

            var app = builder.Build();

            await app.SeedDataAsync();

            #region Configure the HTTP request pipeline
            // Configure the HTTP request pipeline.
            ///app.Use(async (RequestContext, NextMiddleware) =>
            ///{
            ///    Console.WriteLine("Request Under Processing");
            ///    await NextMiddleware.Invoke();
            ///    Console.WriteLine("Waiting Processing");
            ///    Console.WriteLine(RequestContext.Response.Body);
            ///});
            app.UseCustomExceptionMiddleware();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
