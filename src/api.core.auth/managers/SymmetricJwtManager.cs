using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace api.core.auth.managers
{
    public class SymmetricJwtManager : IJwtManager
    {
        private readonly byte[] base64Key;
        private SigningCredentials signingCredentials;

        public SymmetricJwtManager(IOptions<SymmetricOptions> options)
        {
            this.base64Key = Encoding.UTF8.GetBytes(options.Value.Key);
        }

        public JwtOutput GenerateToken(IEnumerable<Claim> claims)
        {
            var jwtDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                audience: "audience",
                issuer: "issuer",
                expires: jwtDate.AddDays(1),
                // claims
                claims: claims ?? new List<Claim>(),
                // Provide a cryptographic key used to sign the token.
                // When dealing with symmetric keys then this must be
                // the same key used to validate the token.
                signingCredentials: GetSigningCredentials()
            );

            // generate the actual token as a string
            return new JwtOutput
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresAt = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds()
            };
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadJwtToken(token);
            // securityToken.
            SecurityToken validatedToken;
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(base64Key)
            }, out validatedToken);
        }

        public string GetTokenAsJson(string token)
        {
            return ValidateToken(token).ToString();
        }

        private SigningCredentials GetSigningCredentials()
        {
            if (signingCredentials == null)
            {
                this.signingCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(base64Key),
                    algorithm: SecurityAlgorithms.HmacSha256
                );
            }

            return signingCredentials;
        }


    }
}