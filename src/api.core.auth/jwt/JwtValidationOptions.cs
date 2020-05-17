namespace api.core.auth.jwt
{
    public class JwtValidationOptions
    {
        public bool SymmetricAlgorhitm { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}