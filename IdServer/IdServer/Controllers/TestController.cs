using IdServer.Contracts;
using IdServer.Controllers.ErrorFiltering;
using IdServerCommon.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace IdServer.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    [Route("idserver/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private IAuthService AuthService { get; }
        private UserManager<IdentityUser> UserManager { get; }
        private RoleManager<IdentityRole> RoleManager { get; }

        public TestController(IAuthService authService, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            AuthService = authService;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetUsers() 
        {
            throw new Exception("что бы такого сделать плохого не хендлится кустомфильтером");
            return Ok(UserManager.Users);
        }


        [HttpGet]
        public IActionResult GetRoles()
        {
            return Ok(RoleManager.Roles);
        }


        [HttpGet]
        public async Task<IActionResult> TestRoleCapsAsync() 
        {
            var results = new List<string>();
            var username = "admin@admin";
            var user = await UserManager.FindByEmailAsync(username);

            results.Add("admin: " + await UserManager.IsInRoleAsync(user, "admin"));
            results.Add("Admin: " + await UserManager.IsInRoleAsync(user, "Admin"));
            results.Add("ADMIN: " + await UserManager.IsInRoleAsync(user, "ADMIN"));
            results.Add("adMin: " + await UserManager.IsInRoleAsync(user, "adMin"));
            results.Add("huilo: " + await UserManager.IsInRoleAsync(user, "huilo"));
            return Ok(results);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            
            IList<IdentityUser> asmins = await UserManager.GetUsersInRoleAsync(nameof(IdServerBasicRoles.Admin));
            return Ok(asmins);
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateDto user)
        {
            
            return Ok(await AuthService.CreateUser(user));
        }
    }
}