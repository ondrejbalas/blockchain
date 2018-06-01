namespace ProofOfWork.Demo.Components.Cryptography
{
public interface ICryptoProvider
{
    void GenerateKeyPair(out byte[] privateKey, out byte[] publicKey);
    byte[] Sign(byte[] message, byte[] privateKey);
    bool Verify(byte[] message, byte[] publicKey, byte[] signature);
}
}