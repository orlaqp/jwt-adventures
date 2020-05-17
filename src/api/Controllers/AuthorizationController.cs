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
        private readonly IConfiguration configuration;
        private readonly JwtOptions jwtOptions;

        public AuthorizationController(
            IConfiguration configuration,
            IOptions<JwtOptions> jwtConfig
        )
        {
            this.configuration = configuration;
            this.jwtOptions = jwtConfig.Value;
        }

        [HttpGet]
        public IActionResult NewToken() {
            var base64Key = Encoding.UTF8.GetBytes(jwtOptions.Symmetric.Key);
            var signingCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(base64Key),
                algorithm: SecurityAlgorithms.HmacSha256
            );

            var jwtDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                audience: "audience",
                issuer: "issuer",
                // claims
                claims: new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, "orlando"),
                    new Claim(ClaimTypes.Role, "admin")
                },
                // Provide a cryptographic key used to sign the token.
                // When dealing with symmetric keys then this must be
                // the same key used to validate the token.
                signingCredentials: signingCredentials 
            );

            // generate the actual token as a string
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new {
                jwt = token,
                // Even if the expiration time is already a part of the token, it's common to be 
                // part of the response body.
                unixTimeExpiresAt = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds()
            });
        }
    }
}