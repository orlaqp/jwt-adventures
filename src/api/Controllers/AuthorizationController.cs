using api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using api.core.auth.jwt;
using api.core.auth.managers;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

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
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,  "username"),
                new Claim(ClaimTypes.Role,  "admin"),
                new Claim(ClaimTypes.Country,  "United States"),
            };

            return Ok(jwtManager.GenerateToken(claims));
        }

        [HttpPost("validate")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ValidateToken([FromForm] string token) {
            var claimsPrincipal = jwtManager.ValidateToken(token);
            return Ok(new {
                Name = claimsPrincipal.Identity.IsAuthenticated,
                Schema = claimsPrincipal.Identity.AuthenticationType,
                Claims = claimsPrincipal.Claims.Select(c => new {
                    Issuer = c.Issuer,
                    Value = c.Value,
                    ValueType = c.ValueType
                })
            });
        }

        [HttpPost("asJson")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult AsJson([FromForm] string token) {
            return Ok(jwtManager.GetTokenAsJson(token));
        }
    }
}