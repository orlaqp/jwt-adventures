using System;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace api.core.auth.jwt
{
    public static class RsaSecurityKeyProvider
    {
        public static void ConfigureRsaAuthKey(this IServiceCollection services, string key) {
            services.AddSingleton<RsaSecurityKey>(provider => {
                var rsa = RSA.Create();

                if (key.IndexOf("BEGIN PUBLIC KEY") != -1) {
                    var cleanKey = key
                        .Replace("-----BEGIN PUBLIC KEY-----", "")
                        .Replace("-----END PUBLIC KEY-----", "");

                    rsa.ImportSubjectPublicKeyInfo(
                        source: Convert.FromBase64String(cleanKey),
                        bytesRead: out _
                    );
                } else if (key.IndexOf("BEGIN RSA PUBLIC KEY") != -1) {
                    var cleanKey = key
                        .Replace("-----BEGIN RSA PUBLIC KEY-----", "")
                        .Replace("-----END RSA PUBLIC KEY-----", "");

                    rsa.ImportRSAPublicKey(
                        source: Convert.FromBase64String(cleanKey),
                        bytesRead: out _
                    );
                }

                return new RsaSecurityKey(rsa);
            });   
        }
    }
}