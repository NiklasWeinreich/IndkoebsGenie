
using IndkoebsGenieBackend.Database.DatabaseContext;
using IndkoebsGenieBackend.Interfaces.IProductItem;
using IndkoebsGenieBackend.Repositories.ProductItemRepository;
using IndkoebsGenieBackend.Services.ProductItemService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace IndkoebsGenieBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------- Services ----------
            // DbContext (SQL Server)
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConString"));
            });


            builder.Services.AddScoped<IProductItemRepository, ProductItemRepository>();
            builder.Services.AddScoped<IProductItemService, ProductItemService>();

            // Controllers + JSON enum som string (valgfrit, men rart i API)
            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IndkoebsGenieBackend", Version = "v1" });
            });

            // CORS til Angular dev-server
            const string CorsPolicy = "CorsPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, policy =>
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .WithExposedHeaders("Content-Disposition"));
            });

            var app = builder.Build();

            // ---------- Middleware ----------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(CorsPolicy);

            app.UseStaticFiles();    // hvis du senere vil serve statiske filer

            app.UseRouting();

            // (Ingen auth endnu – kan tilføjes senere)
            // app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

