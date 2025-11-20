using GlobalAutoAPI.Services;
using GlobalAutoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace GlobalAutoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            //  adding the db context  
            builder.Services.AddDbContext<GlobalAutoDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionToGlobalAutoDB")));

            //  we add repository services  
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();

            // Replaced IUserRepository with IVehicleTypeRepository
            builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();

            // we add the automapper  
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}