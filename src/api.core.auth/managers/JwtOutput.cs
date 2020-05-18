namespace api.core.auth.managers
{
    public class JwtOutput
    {
        public string Token { get; set; }
        public long ExpiresAt { get; set; }
    }
}