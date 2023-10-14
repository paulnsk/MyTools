
using IdServerCommon.Dtos;
using Microsoft.AspNetCore.Identity;

namespace IdServer.Contracts
{
    public interface IAuthService
    {
        Task<IdentityResult> CreateUser(UserCreateDto user);
        Task<IdentityResult> DeleteUser(string userName);
        Task<IdentityResult> ChangePassword(string userName, string currentPassword, string newPassword);
        Task<AuthResponse> Login(string userName, string password);
        Task<AuthResponse> Refresh(string refreshToken);

    }
}
