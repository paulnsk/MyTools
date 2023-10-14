using IdServer.Contracts;
using IdServer.Models;
using IdServerCommon.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;

namespace IdServer.Services
{
    public class AuthService : IAuthService
    {
        private AuthConfig Config { get; }
        private UserManager<IdentityUser> UserManager { get; }
        private RoleManager<IdentityRole> RoleManager { get; }

        public AuthService(IOptions<AuthConfig> options, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            Config = options.Value;
            UserManager = userManager;
            RoleManager = roleManager;
        }


        public async Task<IdentityResult> CreateUser(UserCreateDto user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Email,
                Email = user.Email,
            };

            var result = await UserManager.CreateAsync(identityUser, user.Password);
            await EnsureRoleExists(user.Role);
            await UserManager.AddToRoleAsync(identityUser, user.Role);
            return result;
        }
        

        private async Task EnsureRoleExists(string role)
        {
            if (!await RoleManager.RoleExistsAsync(role)) await RoleManager.CreateAsync(new IdentityRole { Name = role });
        }

        private async Task<IdentityUser> CheckUserExists(string userName)
        {
            var user = await UserManager.FindByEmailAsync(userName);
            if (user == null) throw new Exception($"user {userName} not found!");
            return user;
        }

        //todo IMPLEMENT:
                

        public async Task<IdentityResult> DeleteUser(string userName)
        {
            var user = await CheckUserExists(userName);

            var isAdmin = await UserManager.IsInRoleAsync(user, nameof(IdServerBasicRoles.Admin));
            if (isAdmin)
            {
                var adminCount = (await UserManager.GetUsersInRoleAsync(nameof(IdServerBasicRoles.Admin))).Count;
                if (adminCount == 1) throw new Exception($"{userName} is the last {IdServerBasicRoles.Admin} and cannot be deleted");
            }

            return await UserManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ChangePassword(string userName, string currentPassword, string newPassword)
        {
            await CheckUserExists(userName);
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> Login(string userName, string password)
        {
            await CheckUserExists(userName);
            throw new NotImplementedException();
        }

        public Task<AuthResponse> Refresh(string refreshToken)
        {
            throw new NotImplementedException();
        }



        //private string GenerateTokenString(AuthLogin user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new(ClaimTypes.Email, user.Email),
        //        new(ClaimTypes.Role, "Admin"),
        //    };

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConst.Key));

        //    var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        //    var securityToken = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(60),
        //        issuer: JwtConst.Issuer,
        //        audience: JwtConst.Audience,
        //        signingCredentials: signingCred);

        //    return new JwtSecurityTokenHandler().WriteToken(securityToken);
        //}



    }
}
