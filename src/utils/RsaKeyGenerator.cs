using System;
using System.Security.Cryptography;

namespace utils
{
    public static class RsaKeyGenerator
    {
        public static void Generate() {
            using RSA rsa = RSA.Create();

            Console.WriteLine($"-----Private key-----{Environment.NewLine}{Convert.ToBase64String(rsa.ExportRSAPrivateKey())}{Environment.NewLine}");
            Console.WriteLine($"-----Public key-----{Environment.NewLine}{Convert.ToBase64String(rsa.ExportRSAPublicKey())}");        
        }
    }
}