using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System;

namespace api.core.auth.jwt
{
    public static class JwtBearerAuthProvider
    {
        public static void ConfigureTokenValidation(
            this IServiceCollection services,
            JwtValidationOptions config,
            Func<TokenValidatedContext, Task> onTokenValidated = null)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var provider = services.BuildServiceProvider();
                    SecurityKey securityKey = null;

                    if (config.SymmetricAlgorhitm)
                    {
                        securityKey = provider.GetRequiredService<SymmetricSecurityKey>();
                    }
                    else
                    {
                        securityKey = provider.GetRequiredService<RsaSecurityKey>();
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = false,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Issuer,
                        ValidAudience = config.Audience,
                        IssuerSigningKey = securityKey
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            else if (context.Exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
                            {
                                context.Response.Headers.Add("Token-Invalid-Signature", "true");
                            }
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = onTokenValidated ?? null                        
                    };

                });
        }


    }
}