using IdServer.Contracts;
using IdServer.Data;
using IdServer.Models;
using IdServer.Services;
using IdServerCommon.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IdServer.Setup
{
    public static class IdServerSetupExtensions
    {

        /// <summary>
        /// Adds support for identity server with local sqlite DB based on AuthConfig
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdServer(this IServiceCollection services)
        {
            services.AddOptions<AuthConfig>()
                .BindConfiguration(nameof(AuthConfig))
                .ValidateDataAnnotations();

            var config = services.BuildServiceProvider().GetRequiredService<IOptions<AuthConfig>>().Value;

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = config.PasswordRequiredLength;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<IdContext>()
                .AddDefaultTokenProviders();

            var dbPath = GetActualDbPath(config.DatabaseFilePath);
            var dbExisted = File.Exists(dbPath);

            AddSqliteIdServerDatabase(services, dbPath);

            services.AddScoped<IAuthService, AuthService>();

            if (!dbExisted) CreateFirstUser(services);

            return services;
        }

        private static string GetActualDbPath(string pathFromConfig)
        {
            if (pathFromConfig.ToLower() == "auto") pathFromConfig = AutoDbFilePath();
            return pathFromConfig;
        }

                
        private static IServiceCollection AddSqliteIdServerDatabase(IServiceCollection services, string databasePath)
        {            

            EnsureDirCreated(databasePath);

            var connectionString = $@"Data Source={databasePath}";
            services.AddDbContext<IdContext>(options => options.UseSqlite(connectionString));
            
            EnsureDbExistsAndUpToDate(services);
            

            return services;
        }

        
        private static void EnsureDirCreated(string filePath)
        {
            var dirPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dirPath)) return;
            if (string.IsNullOrWhiteSpace(dirPath)) throw new Exception("Unable to determine directory path for file path: " + filePath);
            Directory.CreateDirectory(dirPath);
            if (!Directory.Exists(dirPath)) throw new Exception($"Error creating dir {dirPath}");
        }

        private static void EnsureDbExistsAndUpToDate(IServiceCollection services)
        {            
            using var context = services.BuildServiceProvider().GetRequiredService<IdContext>();
            context.Database.Migrate();
        }

        private static void CreateFirstUser(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var authService = provider.GetRequiredService<IAuthService>();
            var logger= provider.GetRequiredService<ILogger<IAuthService>>();

            var email = "admin@admin";
            var password = "P@ss12345";
            authService.CreateUser(new UserCreateDto { Email = email, Password = password, Role = nameof(IdServerBasicRoles.Admin) }).GetAwaiter().GetResult();
            logger.LogInformation($"Empty DB has been created with a new user {email}:{password}. Considre deleting it after actual users are added");
        }

        private static string AutoDbFilePath()
        {
            var dir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName ?? "");
            if (string.IsNullOrWhiteSpace(dir)) throw new Exception("Unable to determine the location of the running process");
            return Path.Combine(dir, "Data\\users.sqlite");
        }

    }
}


