#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;

namespace IdServer.Models
{    
    public class AuthConfig
    {
        private const int OneDayInSeconds = 3600 * 24;

        [Required]
        [MinLength(4)]
        public string DatabaseFilePath { get; set; }

        [Range(60, OneDayInSeconds * 3650)]
        public int RefreshTokenLifeTimeSeconds { get; set; } = OneDayInSeconds * 30;

        [Range(1, int.MaxValue)]
        public int MaxSessions { get; set; } = 1;

        [Range(3, 100)]
        public int PasswordRequiredLength { get; set; } = 5;        

        [Required]
        [MinLength(10)]
        public string JwtKey { get; set; } = "bad6661a35eb4831a2360a3687d56521";

        [Required]
        [MinLength(2)]
        public string JwtIssuer { get; set; } = "MyIdIssuer";

        [Required]
        [MinLength(2)]
        public string JwtAudience { get; set; } = "MyAudience";

        
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
