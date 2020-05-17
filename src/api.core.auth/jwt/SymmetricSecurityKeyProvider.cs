using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace api.core.auth.jwt
{
    public static class StmmetricSecurityKeyProvider
    {
        public static void ConfigureSymmetricAuthKey(this IServiceCollection services, string key) {
            services.AddSingleton<SymmetricSecurityKey>(provider => 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            );   
        }
    }
}