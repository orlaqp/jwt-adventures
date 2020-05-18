using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using api.core.auth.jwt;

namespace api.core.auth.managers
{
    public class AsymmetricJwtManager : IJwtManager
    {
        private readonly string key;
        private readonly byte[] base64Key;

        public JwtManager(string key)
        {
            this.key = key;
            this.base64Key = Encoding.UTF8.GetBytes(key);
        }

        public JwtOutput GenerateToken(string key)
        {
            var jwtDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                audience: "audience",
                issuer: "issuer",
                expires: jwtDate.AddDays(1),
                // claims
                claims: new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, "orlando"),
                    new Claim(ClaimTypes.Role, "admin")
                },
                // Provide a cryptographic key used to sign the token.
                // When dealing with symmetric keys then this must be
                // the same key used to validate the token.
                signingCredentials: GetSigningCredentials(key)
            );

            // generate the actual token as a string
            return new JwtOutput
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresAt = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds()
            };
        }

        public string DecryptToken(object key, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadJwtToken(token);
            // securityToken.
            SecurityToken validatedToken;
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(base64Key)
            }, out validatedToken);

            return securityToken.ToString();
        }

        private SigningCredentials GetSigningCredentials(bool symmetric)
        {

            if (symmetric)
            {
                return new SigningCredentials(
                    key: new SymmetricSecurityKey(base64Key),
                    algorithm: SecurityAlgorithms.HmacSha256
                );
            }
            else
            {
                var rsa = RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(
                    source: base64Key,
                    bytesRead: out _
                );

                var rsaKey = new RsaSecurityKey(rsa);
            }

        }

        public JwtOutput GenerateToken(IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            throw new NotImplementedException();
        }

        public string GetTokenAsJson(string token)
        {
            throw new NotImplementedException();
        }
    }
}