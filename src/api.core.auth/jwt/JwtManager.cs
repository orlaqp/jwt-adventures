using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace api.core.auth.jwt
{
    public class JwtManager
    {
        private readonly string key;
        private readonly byte[] base64Key;

        public JwtManager(string key)
        {
            this.key = key;
            this.base64Key = Encoding.UTF8.GetBytes(key);
        }

        public JwtOutput GenerateToken(string key) {
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
            return new JwtOutput {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresAt = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds()
            };            
        }

        public string DecryptToken(object key, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token).ToString();
        }
    }
}