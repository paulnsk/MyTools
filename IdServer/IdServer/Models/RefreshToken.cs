using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


namespace IdServer.Models
{
    [Index(nameof(Session), IsUnique = true)]
    public class RefreshToken
    {
        public int Id { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public string IdentityUserId { get; set; }
        
        public string Session { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
