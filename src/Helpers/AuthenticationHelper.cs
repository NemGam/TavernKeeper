using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Helpers
{
    public static class AuthenticationHelper
    {
        const int keySize = 64;
        const int iterations = 350000;
        static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public static string HashString(string password, byte[] salt)
        {
            return Convert.ToHexString(Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize));
        }
        public static string GenerateNewHashedString(string s, out string salt)
        {
            byte[] tempSalt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(s),
            tempSalt,
                iterations,
                hashAlgorithm,
                keySize);
            salt = Convert.ToHexString(tempSalt);
            return Convert.ToHexString(hash);
        }
    }
}
