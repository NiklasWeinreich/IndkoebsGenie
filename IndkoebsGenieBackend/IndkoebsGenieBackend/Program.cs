using IndkoebsGenieBackend.Database.DatabaseContext;
using IndkoebsGenieBackend.Interfaces.IProductItem;
using IndkoebsGenieBackend.Repositories.ProductItemRepository;
using IndkoebsGenieBackend.Services.ProductItemService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
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

            // Dependency Injection
            builder.Services.AddScoped<IProductItemRepository, ProductItemRepository>();
            builder.Services.AddScoped<IProductItemService, ProductItemService>();

            // Controllers + JSON enum som string
            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Autorisation (skal til for at UseAuthorization virker)
            builder.Services.AddAuthorization();

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IndkoebsGenieBackend",
                    Version = "v1"
                });

                // Gør ProductCategory enum læsbar i Swagger
                c.MapType<IndkoebsGenieBackend.Database.Entities.ProductCategory>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(IndkoebsGenieBackend.Database.Entities.ProductCategory))
                        .Select(n => (IOpenApiAny)new OpenApiString(n))
                        .ToList()
                });
            });

            // CORS (til Angular dev-server)
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

            app.UseStaticFiles();

            app.UseRouting();

            // (Hvis du senere får auth, så skal app.UseAuthentication() stå før UseAuthorization)
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
