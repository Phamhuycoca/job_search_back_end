using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Helpers
{
    public static class PasswordHelper
    {
        public static string CreateHashedPassword(string password)
        {
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var passwordSalt = hmac.Key;
            return Base64Encode(passwordSalt) + "$" + Base64Encode(passwordHash);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var saltBytes = Base64Decode(hashedPassword.Split('$')[0]);
            var hashBytes = Base64Decode(hashedPassword.Split('$')[1]);

            using var hmac = new HMACSHA512(saltBytes);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != hashBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static string GenerateRandomString(int length, bool createGuid = true)
        {
            var randomstring = new string(Enumerable
                .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length)
                .Select(x =>
                {
                    var cryptoResult = new byte[4];
                    using (var cryptoProvider = RandomNumberGenerator.Create())
                        cryptoProvider.GetBytes(cryptoResult);
                    return x[new Random(BitConverter.ToInt32(cryptoResult, 0)).Next(x.Length)];
                })
                .ToArray());
            if (createGuid)
                return randomstring + Guid.NewGuid().ToString().Replace("-", "");
            else
                return randomstring;
        }

        private static string Base64Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        private static byte[] Base64Decode(string encodedData)
        {
            return Convert.FromBase64String(encodedData);
        }
    }
}
