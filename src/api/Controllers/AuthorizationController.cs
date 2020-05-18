using api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using api.core.auth.jwt;
using api.core.auth.managers;

namespace api.Controllers
{
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IJwtManager jwtManager;

        public AuthorizationController(IJwtManager jwtManager)
        {
            this.jwtManager = jwtManager;
        }

        [HttpGet]
        public IActionResult GenToken() {
            return Ok(jwtManager.GenerateToken(jwtOptions.Symmetric.Key));
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult DecryptToken([FromForm] string token) {
            return Ok(jwtManager.DecryptToken(jwtOptions.Symmetric.Key, token));
        }
    }
}