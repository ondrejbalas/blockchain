using System.Linq;
using ProofOfWork.Demo.Models;
using ProofOfWork.Demo.Rules.TransactionValidation;

namespace ProofOfWork.Demo.Rules.BlockValidation
{
    public class GenesisBlockValidator : IBlockValidator
    {
        private readonly ITransactionValidator _transactionValidator;

        public GenesisBlockValidator(ITransactionValidator transactionValidator)
        {
            _transactionValidator = transactionValidator;
        }

        public bool IsValid(Block block)
        {
            bool result = true;
            // RULES:
            // Has just one transaction
            var tx = block.Transactions.Single(); // Throws exception if anything other than one tx exists.

            result = result && tx.Type == BlockType.Genesis;

            result = result && block?.Type == BlockType.Genesis
                            && block.CreatedTimestamp == Program.GenesisTimeStamp
                            && block.Index == 0
                            && block.PreviousHash == 0
                            && block.Hash() == 0;

            return result;
        }
    }
}