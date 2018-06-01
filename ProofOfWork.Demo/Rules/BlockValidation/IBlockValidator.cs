using ProofOfWork.Demo.Models;

namespace ProofOfWork.Demo.Rules.BlockValidation
{
    public interface IBlockValidator
    {
        bool IsValid(Block block);
    }
}