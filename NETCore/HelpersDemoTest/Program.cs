
using AspNetHelpers.Middleware.LogUrl;

namespace HelpersDemoTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //use config from appsettings, if present
            builder.Services.AddLogUrlMiddleware();

            //use config from appsettings, if present, and apply configAction
            //builder.Services.AddLogUrlMiddleware(x => x.LogRequestHeaders = false);
            
            var app = builder.Build();

            app.UseLogUrl();
            //app.UseLogUrl(x => x.LogRequestHeaders = false);


            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
