using System;
using System.Security.Cryptography;

namespace Blockchain.Demo.Components.Cryptography
{
    /// <summary>
    /// Not for production use! Insecure!
    /// </summary>
    public class FakeEcdsaProvider : ICryptoProvider
    {
        public void GenerateKeyPair(out byte[] privateKey, out byte[] publicKey)
        {

            using (var ecdsa = new ECDsaCng(256))
            {
                privateKey = ecdsa.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
                publicKey = ecdsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
            }
        }

        public byte[] Sign(byte[] message, byte[] privateKey)
        {
            using (var ecdsa = new ECDsaCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob)))
            {
                return ecdsa.SignData(message, HashAlgorithmName.SHA1);
            }
        }

        public bool Verify(byte[] message, byte[] publicKey, byte[] signature)
        {
            try
            {
                using (var ecdsa = new ECDsaCng(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob)))
                {
                    return ecdsa.VerifyData(message, signature, HashAlgorithmName.SHA1);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}