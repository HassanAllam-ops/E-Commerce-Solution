
using DomainLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using ServiceAbstraction;
using Services;
using Services.MappingProfiles;

namespace E_Commerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IUintOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(config => config.AddProfile(new ProductProfile()), typeof(Services.AssemblyReference).Assembly);
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            #endregion

            var app = builder.Build();

            #region Data Seeding
            var Scope = app.Services.CreateScope();

            var Seed = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            Seed.DataSeedAsync();
            #endregion

            #region Configure the HTTP request pipeline
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
