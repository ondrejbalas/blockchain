namespace Blockchain.Demo.Components.Hashing
{
    public interface ISimpleHashProvider
    {
        uint CalculateHash(byte[] data);
    }
}