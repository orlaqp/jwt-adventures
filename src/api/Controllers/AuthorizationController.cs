using System.Security.Claims;
using System.Collections.Generic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using api.core.auth.jwt;

namespace api.Controllers
{
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly JwtOptions jwtOptions;
        private readonly JwtManager jwtManager;

        public AuthorizationController(
            JwtManager jwtManager,
            IOptions<JwtOptions> jwtConfig
        )
        {
            this.jwtOptions = jwtConfig.Value;
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