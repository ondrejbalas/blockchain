using System;
using System.Security.Cryptography;

namespace ProofOfWork.Demo.Components.Hashing
{
    public class ShortSimpleHashProvider : ISimpleHashProvider
    {
        private static SHA256 sha256 = SHA256.Create();

        public uint CalculateHash(byte[] bytes)
        {
            return BitConverter.ToUInt32(sha256.ComputeHash(bytes), 0);
        }
    }
}