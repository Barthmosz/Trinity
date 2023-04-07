﻿using SecureIdentity.Password;

namespace Trinity.Application.Wrappers
{
    public class PasswordHasherWrapper : IPasswordHasherWrapper
    {
        public bool Verify(string hash, string password, short keySize = 32, int iterations = 10000, char splitChar = '.', string privateKey = "")
        {
            return PasswordHasher.Verify(hash, password, keySize, iterations, splitChar, privateKey);
        }
    }
}
