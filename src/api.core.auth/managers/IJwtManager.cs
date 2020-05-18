using System.Collections.Generic;
using System.Security.Claims;
using api.core.auth.jwt;

namespace api.core.auth.managers
{
    public interface IJwtManager
    {
        JwtOutput GenerateToken(IEnumerable<Claim> claims);

        ClaimsPrincipal ValidateToken(string token);

        string GetTokenAsJson(string token);
    }
}