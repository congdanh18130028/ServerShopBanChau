using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IAuthenticateServices _authenticateService;
        public LoginController(IAuthenticateServices authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _authenticateService.Authenticate(email, password);
            ActionResult response = Unauthorized();
            if (user != null)
            {
                var tokenStr = _authenticateService.GenerateJSONWebToken(user);
                response = Ok(new { token = tokenStr });
            }
            return response;
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var id = claim[0].Value;
            return  Ok(id);
        }

        

    }
}
