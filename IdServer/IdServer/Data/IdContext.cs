using IdServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdServer.Data
{
    public class IdContext : IdentityDbContext
    {

        /*
         
         MIGRATIONS:

        -. Set startup project to IdServerTest 
        
        -. Alt T N O, set default project to IdServer 
        
        -. Comment out all custom code in this IdContext and run
        
           Add-Migration I_BasicIdentityDbContext

        -. Uncomment custom code and run

           Add-Migration II_IdContextWithCustomFields
         
        This creates the test DB specified in "DatabaseFilePath": "o:\\STUDIO3\\Repos\\paulnsk\\MyTools\\IdServer\\IdServerTest\\DB\\users.sqlite"
        You may want to delete it so that it gets recreated when the app is first launched
         
         */


        public IdContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }



}
