using Manager.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace Manager.Services.Providers.Hash
{
    public class HashProvider : IHashProvider
    {
        private readonly int _iterations = 100000;
        
        //public HashProvider()
        //{
        //    _iterations = 100000;
        //}

        public PayloadModel GenerateHash(string payload)
        {
            byte[] salt = GenerateSalt();
            string hashedPassword = Hash(payload, salt);

            return new PayloadModel
            {
                Salt = Convert.ToBase64String(salt),
                Hash = hashedPassword
            };
        }

        public bool VerifyHash(PayloadModel payload, string password)
        {
            byte[] salt = Convert.FromBase64String(payload.Salt);

            string hashedPassword = Hash(password, salt);

            return payload.Hash == hashedPassword;
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private string Hash(string payload, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: payload,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iterations,
                numBytesRequested: 256 / 8));
        }
    }
}
