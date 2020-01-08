using System;
using System.Security.Cryptography;

namespace api.obj
{
    public static class Helpers
    {
        public static string SecureRandomNumber(int byteCount = 69)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[byteCount];
            provider.GetBytes(byteArray);

            //convert 16 bytes to a hex string
            return BitConverter.ToString(byteArray, 0).Replace("-", "");
        }
    }
}