using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SoftwareAccountingService.Api.Data;
using SoftwareAccountingService.Api.Services;

namespace SoftwareAccountingService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString =
                builder.Configuration.GetConnectionString("PostgreSql")
                ?? throw new InvalidOperationException(
                    "Connection string 'PostgreSql' was not found.");

            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

          
            builder.Services.AddScoped<
                IInspectionObjectService,
                InspectionObjectService>();
           
            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(
                            allowIntegerValues: false));
                });

            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}