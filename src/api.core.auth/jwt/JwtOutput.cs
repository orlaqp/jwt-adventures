namespace api.core.auth.jwt
{
    public class JwtOutput
    {
        public string Token { get; set; }
        public long ExpiresAt { get; set; }
    }
}