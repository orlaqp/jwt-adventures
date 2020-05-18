using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace api.core.auth.managers
{
    public class AsymmetricJwtManager : IJwtManager
    {
        private readonly byte[] base64PrivateKey;
        private readonly byte[] base64PublicKey;
        private readonly RSA rsa;

        public AsymmetricJwtManager(IOptions<AsymmetricOptions> options)
        {
            this.base64PrivateKey = Convert.FromBase64String(options.Value.PrivateKey);
            this.base64PublicKey = Convert.FromBase64String(options.Value.PublicKey);
            this.rsa = GetRSAKey();
        }

        public JwtOutput GenerateToken(IEnumerable<Claim> claims)
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
                IssuerSigningKey = new RsaSecurityKey(rsa)
            }, out validatedToken);
        }

        public string GetTokenAsJson(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadJwtToken(token);

            return securityToken.ToString();
        }

        private SigningCredentials GetSigningCredentials()
        {
            return new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSsaPssSha256
            );
        }

        private RSA GetRSAKey() {
            var rsa = RSA.Create();
            
            rsa.ImportRSAPrivateKey(
                source: base64PrivateKey,
                bytesRead: out _
            );
            
            return rsa;
        }
    }
}