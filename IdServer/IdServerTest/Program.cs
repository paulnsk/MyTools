using IdServer.Setup;

namespace IdServerTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var confog = new AuthConfig()
            //{
            //    DatabaseFilePath = "k:\\TEST.SQLITE"
            //};
            //Console.WriteLine(JsonSerializer.Serialize(confog));
            //Console.ReadKey();


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            //var path = @"k:\TEST.SQLITE";
            //var connectionString = $@"Data Source={path}";
            //builder.Services.AddDbContext<IdContext>(options => options.UseSqlite(connectionString));


            builder.Services.AddIdServer();
            //builder.Services.AddSqliteIdServerDatabase(@"k:\TEST.SQLITE");

            //builder.Services.AddAutoMapper(
            //Assembly.GetAssembly(
            //    typeof(IdServerMappingProfile))
            //    //, add your automapper profiles here
            //);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
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