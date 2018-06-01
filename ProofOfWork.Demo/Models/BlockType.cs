namespace ProofOfWork.Demo.Models
{
    public enum BlockType : int
    {
        Normal = 0,
        ProofOfWork = 1,
        ProofOfStake = 2,
        Genesis = -1
    }
}