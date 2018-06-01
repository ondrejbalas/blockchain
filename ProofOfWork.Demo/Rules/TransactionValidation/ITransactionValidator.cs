using ProofOfWork.Demo.Models;

namespace ProofOfWork.Demo.Rules.TransactionValidation
{
    public interface ITransactionValidator
    {
        bool IsValid(Transaction tx);
    }
}