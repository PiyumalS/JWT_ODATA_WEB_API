using System;
using System.Security.Cryptography;

namespace JWT_ODATA_WEB_API.Utils
{
    public static class PasswordGenerator
    {
        public static string Generate(int passwordLength, bool singleCase)
        {
            if (passwordLength == 0)
            {
                throw new InvalidOperationException("passwordLength");
            }

            string password;

            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[64];
                randomNumberGenerator.GetBytes(randomNumber);

                password = Convert.ToBase64String(randomNumber);
            }

            password = password.Substring(0, passwordLength);

            if (singleCase)
            {
                password = password.ToLower();
            }

            return password;
        }
    }
}