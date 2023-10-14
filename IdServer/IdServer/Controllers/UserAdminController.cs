using IdServer.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace IdServer.Controllers
{
    [Route("idserver/[controller]/[action]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private IAuthService AuthService { get; }

        public UserAdminController(IAuthService authService)
        {
            AuthService = authService;
        }

        //[HttpPost]
        //public ActionResult<>

    }
}
