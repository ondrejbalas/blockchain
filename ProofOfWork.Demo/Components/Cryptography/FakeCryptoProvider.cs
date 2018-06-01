using System.Security.Cryptography;

namespace ProofOfWork.Demo.Components.Cryptography
{
    /// <summary>
    /// Not for production use! Insecure!
    /// </summary>
    public class FakeCryptoProvider : ICryptoProvider
    {
        public void GenerateKeyPair(out byte[] privateKey, out byte[] publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(512))
            {
                privateKey = rsa.ExportCspBlob(true);
                publicKey = rsa.ExportCspBlob(false);
            }
        }

        public byte[] Sign(byte[] message, byte[] privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(privateKey);
                return rsa.SignData(message, new SHA256Managed());
            }
        }

        public bool Verify(byte[] message, byte[] publicKey, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(publicKey);
                return rsa.VerifyData(message, new SHA256Managed(), signature);
            }
        }
    }
}