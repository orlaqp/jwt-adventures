using Microsoft.Extensions.DependencyInjection;

namespace api.core.auth.jwt
{
    public static class JwtBearerAuthentication
    {
        public static void AddJwtBearerAuthentication(
            this IServiceCollection services,
            JwtValidationOptions config
        ) {
            services.ConfigureSymmetricAuthKey(config.Key);
            services.ConfigureRsaAuthKey(config.Key);
            services.ConfigureTokenValidation(config);
        }
    }
}