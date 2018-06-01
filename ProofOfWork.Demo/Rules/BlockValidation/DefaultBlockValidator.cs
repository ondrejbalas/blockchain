using System;
using ProofOfWork.Demo.Models;

namespace ProofOfWork.Demo.Rules.BlockValidation
{
    public class DefaultBlockValidator : IBlockValidator
    {
        public bool IsValid(Block block)
        {
            bool valid = true;

            foreach (var transaction in block.Transactions)
            {
                valid = valid && Program.TransactionValidator.IsValid(transaction);
            }

            if (!valid) return false;

            if (block.Type != BlockType.Genesis)
            {
                valid = valid
                        && block.Index > 0
                        && block.CreatedTimestamp > block.PreviousBlock.CreatedTimestamp
                        && block.CreatedTimestamp < DateTime.Now.Ticks
                        && block.PreviousHash == block.PreviousBlock.Hash();
            }

            return valid;
        }
    }
}